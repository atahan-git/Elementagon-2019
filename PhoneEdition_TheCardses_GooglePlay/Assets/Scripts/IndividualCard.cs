using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualCard : MonoBehaviour {

	[SerializeField]
	CardBase _cBase;
	public CardBase cBase {
		get {
			return _cBase;
		}
		set {
			_cBase = value;
			UpdateGraphics ();
		}
	}

	public bool isOccupied { get { return myOccupants.Count > 0; } }
	public List<NPCBase> myOccupants = new List<NPCBase>();
	public bool isTargeted = false;

	public int x = -1;
	public int y = -1;

	public static int poisonCount = 0;

	[SerializeField]
	bool _isPoison = false;
	public bool isPoison{
		get{
			return _isPoison;
		}
		set{
			if (_isPoison) {
				poisonCount++;
			} else{
				poisonCount--;
			}
			_isPoison = value;
			//UpdateGraphics ();
		}
	}

	
	[HideInInspector]
	public bool isSelectable = true;
	[HideInInspector]
	public bool isUnselectable = true;

    //Animator anim;
    CardAnimator anim;

	[Space]

	public AnimatedSpriteController myFront;
	public SpriteRenderer myBack;

	// Use this for initialization
	void Awake () {
		anim = GetComponent<CardAnimator> ();
		isSelectable = true;
	}

	void Update (){
		if (Input.GetKeyDown (KeyCode.R)) {
			RandomizeCardType ();
		}
	}

	//-----------------------------------------Card Matching Functions
	public DataHandler.cardStates cardState = DataHandler.cardStates.close;

	public void KillOccupants () {
		if (myOccupants.Count > 0) {
			foreach (NPCBase npc in myOccupants) {
				if (npc != null)
					npc.Die ();
			}
		}
	}

	public void SelectCard (int playerID) {
		if (cBase == null) {
			ForceDeselectCard ();
			RequestcardType ();
		}
		anim.SetOpenState (true);
		isSelectable = false;
		isUnselectable = true;
		//CancelInvoke ();

		cardState = DataHandler.cardStates.open;
		SpawnEffect (cBase.onCard_SelectEffect != null ? cBase.onCard_SelectEffect : GS.a.gfxs.onCard_SelectEffect, playerID);
		SpawnEffectOnScoreBoard (cBase.onScoreBoard_SelectEffect != null ? cBase.onScoreBoard_SelectEffect : GS.a.gfxs.onScoreBoard_SelectEffect, playerID);
		SpawnEffectOnEnemyScoreBoards (cBase.onEnemySbs_SelectEffect != null ? cBase.onEnemySbs_SelectEffect : GS.a.gfxs.onEnemySbs_SelectEffect, playerID);
	}

	void ForceDeselectCard (){
		UnSelectCard ();
		DataHandler.s.NetworkCorrection (this);
		LocalPlayerController.s.ReceiveNetworkCorrection(this);
		PowerUpManager.s.ReceiveNetworkCorrection(this);
	}

	void RequestcardType (){
		DataHandler.s.SendRequest (DataHandler.RequestTypes.CardTypeRequest);
	}


	[HideInInspector]
	public GameObject selectedEffect {
		get{
			return _selectedEffect.gameObject;
		}set{
			DestroySelectedEfect ();
			if (value.GetComponent<SelectEffect> () == null)
				throw new System.NullReferenceException ("Given object isnt a select effect!");
			_selectedEffect = value.GetComponent<SelectEffect>();
			selectEffectID = _selectedEffect.id;
		}
	}

	SelectEffect _selectedEffect;

	public int selectEffectID;

	public void UnSelectCard () {
		   
		if (isUnselectable) {
			anim.SetOpenState (false);
			isSelectable = true;
			isUnselectable = false;
			DestroySelectedEfect ();

			cardState = DataHandler.cardStates.close;
		}
	}

	static public void MatchCards (int playerID, IndividualCard card1, IndividualCard card2) {
		SpawnEffectBetweenCards (card1.cBase.onCard_MatchEffectBetweenCards != null ? 
			card1.cBase.onCard_MatchEffectBetweenCards : GS.a.gfxs.onCard_MatchEffectBetweenCards, 
			-1, card1, card2);

		SpawnEffectBetweenCards (card1.cBase.onScoreBoard_MatchEffectBetweenCards != null ? 
			card1.cBase.onScoreBoard_MatchEffectBetweenCards : GS.a.gfxs.onScoreBoard_MatchEffectBetweenCards,
			playerID, card1, card2);

		for (int i = 0; i < ScoreBoardManager.s.scoreGetTargets.Length; i++) {
			if (i != playerID && ScoreBoardManager.s.scoreGetTargets[i] != null) {
				SpawnEffectBetweenCards (card1.cBase.onEnemySbs_MatchEffectBetweenCards != null ? 
					card1.cBase.onEnemySbs_MatchEffectBetweenCards : GS.a.gfxs.onEnemySbs_MatchEffectBetweenCards,
					i, card1, card2);
			}
		}
		card1.MatchCard (playerID);
		card2.MatchCard (playerID);
	}

	public void NetworkCorrectMatch () {
		MatchCard (-1);
	}

	protected void MatchCard (int playerID) {
		UnReveal ();

		anim.TriggerMatched ();
		//anim.SetOpenState (true);
		isSelectable = false;
		isUnselectable = false;
		_isPoison = false;
		DestroySelectedEfect ();
		Invoke ("ReOpenCard", GS.a.cardReOpenTime);
		   
		cardState = DataHandler.cardStates.matched;
		SpawnEffect (cBase.onCard_MatchEffect != null ? cBase.onCard_MatchEffect : GS.a.gfxs.onCard_MatchEffect, playerID);
		SpawnEffectOnScoreBoard (cBase.onScoreBoard_MatchEffect != null ? cBase.onScoreBoard_MatchEffect : GS.a.gfxs.onScoreBoard_MatchEffect, playerID);
		SpawnEffectOnEnemyScoreBoards (cBase.onEnemySbs_MatchEffect != null ? cBase.onEnemySbs_MatchEffect : GS.a.gfxs.onEnemySbs_MatchEffect, playerID);

		cBase = null;
	}

	void ReOpenCard () {
		   
		DestroySelectedEfect ();
		//anim.SetOpenState (false);
		anim.SetUnmatched ();
		Invoke ("RandomizeCardType", 0.35f);
		Invoke ("ReEnableSelection", 0.5f);

		cardState = DataHandler.cardStates.close;
	}

	public void NetherReset () {
		UnReveal ();

		DestroySelectedEfect ();
		isSelectable = false;
		isUnselectable = false;
		_isPoison = false;
		anim.TriggerJustRotate ();
		Invoke ("RandomizeCardType", 0.35f);
		Invoke ("ReEnableSelection", 0.5f);
		   
		cardState = DataHandler.cardStates.close;
	}

	public void PoisonMatch (){
		UnReveal ();

		anim.SetOpenState (false);
		isSelectable = false;
		isUnselectable = false;
		_isPoison = false;
		DestroySelectedEfect ();
		Invoke ("RandomizeCardType", 0.35f);
		Invoke ("ReEnableSelection", 0.5f);
	}

	GameObject myRevealEffect;
	public bool isRevealed;
	public void Reveal (float amount) {
		if (myRevealEffect != null)
			return;

		myRevealEffect = Instantiate (GS.a.gfxs.revealEffect, transform.position, Quaternion.identity);
		anim.isRevealed = true;
		isRevealed = true;

		Invoke ("UnReveal", amount);
	}

	void UnReveal () {
		CancelInvoke ("UnReveal");
		if(myRevealEffect != null)
			myRevealEffect.GetComponent<DisableAndDestroy> ().Engage ();
		myRevealEffect = null;
		anim.isRevealed = false;
		isRevealed = false;
	}

	public void PoisonProtect () {

	}

	//-----------------------------------------Utility Functions

	public void DestroySelectedEfect (){

		if (_selectedEffect != null)
			_selectedEffect.DestroyEffect ();
		_selectedEffect = null;
		selectEffectID = 0;
	}

	//GameObject activeOne;
	void UpdateGraphics () {
		CardBase theBase = cBase;

		if (theBase == null) {
			theBase = GS.a.defCard;
			//Debug.LogError ("The card " + x.ToString() + "," + y.ToString() + " is missing its cardBase!");
		}

		if (theBase.isAnimated) {
			myFront.SetAnimation (theBase.myAnim);
		} else {
			myFront.SetSprite (theBase.mySprite);
		}
	}

	public void UpdateCardType (int type) {
		   
		cBase = GS.a.cards[type].cBase;
		CancelInvoke("RandomizeCardType");

		if (DataHandler.s.myPlayerInteger == 0) {
			//DataLogger.LogMessage ("Master Card Type Send");
			DataHandler.s.SendCardType (x, y, type);
		}

		UpdateGraphics ();
	}

	void ReEnableSelection (){
		isSelectable = true;
		isUnselectable = true;
	}

	static float[] myChanceArray;
	public void RandomizeCardType () {
		   
		int type = 0;

		if (myChanceArray == null) {
			myChanceArray = new float[GS.a.cards.Length];

			for (int i = 0; i < GS.a.cards.Length; i++) {
				myChanceArray[i] = GS.a.cards[i].chance;
			}
		}

		type = RandFuncs.Sample (myChanceArray);

		UpdateCardType (type);   
	}

	void SpawnEffect (GameObject fx,int playerID) {
		if (fx != null) {
			GameObject myFx = Instantiate (fx, transform.position, Quaternion.identity);
			if (myFx.GetComponent<ElementalTypeSpriteColorChanger> () != null)
				myFx.GetComponent<ElementalTypeSpriteColorChanger> ().ChangeColor (cBase.elementType);
		}
	}

	static void SpawnEffectBetweenCards (GameObject fx, int playerID, IndividualCard card1, IndividualCard card2) {
		if (fx != null) {
			GameObject myFx = Instantiate (fx);
			if (myFx.GetComponent<BetweenCardsEffect> () != null)
				myFx.GetComponent<BetweenCardsEffect> ().SetUp (playerID, card1.cBase.elementType > 7, card1, card2);
			if (myFx.GetComponent<ElementalTypeSpriteColorChanger> () != null)
				myFx.GetComponent<ElementalTypeSpriteColorChanger> ().ChangeColor (card1.cBase.elementType);
		}
	}

	void SpawnEffectOnScoreBoard (GameObject fx, int playerID) {
		if (fx != null) {
			if (ScoreBoardManager.s.scoreGetTargets[playerID] != null) {
				GameObject myFx = Instantiate (fx, ScoreBoardManager.s.scoreGetTargets[playerID].position, Quaternion.identity);
				if (myFx.GetComponent<ElementalTypeSpriteColorChanger> () != null)
					myFx.GetComponent<ElementalTypeSpriteColorChanger> ().ChangeColor (cBase.elementType);
			}
		}
	}

	void SpawnEffectOnEnemyScoreBoards (GameObject fx, int playerID) {
		if (fx != null) {
			for (int i = 0; i < ScoreBoardManager.s.scoreGetTargets.Length; i++) {
				if (i != playerID) {
					if (ScoreBoardManager.s.scoreGetTargets[i] != null) {
						GameObject myFx = Instantiate (fx, ScoreBoardManager.s.scoreGetTargets[i].position, Quaternion.identity);
						if (myFx.GetComponent<ElementalTypeSpriteColorChanger> () != null)
							myFx.GetComponent<ElementalTypeSpriteColorChanger> ().ChangeColor (cBase.elementType);
					}
				}
			}
		}
	}

	
}


public abstract class BetweenCardsEffect : MonoBehaviour {
	public abstract void SetUp (int playerID, bool isPowerUp, IndividualCard card1, IndividualCard card2);

	public enum AlignMode { position, rotation, both };
	protected void AlignBetweenCards (IndividualCard card1, IndividualCard card2, AlignMode mode) {
		AlignBetweenCards (this.gameObject, card1, card2, mode);
	}
	 
	public static void AlignBetweenCards (GameObject obj, IndividualCard card1, IndividualCard card2, AlignMode mode) {
		Vector3 pos1 = card1.transform.position;
		Vector3 pos2 = card2.transform.position;
		if (pos1.x == pos2.x) {
			pos1.x += 0.01f;
		}

		if (pos1.x > pos2.x) {
			Vector3 temp = pos1;
			pos1 = pos2;
			pos2 = temp;
		}



		if (mode == AlignMode.position || mode == AlignMode.both) {
			obj.transform.position = Vector3.Lerp (pos1, pos2, 0.5f) + -Vector3.forward*0.1f;
		}

		if (mode == AlignMode.rotation || mode == AlignMode.both) {
			Vector3 lookVector = pos2 - pos1;
			Quaternion myRot = Quaternion.LookRotation (lookVector, Vector3.up);
			myRot = Quaternion.Euler (Mathf.Clamp (myRot.eulerAngles.x < 180 ? myRot.eulerAngles.x : (myRot.eulerAngles.x - 360f), -30f, 30f), myRot.eulerAngles.y, myRot.eulerAngles.z);

			obj.transform.rotation = myRot;
		}
	}
}
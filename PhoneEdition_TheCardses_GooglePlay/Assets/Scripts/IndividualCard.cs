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

	
	[HideInInspector]
	public bool isSelectable = true;
	[HideInInspector]
	public bool isUnselectable = true;
	[HideInInspector]
	public int currentSelectingPlayer = -1;

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

	//-----------------------------------------Card Matching Functions
	public DataHandler.cardStates cardState = DataHandler.cardStates.close;

	public void KillOccupants () {
		if (myOccupants.Count > 0) {
			foreach (NPCBase npc in myOccupants) {
				if (npc != null)
					npc.Die (false);
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
		currentSelectingPlayer = playerID;
		//CancelInvoke ();

		cardState = DataHandler.cardStates.open;
		SpawnEffect (cBase.onCard_SelectEffect != null ? cBase.onCard_SelectEffect : GS.a.gfxs.onCard_SelectEffect, playerID, cBase.effectColor);
		SpawnEffectOnScoreBoard (cBase.onScoreBoard_SelectEffect != null ? cBase.onScoreBoard_SelectEffect : GS.a.gfxs.onScoreBoard_SelectEffect, playerID, cBase.effectColor);
		SpawnEffectOnEnemyScoreBoards (cBase.onEnemySbs_SelectEffect != null ? cBase.onEnemySbs_SelectEffect : GS.a.gfxs.onEnemySbs_SelectEffect, playerID, cBase.effectColor);
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
			currentSelectingPlayer = -1;
			DestroySelectedEfect ();

			cardState = DataHandler.cardStates.close;
		}
	}

	static public void MatchCards (int playerID, IndividualCard card1, IndividualCard card2) {
		CardBase cbase = card1.cBase;
		if (playerID == DataHandler.NPCInteger) {
			if(cbase.npcMatchOverride != null)
			cbase = card1.cBase.npcMatchOverride;
		}

		if (cbase.onCard_MatchEffectBetweenCards != null)
			SpawnEffectBetweenCards (cbase.onCard_MatchEffectBetweenCards, playerID, card1, card2, cbase.effectColor);
		else
			SpawnEffectBetweenCards (GS.a.gfxs.onCard_MatchEffectBetweenCards, playerID, card1, card2, new Color ());

		if (cbase.onScoreBoard_MatchEffectBetweenCards != null)
			SpawnEffectBetweenCards (cbase.onScoreBoard_MatchEffectBetweenCards, playerID, card1, card2, cbase.effectColor);
		else
			SpawnEffectBetweenCards (GS.a.gfxs.onScoreBoard_MatchEffectBetweenCards, playerID, card1, card2, new Color ());

		for (int i = 0; i < ScoreBoardManager.s.scoreGetTargets.Length; i++) {
			if (i != playerID && ScoreBoardManager.s.scoreGetTargets[i] != null) {
				if (cbase.onEnemySbs_MatchEffectBetweenCards != null)
					SpawnEffectBetweenCards (cbase.onEnemySbs_MatchEffectBetweenCards, i, card1, card2, cbase.effectColor);
				else
					SpawnEffectBetweenCards (GS.a.gfxs.onEnemySbs_MatchEffectBetweenCards, i, card1, card2, new Color ());
			}
		}
		card1.MatchCard (playerID);
		card2.MatchCard (playerID);
	}

	public void NetworkCorrectMatch () {
		MatchCard (-1);
	}

	/// <summary>
	/// You shouldn't normally call this method from elsewhere, use the double method up there
	/// </summary>
	public void MatchCard () {
		MatchCard (-1);
	}

	protected void MatchCard (int playerID) {
		UnReveal ();

		anim.TriggerMatched ();
		//anim.SetOpenState (true);
		isSelectable = false;
		isUnselectable = false;
		currentSelectingPlayer = -1;
		DestroySelectedEfect ();
		Invoke ("ReOpenCard", GS.a.cardReOpenTime);
		   
		cardState = DataHandler.cardStates.matched;
		if (playerID == DataHandler.NPCInteger) {
			if (cBase.npcMatchOverride != null)
				_cBase = cBase.npcMatchOverride;
		}

		if(cBase.onCard_MatchEffect != null)
		SpawnEffect (cBase.onCard_MatchEffect, playerID, cBase.effectColor);
		else
			SpawnEffect (GS.a.gfxs.onCard_MatchEffect, playerID, new Color());

		if (cBase.onScoreBoard_MatchEffect != null)
			SpawnEffectOnScoreBoard (cBase.onScoreBoard_MatchEffect, playerID, cBase.effectColor);
		else
			SpawnEffectOnScoreBoard (GS.a.gfxs.onScoreBoard_MatchEffect, playerID, new Color ());

		if (cBase.onEnemySbs_MatchEffect != null)
			SpawnEffectOnEnemyScoreBoards (cBase.onEnemySbs_MatchEffect, playerID, cBase.effectColor);
		else
			SpawnEffectOnEnemyScoreBoards (GS.a.gfxs.onEnemySbs_MatchEffect, playerID, new Color ());
		
		cBase = GS.a.cardSet.matchedCard;
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
		currentSelectingPlayer = -1;
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
		currentSelectingPlayer = -1;
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
			theBase = GS.a.cardSet.defCard;
			Debug.LogError ("The card " + x.ToString() + "," + y.ToString() + " is missing its cardBase!");
		}

		if (theBase.isAnimated) {
			myFront.SetAnimation (theBase.myAnim);
		} else {
			myFront.SetSprite (theBase.mySprite);
		}
	}

	public void SetCardType (int type) {

		if (type < CardTypeRandomizer.s.allCards.Length && type >= 0) {
			cBase = CardTypeRandomizer.s.allCards[type];
		} else {
			DataLogger.LogError ("Illegal card type update request: " + type.ToString ());
			cBase = GS.a.cardSet.defCard;
		}
		CancelInvoke ("RandomizeCardType");

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


	public void RandomizeCardType () {
		SetCardType (CardTypeRandomizer.s.GiveRandomCardType());   
	}

	void SpawnEffect (GameObject fx, int playerID, Color effectColor) {
		if (effectColor.a <= 0.5f)
			effectColor = PowerUpManager.s.dragonColors[0];

		if (fx != null) {
			GameObject myFx = Instantiate (fx, transform.position, Quaternion.identity);
			if (myFx.GetComponent<ElementalTypeSpriteColorChanger> () != null)
				myFx.GetComponent<ElementalTypeSpriteColorChanger> ().ChangeColor (effectColor);
		}
	}

	static void SpawnEffectBetweenCards (GameObject fx, int playerID, IndividualCard card1, IndividualCard card2, Color effectColor) {
		if (card1.cBase.effectColor.a <= 0.5f)
			card1.cBase.effectColor = PowerUpManager.s.dragonColors[0];

		if (fx != null) {
			GameObject myFx = Instantiate (fx);
			if (myFx.GetComponent<BetweenCardsEffect> () != null) 
				myFx.GetComponent<BetweenCardsEffect> ().SetUp (playerID, card1.cBase.isPowerUpRelated, card1, card2);
			if (myFx.GetComponent<ElementalTypeSpriteColorChanger> () != null)
				myFx.GetComponent<ElementalTypeSpriteColorChanger> ().ChangeColor (card1.cBase.effectColor);
		}
	}

	void SpawnEffectOnScoreBoard (GameObject fx, int playerID, Color effectColor) {
		if (effectColor.a <= 0.5f)
			effectColor = PowerUpManager.s.dragonColors[0];

		if (fx != null) {
			if (ScoreBoardManager.s.scoreGetTargets[playerID] != null) {
				GameObject myFx = Instantiate (fx, ScoreBoardManager.s.scoreGetTargets[playerID].position, Quaternion.identity);
				if (myFx.GetComponent<ElementalTypeSpriteColorChanger> () != null)
					myFx.GetComponent<ElementalTypeSpriteColorChanger> ().ChangeColor (effectColor);
			}
		}
	}

	void SpawnEffectOnEnemyScoreBoards (GameObject fx, int playerID, Color effectColor) {
		if (effectColor.a <= 0.5f)
			effectColor = PowerUpManager.s.dragonColors[0];

		if (fx != null) {
			for (int i = 0; i < ScoreBoardManager.s.scoreGetTargets.Length; i++) {
				if (i != playerID) {
					if (ScoreBoardManager.s.scoreGetTargets[i] != null) {
						GameObject myFx = Instantiate (fx, ScoreBoardManager.s.scoreGetTargets[i].position, Quaternion.identity);
						if (myFx.GetComponent<ElementalTypeSpriteColorChanger> () != null)
							myFx.GetComponent<ElementalTypeSpriteColorChanger> ().ChangeColor (effectColor);
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
		print ("Aligning between " + card1.transform.position.ToString() + " and " + card2.transform.position.ToString());
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
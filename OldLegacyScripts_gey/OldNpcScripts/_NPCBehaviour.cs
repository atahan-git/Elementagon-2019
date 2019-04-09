using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _NPCBehaviour : MonoBehaviour {

	[HideInInspector]
	public float firstActivateTime = 5f;
	[HideInInspector]
	public float minTime = 5f;
	[HideInInspector]
	public float maxTime = 10f;

	[HideInInspector]
	public float factiveSelectTime = 7f;
	[HideInInspector]
	public float minSelectTime = 0.5f;
	[HideInInspector]
	public float maxSelectTime = 1f;
	[HideInInspector]
	public float checkTime = 0.5f;
	[HideInInspector]
	public float AIStrength = 0.5f;
	[HideInInspector]
	public bool poisonImmune = false;

	[HideInInspector]
	public bool isActive = true;

	[HideInInspector]
	public int type = 2;

	[HideInInspector]
	public IndividualCard[] mem_Cards = new IndividualCard[9];

	public GameObject normalSelectEffect;
	public Sprite SelectorObject;
	GameObject selObj;

	public static _NPCBehaviour activeNPC;
	public static bool isFrozen = false;

	void Awake (){
		activeNPC = this;
		selObj = new GameObject ();
		selObj.AddComponent<SpriteRenderer> ();
		selObj.GetComponent<SpriteRenderer> ().sprite = SelectorObject;
		Invoke ("pow", 0.1f);
	}


	void pow (){
		selObj.transform.localScale = CardHandler.s.allCards [2, 2].transform.localScale * 1.05f;
		selObj.transform.position = CardHandler.s.allCards [2, 2].transform.position + new Vector3 (0, 0, -0.2f);
	}

	Vector3 curPos = new Vector3();
	Vector3 endPos = new Vector3();

	void Update (){
		selObj.transform.position = Vector3.Lerp (selObj.transform.position, curPos + new Vector3 (0, 0, -0.2f), Time.deltaTime * 20f);
	}

	public void GoToPos (int x, int y, string target){
		StartCoroutine (_GoToPos (x, y,target));
	}

	public bool isGoToPosActive = false;
	int _x = 2;
	int _y = 2;
	IEnumerator _GoToPos (int x, int y, string target){
		isGoToPosActive = true;
		endPos = CardHandler.s.allCards [x, y].transform.position;

		while (x != _x || y != _y) {

			//print (_x + " - " + _y + " to " + x + " - " + y + " - " + target);

			if (x != _x && y != _y) {
				if (Random.value > 0.5f) {
					_x = (int)Mathf.MoveTowards (_x, x, 1);
				} else {
					_y = (int)Mathf.MoveTowards (_y, y, 1);
				}

			} else if (x != _x) {
				_x = (int)Mathf.MoveTowards (_x, x, 1);
			} else if (y != _y) {
				_y = (int)Mathf.MoveTowards (_y, y, 1);
			}
				
			curPos = CardHandler.s.allCards [_x, _y].transform.position;

			//print ("************");

			yield return new WaitForSeconds (Random.Range (0.2f, 0.25f));

			if (isFrozen)
				yield return new WaitForSeconds (0.5f);
		}
		isGoToPosActive = false;
	}

	public bool IsOnPos (){
		if (Vector3.Distance (selObj.transform.position, endPos) <= 0.5f) {
			return true;
		} else {
			return false;
		}
	}

	public void SetUp (float fatime, float mtime, float matime, int tpe, int memsize){

		firstActivateTime = fatime;
		minTime = mtime;
		maxTime = matime;
		type = tpe;
		mem_Cards = new IndividualCard[memsize];
		isActive = true;

		StartCoroutine (PowerUpLoop ());
	}

	public void NormalSelectSetUp (float fatime, float mtime, float matime, float cheTime, int tpe, float hardness, bool isPoisonImmune) {

		factiveSelectTime = fatime;
		minSelectTime = mtime;
		maxSelectTime = matime;
		type = tpe;
		checkTime = cheTime;
		AIStrength = hardness;
		poisonImmune = isPoisonImmune;

		StartCoroutine (NormalLoop ());
	}

	public IEnumerator PowerUpLoop (){

		yield return new WaitForSeconds (firstActivateTime);

		while (isActive) {
			Activate ();

			yield return new WaitForSeconds (Random.Range (minTime, maxTime));

			if (isFrozen)
				yield return new WaitForSeconds (1f);
		}
	}

	public IEnumerator NormalLoop (){

		yield return new WaitForSeconds (factiveSelectTime);

		while (isActive) {
			doneSelect = false;
			StartCoroutine (NormalSelect ());

			while (!doneSelect)
				yield return 0;
			
			if (normal_mem_Cards [1] == null)
				yield return new WaitForSeconds (Random.Range (0.2f, 0.3f));
			else
				yield return new WaitForSeconds (Random.Range (minSelectTime, maxSelectTime));

			if (isFrozen)
				yield return new WaitForSeconds (1f);
		}
	}

	public virtual void Activate (){
		//print ("Please override this method");
	}

	bool doneSelect = false;
	[HideInInspector]
	public IndividualCard[] normal_mem_Cards = new IndividualCard[2];
	public IEnumerator NormalSelect (){
		IndividualCard myCardS;
		if (Random.value > AIStrength) {
			myCardS = SelectRandomCard ();
		} else {
			if (normal_mem_Cards [0] != null) {
				myCardS = SelectCardType (normal_mem_Cards [0].cardType);
			} else {
				myCardS = SelectRandomCard ();
			}
		}

		while (!IsOnPosFar () || isGoToPosActive) {
			yield return 0;
		}


		GoToPos (myCardS.x, myCardS.y,"normal select");

		while (!IsOnPosFar ()) {
			yield return 0;
		}

		if (myCardS.isSelectable) {
			myCardS.isSelectable = false;
			yield return new WaitForSeconds (0.5f);
		} else {
			yield break;
		}

		if (myCardS.isPoison) {
			if (normal_mem_Cards [0] != null) {
				normal_mem_Cards [0].UnSelectCard ();
				normal_mem_Cards [0] = null;
			}

			PowerUpManager.s.ChoosePoisonCard (DataHandler.NPCInteger, myCardS, "npc");
		}

		if (normal_mem_Cards [0] == null) {
			normal_mem_Cards [0] = myCardS;
			if (normalSelectEffect != null)
				normal_mem_Cards [0].selectedEffect = (GameObject)Instantiate (normalSelectEffect, normal_mem_Cards [0].transform.position, Quaternion.identity);
			normal_mem_Cards [0].SelectCard ();
		} else {
			normal_mem_Cards [1] = myCardS;
			if (normalSelectEffect != null)
				normal_mem_Cards [1].selectedEffect = (GameObject)Instantiate (normalSelectEffect, normal_mem_Cards [1].transform.position, Quaternion.identity);
			normal_mem_Cards [1].SelectCard ();
			Invoke ("NormalCheckCards", checkTime);
		}
		doneSelect = true;
	}

	public void NormalCheckCards (){
		CardChecker.s.CheckNormal (DataHandler.NPCInteger, normal_mem_Cards);
	}




	public IndividualCard SelectRandomCard (){
		int gridSizeX = CardHandler.s.allCards.GetLength (0);
		int gridSizeY = CardHandler.s.allCards.GetLength (1);

		IndividualCard myCard;

		do {
			int x = Random.Range (0, gridSizeX);
			int y = Random.Range (0, gridSizeY);
			myCard = CardHandler.s.allCards [x, y];

		} while((!myCard.isSelectable || myCard.cardType > 7) || (poisonImmune && myCard.isPoison));

		return myCard;
	}

	public bool IsCardSelectable (IndividualCard myCard){
		if (!((!myCard.isSelectable || myCard.cardType > 7) || (poisonImmune && myCard.isPoison)))
			return true;
		else
			return false;
	}

	public IndividualCard SelectCardType (int type){
		int gridSizeX = CardHandler.s.allCards.GetLength (0);
		int gridSizeY = CardHandler.s.allCards.GetLength (1);

		IndividualCard myCard;

		int maxTryCount = 50;
		int c = 0;
		do {
			int x = Random.Range (0, gridSizeX);
			int y = Random.Range (0, gridSizeY);
			myCard = CardHandler.s.allCards [x, y];

			c++;
		} while((!myCard.isSelectable || myCard.cardType != type) && c < maxTryCount);

		return myCard;
	}

	public void SelectCard (IndividualCard myCard, int i){
		myCard.SelectCard ();
		mem_Cards[i] = myCard;
	}

	public void MoveAndSelectCard (IndividualCard myCard, int i){
		StartCoroutine (_MoveAndSelectCard (myCard, i));
	}
		

	IEnumerator _MoveAndSelectCard (IndividualCard myCard, int i){
		while (!IsOnPos ())
			yield return 0;

		GoToPos (myCard.x, myCard.y,"move and select");

		while (!IsOnPosFar ())
			yield return 0;

		if (myCard.isSelectable) {
			myCard.isSelectable = false;
			yield return new WaitForSeconds (0.5f);
			SelectCard (myCard, i);
		} else {
			MoveAndSelectCard (SelectRandomCard(),i);
		}
	}

	public bool IsOnPosFar (){
		if (Vector3.Distance (selObj.transform.position, endPos) <= 1f) {
			return true;
		} else {
			return false;
		}
	}

	public void CheckCards (){
		CardChecker.s.CheckPowerUp (DataHandler.NPCInteger, mem_Cards, type, CallBack);
	}

	public virtual void CallBack (){
	}
}

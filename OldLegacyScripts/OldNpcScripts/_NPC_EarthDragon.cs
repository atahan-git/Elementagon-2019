using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _NPC_EarthDragon : _NPCBehaviour {

	float effectActiveTime = 1.07f;

	public GameObject activatePrefab;
	public GameObject selectPrefab;

	// Use this for initialization
	void Start () {
		SetUp (0, 1, 1.5f, 1, 4);
	}

	bool isChecking = false;
	public override void Activate (){
		if(isFin)
			StartCoroutine (_Activate());
	}

	bool isFin = true;
	IEnumerator _Activate (){
		isFin = false;
		while (!IsOnPos ())
			yield return 0;

		if (isChecking) {
			isFin = true;
			yield break;
		}

		//check if we have done the first round
		int i = 0;
		if (mem_Cards [0] != null)
			i = 2;

		//select a random card
		IndividualCard myCard = SelectRandomCard();
		MoveAndSelectCard (myCard, i);

		while (!IsOnPos ())
			yield return 0;

		yield return new WaitForSeconds (0.51f);

		mem_Cards[i].selectedEffect = (GameObject)Instantiate (selectPrefab, myCard.transform.position, Quaternion.identity);
		Instantiate (activatePrefab, myCard.transform.position, Quaternion.identity);

		//select the other random card
		myCard = SelectRandomCard();
		SelectCard(myCard, i + 1);
		mem_Cards[i + 1].selectedEffect = (GameObject)Instantiate (selectPrefab, myCard.transform.position, Quaternion.identity);
		Instantiate (activatePrefab, myCard.transform.position, Quaternion.identity);

		if (i == 0) {
			if (mem_Cards [0].cardType == mem_Cards [1].cardType) {
				isChecking = true;
				Invoke ("CheckCards", GS.a.powerUpSettings.earth_checkSpeed*2);
			} else {
				Invoke ("Activate", 0.5f);
			}
		} else {
			isChecking = true;
			Invoke ("CheckCards", GS.a.powerUpSettings.earth_checkSpeed*2);
		}

		yield return new WaitForSeconds (0.5f);

		isFin = true;
	}

	public override void CallBack (){
		isChecking = false;
	}
}

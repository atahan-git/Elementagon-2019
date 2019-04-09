using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _NPC_ShadowDragon : _NPCBehaviour {

	public float shadowActiveTime = 5f;
	float shadowPickTime = 0.3f;

	public GameObject selectPrefab;
	public GameObject activatePrefab;

	// Use this for initialization
	void Start () {
		SetUp (0, 25, 30, 7, 2);
	}


	public override void Activate (){
		mem_Cards_List = new List<IndividualCard> ();
		StartCoroutine (ChooseLoop ());
	}

	IEnumerator ChooseLoop (){

		IndividualCard thisCard = SelectRandomCard ();

		while (!IsOnPos ())
			yield return 0;

		GoToPos (thisCard.x, thisCard.y,"Shadow");

		while (!IsOnPos ())
			yield return 0;

		if (thisCard.isSelectable) {
			thisCard.isSelectable = false;
			yield return new WaitForSeconds (0.5f);
		} else {
			StartCoroutine (ChooseLoop ());
			yield break;
		}

		ShadowSelectCard (thisCard);
		float addition = 0;
		float timeCounter = 0;
		while(timeCounter<shadowActiveTime){

			addition = Random.Range (shadowPickTime, shadowPickTime + 0.1f);
			
			yield return new WaitForSeconds (addition);


			thisCard = RelatedChoose (thisCard);

			while (!IsOnPos () || isGoToPosActive) {
				addition += Time.deltaTime;
				yield return 0;
			}

			GoToPos (thisCard.x, thisCard.y,"Shadow related");

			while (!IsOnPos ()) {
				addition += Time.deltaTime;
				yield return 0;
			}

			if (thisCard.isSelectable) {
				thisCard.isSelectable = false;
				ShadowSelectCard (thisCard);
			}
			
			timeCounter += addition;
		}

		addition = Random.Range (shadowPickTime - 0.1f, shadowPickTime + 0.2f);
		addition*=2;

		yield return new WaitForSeconds (addition);


		Disable ();
	}

	IndividualCard RelatedChoose (IndividualCard lastCard){
		IndividualCard toReturn = null;
		int x = -1;
		int y = -1;

		int count = 0;
		while (toReturn == null && count < 10) {
			switch (Random.Range (0, 4)) {
			case 0:	//go Left
				x = lastCard.x - 1;
				y = lastCard.y;
				try {
					if (x >= 0)
						toReturn = CardHandler.s.allCards [x, y];
				} catch {
				}
				break;
			case 1:	//go Up
				x = lastCard.x;
				y = lastCard.y + 1;
				try {
					if (y < CardHandler.s.grid.GetLength (1))
						toReturn = CardHandler.s.allCards [x, y];
				} catch {
				}
				break;
			case 2:	//go right
				x = lastCard.x + 1;
				y = lastCard.y;
				try {
					if (x < CardHandler.s.grid.GetLength (0))
						toReturn = CardHandler.s.allCards [x, y];
				} catch {
				}
				break;
			case 3:	//go down
				x = lastCard.x;
				y = lastCard.y - 1;
				try {
					if (y >= 0)
						toReturn = CardHandler.s.allCards [x, y];
				} catch {
				}
				break;
			default:

				break;
			}

			count++;
			if (toReturn != null) {
				if (!IsCardSelectable (toReturn))
					toReturn = null;
			}
		}

		if (toReturn == null)
			toReturn = SelectRandomCard ();

		return toReturn;
	}
	
	List<IndividualCard> mem_Cards_List = new List<IndividualCard>();
	public void ShadowSelectCard (IndividualCard myCard) {

		myCard.SelectCard ();
		mem_Cards_List.Add(myCard);
		myCard.selectedEffect = (GameObject)Instantiate (selectPrefab, myCard.transform.position, Quaternion.identity);
		Instantiate (activatePrefab, myCard.transform.position, Quaternion.identity);

	}

	public void Disable (){
		mem_Cards = mem_Cards_List.ToArray ();
		CheckCards ();
	}
}

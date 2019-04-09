using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _NPC_FireDragon : _NPCBehaviour {

	float effectActiveTime = 1.07f;

	public GameObject activatePrefab;

	public Vector2 activateTime = new Vector2 (5,10);

	// Use this for initialization
	void Start () {
		SetUp (0, activateTime.x, activateTime.y, 2, 9);
	}

	public override void Activate (){
		StartCoroutine (SelectSquareCards (SelectRandomCenter()));
	}

	int lastid = 0;
	IndividualCard SelectRandomCenter (){
		int id = lastid;
		while(id == lastid){
			id = Random.Range ((int)0, (int)6);
		}
		lastid = id;
		switch (id) {
		case 0:
			return CardHandler.s.allCards [1, 1];
			break;
		case 1:
			return CardHandler.s.allCards [1, 4];
			break;
		case 2:
			return CardHandler.s.allCards [4, 1];
			break;
		case 3:
			return CardHandler.s.allCards [4, 4];
			break;
		case 4: 
			return CardHandler.s.allCards [7, 1];
			break;
		case 5:
			return CardHandler.s.allCards [7, 4];
			break;
		}
		return CardHandler.s.allCards [7, 4];
	}
		
	IEnumerator SelectSquareCards (IndividualCard center){

		while (!IsOnPos ())
			yield return 0;

		GoToPos (center.x, center.y,"Fire");

		while (!IsOnPos ())
			yield return 0;

		yield return new WaitForSeconds (0.5f);

		Instantiate (activatePrefab, center.transform.position, Quaternion.identity);

		//yield return new WaitForSeconds (effectActiveTime);

		int gridSizeX = CardHandler.s.allCards.GetLength (0);
		int gridSizeY = CardHandler.s.allCards.GetLength (1);


		//get cards
		int leftLimit  = (int)Mathf.Clamp (center.x - 1, 0, gridSizeX - 1);
		int rightLimit = (int)Mathf.Clamp (center.x + 1, 0, gridSizeX - 1);
		int downLimit  = (int)Mathf.Clamp (center.y - 1, 0, gridSizeY - 1);
		int upLimit    = (int)Mathf.Clamp (center.y + 1, 0, gridSizeY - 1);

		int n = 0;
		for (int i = leftLimit; i <= rightLimit; i++) {
			for (int m = downLimit; m <= upLimit; m++) {

				IndividualCard myCardS = CardHandler.s.allCards [i, m];

				if (myCardS.cardType != 0) {
					if (myCardS.isSelectable) {

						SelectCard (myCardS, n);
						n++;

						yield return new WaitForSeconds (0.05f);
					}
				}
			}
		}

		yield return new WaitForSeconds (0.3f);

		CheckCards ();
	}
}

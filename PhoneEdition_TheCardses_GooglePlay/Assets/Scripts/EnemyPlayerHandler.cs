using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerHandler : MonoBehaviour {

	public static EnemyPlayerHandler s;

	public GameObject[] selectors = new GameObject[4];

	GameObject[] actSelectors = new GameObject[4];
	Vector3[] curPos = new Vector3[4];

	int playerCount = 2;

	// Use this for initialization
	void Awake () {
		s = this;

	}

	void Start (){
		playerCount = GoogleAPI.playerCount;

		actSelectors = new GameObject[4];
		curPos = new Vector3[4];

		/*if (GoogleAPI.gameMode == 1) {
			for (int i = 0; i < playerCount; i++) {
				actSelectors [i] = (GameObject)Instantiate (selectors [i], CardHandler.s.allCards [0, 0].transform.position, selectors [i].transform.rotation);
			}

			for (int i = 0; i < playerCount; i++) {
				curPos [i] =  CardHandler.s.allCards [0, 0].transform.position;
			}
		}*/
	}

	void Update (){
		/*if (GoogleAPI.gameMode == 1) {
			for (int i = 0; i < playerCount; i++) {
				actSelectors [i].transform.position = Vector3.Lerp (actSelectors [i].transform.position, curPos [i], 20f * Time.deltaTime);
			}
		}*/
	}


	public void ReceiveAction (char player, int x, int y, CardHandler.CardActions action, int x2, int y2){
		   

		int playerID = -1;
		IndividualCard myCard, matchPair = null;
		GameObject myEffect;

		switch (player) {
		case 'B':
			playerID = 0;
			myEffect = GS.a.gfxs.selectEffects[1];
			break;
		case 'R':
			playerID = 1;
			myEffect = GS.a.gfxs.selectEffects[2];
			break;
		case 'G':
			playerID = 2;
			myEffect = GS.a.gfxs.selectEffects[3];
			break;
		case 'Y':
			playerID = 3;
			myEffect = GS.a.gfxs.selectEffects[4];
			break;
		default:
			myEffect = GS.a.gfxs.selectEffects[1];
			break;
		}

		myCard = CardHandler.s.allCards [x, y];
		if(x2 != -1)
			matchPair = CardHandler.s.allCards[x2, y2];


		switch (action) {
		case CardHandler.CardActions.Select:
			if (myCard.isSelectable) {
				myCard.selectedEffect = (GameObject)Instantiate (myEffect, myCard.transform.position, Quaternion.identity);
				myCard.selectEffectID = DataHandler.s.toInt (player);
				myCard.SelectCard (playerID);
			} else {
				DataHandler.s.NetworkCorrection (myCard);
			}
			break;
		case CardHandler.CardActions.UnSelect:
			if (myCard.isUnselectable) {
				myCard.UnSelectCard ();
			} else {
				DataHandler.s.NetworkCorrection (myCard);
			}
			break;
		case CardHandler.CardActions.Match:
			if (!myCard.isSelectable && matchPair != null) {
				IndividualCard.MatchCards (playerID,myCard,matchPair);
			} else {
				DataHandler.s.NetworkCorrection (myCard);
				if(matchPair == null)
					DataHandler.s.NetworkCorrection (matchPair);
			}
			break;
		default:
			break;
		}
		   
	}


	//----------------------------------LOCAL PLAYER STUFF
	/*
	 * 			  1
	 * 			  |
	 * 		0---	---2
	 * 			 |
	 * 			 3
	 * 
	 */

	int x = 0;
	int y = 0;

	public void MoveWithDir (int dir){
		switch (dir) {
		case 0:
			x--;
			break;
		case 1:
			y++;
			break;
		case 2:
			x++;
			break;
		case 3:
			y--;
			break;
		}

		int gridSizeX = CardHandler.s.allCards.GetLength (0);
		int gridSizeY = CardHandler.s.allCards.GetLength (1);

		x = (int)Mathf.Clamp (x, 0, gridSizeX - 1);
		y = (int)Mathf.Clamp (y, 0, gridSizeY - 1);

		curPos [DataHandler.s.myPlayerInteger] = CardHandler.s.allCards [x, y].transform.position;

		DataHandler.s.SendMovementAction (DataHandler.s.toChar (DataHandler.s.myPlayerInteger), x, y);
	}

	public void SelectCard(){
		LocalPlayerController.s.SelectionCheck (CardHandler.s.allCards [x, y]);
	}

	//----------------------------------LOCAL PLAYER STUFF

	public void ReceiveMovementAction (int player, int x, int y){
		curPos [player] = CardHandler.s.allCards [x, y].transform.position;
	}
}

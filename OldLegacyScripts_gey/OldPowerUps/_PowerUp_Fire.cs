
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PowerUp_Fire : MonoBehaviour {

	public GameObject activatePrefab;
	float effectActiveTime = 1.07f;
	public GameObject indicatorPrefab;
	public GameObject scoreboardPrefab;
	GameObject indicator;

	//-----------------------------------------------------------------------------------------------Main Functions

	public void Enable () {
		   
		SendAction (-1, -1, PowerUpManager.ActionType.Enable);
		indicator = (GameObject)Instantiate (indicatorPrefab, ScoreBoardManager.s.indicatorParent);
		indicator.transform.ResetTransformation ();
		LocalPlayerController.s.PowerUpMode (true);
		PowerUpManager.s.canActivatePowerUp = false;
		   
	}


	IndividualCard[] mem_Cards = new IndividualCard[9];
	public void Activate (IndividualCard myCard) {
		   
		SendAction (myCard.x, myCard.y, PowerUpManager.ActionType.Activate);
		//LocalPlayerController.s.canSelect = false;
		StartCoroutine (SelectSquareCards (myCard));
		Disable ();
		   
	}
		

	public void Disable (){
		   
		SendAction (-1, -1, PowerUpManager.ActionType.Disable);
		indicator.GetComponent<DisableAndDestroy> ().Engage ();
		indicator = null;
		LocalPlayerController.s.PowerUpMode (false);
		PowerUpManager.s.canActivatePowerUp = true;
		LocalPlayerController.s.canSelect = true;
		   
	}


	//-----------------------------------------------------------------------------------------------Helper Functions
	IEnumerator SelectSquareCards (IndividualCard center){


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

	void SelectCard (IndividualCard myCard, int i){
		myCard.SelectCard ();
		mem_Cards[i] = myCard;
		SendAction (myCard.x, myCard.y, PowerUpManager.ActionType.SelectCard);
	}

	void CheckCards (){
		CardChecker.s.CheckPowerUp (mem_Cards, 2, CallBack);
	}

	public void CallBack (){
		//Disable ();
	}



	//-----------------------------------------------------------------------------------------------Networking

	GameObject[] network_scoreboard = new GameObject[4];
	public void ReceiveAction (int player, int x, int y, PowerUpManager.ActionType action) {

		try {
			switch (action) {
			case PowerUpManager.ActionType.Enable:
				network_scoreboard [player] = (GameObject)Instantiate (scoreboardPrefab, ScoreBoardManager.s.scoreBoards [player].transform);
				network_scoreboard [player].transform.ResetTransformation ();
				break;
			case PowerUpManager.ActionType.Activate:
				IndividualCard myCard = CardHandler.s.allCards [x, y];
				Instantiate (activatePrefab, myCard.transform.position, Quaternion.identity);
				break;
			case PowerUpManager.ActionType.SelectCard:
				IndividualCard myCard2 = CardHandler.s.allCards [x, y];
				if (myCard2.isSelectable) {
					myCard2.SelectCard ();
				} else {
					DataHandler.s.NetworkCorrection (myCard2);
				}
				break;
			case PowerUpManager.ActionType.Disable:
				if (network_scoreboard [player] != null)
					network_scoreboard [player].GetComponent<DisableAndDestroy> ().Engage ();
				network_scoreboard [player] = null;
				break;
			default:
				DataLogger.LogError ("Unrecognized power up action PUF");
				break;
			}
		} catch (System.Exception e) {
			DataLogger.LogError (this.name, e);
		}
	}

	void SendAction (int x, int y, PowerUpManager.ActionType action) {
		//DataHandler.s.SendPowerUpAction (x, y, PowerUpManager.PowerUpType.Fire, action);
	}

	//there exists only unselect, so unselect this card if we were selecting it wrongly
	public void ReceiveNetworkCorrection (IndividualCard myCard){
		for (int i = 0; i < 9; i++) {
			if (mem_Cards [i] == myCard) {
				mem_Cards [i] = null;
				myCard.DestroySelectedEfect ();
			}
		}
	}
}
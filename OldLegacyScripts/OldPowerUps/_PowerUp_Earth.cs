using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PowerUp_Earth : MonoBehaviour {


	public GameObject activatePrefab;
	public GameObject indicatorPrefab;
	public GameObject scoreboardPrefab;
	public GameObject selectPrefab;
	GameObject indicator;

	//-----------------------------------------------------------------------------------------------Main Functions

	public void Enable () {
		   
		SendAction (-1, -1, PowerUpManager.ActionType.Enable);

		counter = 0;

		indicator = (GameObject)Instantiate (indicatorPrefab, ScoreBoardManager.s.indicatorParent);
		indicator.transform.ResetTransformation ();
		LocalPlayerController.s.PowerUpMode (true);
		PowerUpManager.s.canActivatePowerUp = false;
		   
	}

	IndividualCard[] mem_Cards = new IndividualCard[4];
	bool isChecking = false;
	int counter = 0;
	public void Activate (IndividualCard myCard) {
		   

		if (isChecking)
			return;

		//check if we have done the first round
		int i = 0;
		if (mem_Cards [0] != null || mem_Cards[1] != null)
			i = 2;

		//select the card the player chose
		SelectCard (myCard, i);

		//select a random card
		SelectRandomCard (i + 1);

		if (i == 0) {
			if (mem_Cards [0].cardType == mem_Cards [1].cardType) {
				isChecking = true;
				Invoke ("CheckCards", GS.a.powerUpSettings.earth_checkSpeed);
			} else {
				LocalPlayerController.s.canSelect = true;
			}
		} else {
			isChecking = true;
			Invoke ("CheckCards", GS.a.powerUpSettings.earth_checkSpeed);
		}

		if (counter >= GS.a.powerUpSettings.earth_activeCount)
			Disable ();
		   
	}

	public void Disable (){
		   
		SendAction (-1, -1, PowerUpManager.ActionType.Disable);
		CheckCards ();
		indicator.GetComponent<DisableAndDestroy> ().Engage ();
		indicator = null;
		LocalPlayerController.s.PowerUpMode (false);
		PowerUpManager.s.canActivatePowerUp = true;
		   
	}


	//-----------------------------------------------------------------------------------------------Helper Functions

	void SelectRandomCard (int i){
		IndividualCard randomCard;
		do {
			randomCard = CardHandler.s.allCards [Random.Range (0, CardHandler.s.allCards.GetLength (0)), Random.Range (0, CardHandler.s.allCards.GetLength (1))].GetComponent<IndividualCard>();
		} while(randomCard.cardType == 0 || randomCard.isSelectable == false);
		SelectCard (randomCard, i);
	}

	void SelectCard (IndividualCard myCard, int i){
		myCard.SelectCard ();
		mem_Cards[i] = myCard;
		mem_Cards[i].selectedEffect = (GameObject)Instantiate (selectPrefab, myCard.transform.position, Quaternion.identity);
		mem_Cards [i].selectEffectID = 1 + 4 + 1;
		Instantiate (activatePrefab, myCard.transform.position, Quaternion.identity);
		SendAction (myCard.x, myCard.y, PowerUpManager.ActionType.Activate);
	}

	void CheckCards () {
		//CardChecker.s.CheckCards (mem_Cards, false);
		isChecking = false;
		LocalPlayerController.s.canSelect = true;

		counter++;
	}



	//-----------------------------------------------------------------------------------------------Networking

	GameObject[] network_scoreboard = new GameObject[4];
	public void ReceiveAction (int player, int x, int y, PowerUpManager.ActionType action) {
		   
		switch (action) {
		case PowerUpManager.ActionType.Enable:
			network_scoreboard [player] = (GameObject)Instantiate (scoreboardPrefab, ScoreBoardManager.s.scoreBoards [player].transform);
			network_scoreboard [player].transform.ResetTransformation ();
			break;
		case PowerUpManager.ActionType.Activate:
			IndividualCard myCard = CardHandler.s.allCards [x, y];
			if (myCard.isSelectable) {
				myCard.SelectCard ();
				myCard.selectedEffect = (GameObject)Instantiate (selectPrefab, myCard.transform.position, Quaternion.identity);
			} else {
				DataHandler.s.NetworkCorrection (myCard);
			}
			break;
		case PowerUpManager.ActionType.Disable:
			if (network_scoreboard [player] != null)
				network_scoreboard [player].GetComponent<DisableAndDestroy> ().Engage ();
			network_scoreboard [player] = null;
			break;
		default:
			DataLogger.LogError ("Unrecognized power up action PUE");
			break;
		}
		   
	}

	void SendAction (int x, int y, PowerUpManager.ActionType action) {
		//DataHandler.s.SendPowerUpAction (x, y, PowerUpManager.PowerUpType.Earth, action);
	}

	//there exists only unselect, so unselect this card if we were selecting it wrongly
	public void ReceiveNetworkCorrection (IndividualCard myCard){
		for (int i = 0; i < 4; i++) {
			if (mem_Cards [i] == myCard) {
				mem_Cards [i] = null;
				myCard.DestroySelectedEfect ();
			}
		}
	}
}

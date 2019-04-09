using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PowerUp_Poison : MonoBehaviour {


	public GameObject activatePrefab;
	public GameObject indicatorPrefab;
	public GameObject scoreboardPrefab;
	public GameObject selectPrefab;
	public GameObject scoreLowerExplosionPrefab;
	GameObject indicator;

	//-----------------------------------------------------------------------------------------------Main Functions

	public void Enable () {
		   
		SendAction (-1, -1, PowerUpManager.ActionType.Enable);
		indicator = (GameObject)Instantiate (indicatorPrefab, ScoreBoardManager.s.indicatorParent);
		indicator.transform.ResetTransformation ();
		LocalPlayerController.s.PowerUpMode (true);
		PowerUpManager.s.canActivatePowerUp = false;
		   
	}
		
	public void Activate (IndividualCard myCard) {
		StartCoroutine (_Activate (myCard));
	}

	IEnumerator _Activate (IndividualCard myCard){
		
		SendAction (myCard.x, myCard.y, PowerUpManager.ActionType.Activate);

		myCard.SelectCard ();
		myCard.cardType = 15;
		myCard.isPoison = true;
		myCard.selectedEffect = (GameObject)Instantiate (selectPrefab, myCard.transform.position, Quaternion.identity);


		indicator.GetComponent<DisableAndDestroy> ().Engage ();
		indicator = null;
		LocalPlayerController.s.PowerUpMode (false);
		PowerUpManager.s.canActivatePowerUp = true;
		LocalPlayerController.s.canSelect = true;


		yield return new WaitForSeconds (GS.a.powerUpSettings.poison_activeTime);


		myCard.UnSelectCard ();

	}
		

	public void ChoosePoisonCard (int myPlayerinteger, IndividualCard myCard, string message){
		SendAction (myCard.x, myCard.y, PowerUpManager.ActionType.Disable);
		StartCoroutine (_ChoosePoisonCard (myPlayerinteger, myCard));
		//print (message);
	}
		
	IEnumerator _ChoosePoisonCard (int myPlayerinteger, IndividualCard myCard){

		bool temp = true;
		if (myPlayerinteger == DataHandler.s.myPlayerInteger) {
			//LocalPlayerController.s.canSelect = false;
			//temp = PowerUpManager.s.canActivatePowerUp;
			//PowerUpManager.s.canActivatePowerUp = false;
		}

		myCard.SelectCard ();
		Instantiate (activatePrefab, myCard.transform.position, Quaternion.identity);
		myCard.selectedEffect = (GameObject)Instantiate (selectPrefab, myCard.transform.position, Quaternion.identity);
		myCard.isPoison = false;

		print ("poison coroutine started");

		yield return new WaitForSeconds (GS.a.powerUpSettings.poison_checkSpeed /4f);

		for (int i = 0; i < GS.a.powerUpSettings.poison_damage; i++) {
			ScoreBoardManager.s.AddScore (myPlayerinteger, 0, -1, false);
			GameObject exp = (GameObject)Instantiate (scoreLowerExplosionPrefab, ScoreBoardManager.s.scoreBoards [myPlayerinteger].transform);
			exp.transform.ResetTransformation ();

			yield return new WaitForSeconds ((GS.a.powerUpSettings.poison_checkSpeed / 2f) / (float)GS.a.powerUpSettings.poison_damage);
		}

		yield return new WaitForSeconds (GS.a.powerUpSettings.poison_checkSpeed /4f);

		if (myPlayerinteger == DataHandler.s.myPlayerInteger) {
			//LocalPlayerController.s.canSelect = true;
			//PowerUpManager.s.canActivatePowerUp = temp;
		}
		myCard.PoisonMatch ();

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
			if(myCard.isSelectable){
				StartCoroutine (NetworkActivate (player, myCard));
			} else {
				DataHandler.s.NetworkCorrection (myCard);
			}
			break;
		case PowerUpManager.ActionType.Disable:
			IndividualCard myCard2 = CardHandler.s.allCards [x, y];
			StartCoroutine (NetworkDisable (player, myCard2));
			break;
		default:
			DataLogger.LogError ("Unrecognized power up action PUP");
			break;
		}
		   
	}

	IEnumerator NetworkActivate (int player, IndividualCard myCard){
		myCard.SelectCard ();
		myCard.cardType = 15;
		myCard.isPoison = true;
		myCard.selectedEffect = (GameObject)Instantiate (selectPrefab, myCard.transform.position, Quaternion.identity);

		yield return new WaitForSeconds (GS.a.powerUpSettings.poison_activeTime);


		if (network_scoreboard [player] != null)
			network_scoreboard [player].GetComponent<DisableAndDestroy> ().Engage ();
		network_scoreboard [player] = null;

		myCard.UnSelectCard ();
	}

	IEnumerator NetworkDisable (int player, IndividualCard myCard){
		myCard.SelectCard ();
		Instantiate (activatePrefab, myCard.transform.position, Quaternion.identity);
		myCard.selectedEffect = (GameObject)Instantiate (selectPrefab, myCard.transform.position, Quaternion.identity);

		yield return new WaitForSeconds (GS.a.powerUpSettings.poison_checkSpeed /4f);

		for (int i = 0; i < GS.a.powerUpSettings.poison_damage; i++) {
			GameObject exp = (GameObject)Instantiate (scoreLowerExplosionPrefab, ScoreBoardManager.s.scoreBoards [player].transform);
			exp.transform.ResetTransformation ();

			yield return new WaitForSeconds ((GS.a.powerUpSettings.poison_checkSpeed / 2f) / (float)GS.a.powerUpSettings.poison_damage);
		}

		yield return new WaitForSeconds (GS.a.powerUpSettings.poison_checkSpeed / 4f);

		myCard.PoisonMatch ();

	}

	void SendAction (int x, int y, PowerUpManager.ActionType action) {
		//DataHandler.s.SendPowerUpAction (x, y, PowerUpManager.PowerUpType.Poison, action);
	}

	//there exists only unselect, so unselect this card if we were selecting it wrongly
	public void ReceiveNetworkCorrection (IndividualCard myCard){
		StopCoroutine ("_Activate");
	}
}
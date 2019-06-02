using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PowerUp_Light : MonoBehaviour {


	public GameObject cantSelectPrefab;
	public GameObject affectedPrefab;
	public GameObject affectorPrefab;

	//-----------------------------------------------------------------------------------------------Main Functions

	GameObject[] scoreBoardObjects = new GameObject[4];
	public void Enable () {
		   
		SendAction (-1, -1, PowerUpManager.ActionType.Enable);
		Invoke ("Disable", GS.a.powerUpSettings.light_activeTime);


		//we are the player who activated the effect so we dont freeze but others do

		//spawn freeze effect for the frozen players
		for (int i = 0; i < ScoreBoardManager.s.scoreBoards.Length; i++) {
			if (i != DataHandler.s.myPlayerInteger) {
				if (scoreBoardObjects [i]) {
					Destroy (scoreBoardObjects [i]);
					scoreBoardObjects [i] = null;
				}
				if (ScoreBoardManager.s.scoreBoards [i]) {
					scoreBoardObjects [i] = (GameObject)Instantiate (affectedPrefab, ScoreBoardManager.s.scoreBoards [i].transform);
					scoreBoardObjects [i].transform.ResetTransformation ();
				}
			}
		}
		   
	}

	public void Disable (){
		   
			SendAction (-1, -1, PowerUpManager.ActionType.Disable);
			for (int i = 0; i < scoreBoardObjects.Length; i++) {
				if (scoreBoardObjects [i]) {
					Destroy (scoreBoardObjects [i]);
					scoreBoardObjects [i] = null;
				}
			}
		   
	}



	//-----------------------------------------------------------------------------------------------Networking

	GameObject[] network_scoreboard_or = new GameObject[4];
	GameObject[] network_scoreboard_ed = new GameObject[4];
	GameObject cantSelectObj;
	public void ReceiveAction (int player, int x, int y, PowerUpManager.ActionType action) {
		   
			switch (action) {
			case PowerUpManager.ActionType.Enable:
				PowerUpManager.s.canActivatePowerUp = false;
				for (int i = 0; i < ScoreBoardManager.s.scoreBoards.Length; i++) {
					if (i != player) {
						if (network_scoreboard_ed [i]) {
							Destroy (network_scoreboard_ed [i]);
							network_scoreboard_ed [i] = null;
						}
						if (ScoreBoardManager.s.scoreBoards [i]) {
							network_scoreboard_ed [i] = (GameObject)Instantiate (affectedPrefab, ScoreBoardManager.s.scoreBoards [i].transform);
							network_scoreboard_ed [i].transform.ResetTransformation ();
						}
					}
				}
				network_scoreboard_or [player] = (GameObject)Instantiate (affectorPrefab, ScoreBoardManager.s.scoreBoards [player].transform);
				network_scoreboard_or [player].transform.ResetTransformation ();

				if (cantSelectObj != null) {
					Destroy (cantSelectObj);
					cantSelectObj = null;
				}
				cantSelectObj = (GameObject)Instantiate (cantSelectPrefab, cantSelectPrefab.transform.position, cantSelectPrefab.transform.rotation);

				break;
			case PowerUpManager.ActionType.Disable:
				PowerUpManager.s.canActivatePowerUp = true;
				for (int i = 0; i < network_scoreboard_ed.Length; i++) {
					if (network_scoreboard_ed [i] != null) {
						Destroy (network_scoreboard_ed [i]);
						network_scoreboard_ed [i] = null;
					}
				}
				for (int i = 0; i < network_scoreboard_or.Length; i++) {
					if (network_scoreboard_or [i] != null) {
						DataLogger.LogMessage("Destroying a light element " + i.ToString());
						Destroy (network_scoreboard_or [i]);
						network_scoreboard_or [i] = null;
					}
				}

				if (cantSelectObj != null) {
					Destroy (cantSelectObj);
					cantSelectObj = null;
				}
				break;
			default:
				DataLogger.LogError ("Unrecognized power up action PUL");
				break;
			}
		   
	}

	void SendAction (int x, int y, PowerUpManager.ActionType action) { 
		//DataHandler.s.SendPowerUpAction (x, y, PowerUpManager.PowerUpType.Light, action);  
	}
}
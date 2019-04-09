using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PowerUp_Ice : MonoBehaviour {


	public GameObject sourceScoreBoard;
	public GameObject othersScoreBoard;

	//-----------------------------------------------------------------------------------------------Main Functions

	GameObject[] scoreBoardObjects = new GameObject[4];
	public void Enable () {
		   
		SendAction (-1, -1, PowerUpManager.ActionType.Enable);
		Invoke ("Disable", GS.a.powerUpSettings.ice_activeTime);


		//we are the player who activated the effect so we dont freeze but others do

		//spawn freeze effect for the frozen players
		for(int i = 0; i < ScoreBoardManager.s.scoreBoards.Length; i++) {
			if (i != DataHandler.s.myPlayerInteger) {
				if (scoreBoardObjects [i] != null) {
					Destroy (scoreBoardObjects [i]);
					scoreBoardObjects [i] = null;
				}
				if (ScoreBoardManager.s.scoreBoards [i]) {
					scoreBoardObjects [i] = (GameObject)Instantiate (othersScoreBoard, ScoreBoardManager.s.scoreBoards [i].transform);
					scoreBoardObjects [i].transform.ResetTransformation ();
				}
			}
		}

		//if there is a npc spawn for it too
		/*if(GS.a.npcSettings.isNPC){
			scoreBoardObjects [DataHandler.NPCInteger] = (GameObject)Instantiate (othersScoreBoard, ScoreBoardManager.s.scoreBoards [DataHandler.NPCInteger].transform);
			scoreBoardObjects [DataHandler.NPCInteger].transform.ResetTransformation ();
		}

		//and disable the npc
		if (GS.a.npcSettings.isNPC) {
			_NPCBehaviour.isFrozen = true;
		}*/
		   
	}

	public void Disable (){
		   
		SendAction (-1, -1, PowerUpManager.ActionType.Disable);
		for (int i = 0; i < ScoreBoardManager.s.scoreBoards.Length; i++) {
			if (scoreBoardObjects [i] != null) {
				Destroy (scoreBoardObjects [i]);
				scoreBoardObjects [i] = null;
			}
		}

		/*if(GS.a.npcSettings.isNPC){
			if (scoreBoardObjects [DataHandler.NPCInteger] != null) {
				Destroy (scoreBoardObjects [DataHandler.NPCInteger]);
				scoreBoardObjects [DataHandler.NPCInteger] = null;
			}
		}

		if (GS.a.npcSettings.isNPC) {
			_NPCBehaviour.isFrozen = false;
		}*/
		   
	}



	//-----------------------------------------------------------------------------------------------Networking

	GameObject[] network_scoreboard = new GameObject[4];
	public void ReceiveAction (int player, int x, int y, PowerUpManager.ActionType action) {
		   
		switch (action) {
		case PowerUpManager.ActionType.Enable:
			network_scoreboard [player] = (GameObject)Instantiate (sourceScoreBoard, ScoreBoardManager.s.scoreBoards [player].transform);
			network_scoreboard [player].transform.ResetTransformation ();
			CoolIceEnabler.s.isEnabled = true;
			LocalPlayerController.s.canSelect = false;
			break;
		case PowerUpManager.ActionType.Disable:
			if (network_scoreboard [player])
				network_scoreboard [player].GetComponent<DisableAndDestroy> ().Engage ();
			network_scoreboard [player] = null;
			CoolIceEnabler.s.isEnabled = false;
			LocalPlayerController.s.canSelect = true;
			break;
		default:
			DataLogger.LogError ("Unrecognized power up action PUI");
			break;
		}
		   
	}

	void SendAction (int x, int y, PowerUpManager.ActionType action) {
		//DataHandler.s.SendPowerUpAction (x, y, PowerUpManager.PowerUpType.Ice, action);
	}
}
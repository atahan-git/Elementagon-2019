using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoardManager : MonoBehaviour {

	public static ScoreBoardManager s;

	public int[,] allScores = new int[6, 15]; //0-3 player scores, 4-5 team scores, 3 is enemy score in modes that have enemies

	public int npcid { get { return DataHandler.NPCInteger; } }

	public GameObject scoreboardPrefab;
	public Transform scoreboardParent;
	//[HideInInspector]
	public GameObject[] scoreBoards = new GameObject[6];
	public Transform[] scoreGetTargets = new Transform[6];

	public Transform indicatorParent;

	DataLogger logText;
	// Use this for initialization
	void Awake () {
		s = this;
	}

	void Start () {
		logText = DataLogger.s;
		try {
			scoreBoards = new GameObject[6];
			allScores = new int[6, 15];
			for (int i = 0; i < scoreBoards.Length; i++) {
				scoreBoards[i] = null;
			}
			SetUpPlayerScoreboardPanels ();

			DelayedScoreboard.s.UpdateScore (0, false);
		} catch (System.Exception e) {
			DataLogger.LogError ("Scoreboard creation failed error: ",e);
		}


		UpdatePanels ();
	}


	void SetUpPlayerScoreboardPanels () {

		//DataLogger.text = "Spawning panels";

		if (scoreboardParent == null) {
			print ("Scoreboard parent not found! cant draw conventional scoreboards");
			return;
		}


		/*scoreBoards [DataHandler.s.myPlayerinteger] = (GameObject)Instantiate (scoreboardPrefab, scoreboardParent);
		scoreBoards [DataHandler.s.myPlayerinteger].transform.ResetTransformation ();
		try {
			scoreBoards [DataHandler.s.myPlayerinteger].GetComponent<ScorePanel> ().SetValues (DataHandler.s.myPlayerinteger, GoogleAPI.s.GetSelf ().DisplayName, true);
		} catch {
			scoreBoards [DataHandler.s.myPlayerinteger].GetComponent<ScorePanel> ().SetValues (DataHandler.s.myPlayerinteger, "Player", true);
		}

		DataLogger.LogMessage ("Player scoreboard succesfuly created");*/


		int n = 0;
		foreach (GooglePlayGames.BasicApi.Multiplayer.Participant part in GoogleAPI.s.participants) {
			//if (i != DataHandler.s.myPlayerInteger) {
			DataLogger.LogMessage ("Creating " + n.ToString () + " scoreboard");
			scoreBoards[n] = (GameObject)Instantiate (scoreboardPrefab, scoreboardParent);
			scoreBoards[n].transform.ResetTransformation ();
			scoreBoards[n].GetComponent<ScorePanel> ().SetValues (n, part.DisplayName, false);
			DataLogger.LogMessage (n.ToString () + " scoreboard created");
			//}
			n++;
		}

		if (GS.a.myGameType == GameSettings.GameType.Two_Coop) {
			DataLogger.LogMessage ("Creating Players' Team scoreboard");
			scoreBoards[4] = (GameObject)Instantiate (scoreboardPrefab, scoreboardParent);
			scoreBoards[4].transform.ResetTransformation ();
			scoreBoards[4].GetComponent<ScorePanel> ().SetValues (4, "Allies", false);
			DataLogger.LogMessage ("Players' Team scoreboard created");

		} else if (GS.a.myGameType == GameSettings.GameType.TwoVTwo) {
			bool isTeamBlue = DataHandler.s.myPlayerInteger == 0 || DataHandler.s.myPlayerInteger == 1;
			DataLogger.LogMessage ("Creating Team Blue scoreboard");
			scoreBoards[4] = (GameObject)Instantiate (scoreboardPrefab, scoreboardParent);
			scoreBoards[4].transform.ResetTransformation ();
			scoreBoards[4].GetComponent<ScorePanel> ().SetValues (4, isTeamBlue ? "Allies" : "Enemies", false);
			DataLogger.LogMessage ("Team Blue scoreboard created");
			DataLogger.LogMessage ("Creating Team Red scoreboard");
			scoreBoards[5] = (GameObject)Instantiate (scoreboardPrefab, scoreboardParent);
			scoreBoards[5].transform.ResetTransformation ();
			scoreBoards[5].GetComponent<ScorePanel> ().SetValues (5, !isTeamBlue ? "Allies" : "Enemies", false);
			DataLogger.LogMessage ("Team Red scoreboard created");
		}

		switch (GS.a.myGameType) {
		case GameSettings.GameType.Singleplayer:
		case GameSettings.GameType.OneVOne:
		case GameSettings.GameType.Three_FreeForAll:
		case GameSettings.GameType.Four_FreeForAll:
			DelayedScoreboard.s.myPlayer = DataHandler.s.myPlayerInteger;
			break;
		case GameSettings.GameType.Two_Coop:
			DelayedScoreboard.s.myPlayer = 4;
			break;
		case GameSettings.GameType.TwoVTwo:
			if (DataHandler.s.myPlayerInteger == 0 || DataHandler.s.myPlayerInteger == 1) {
				DelayedScoreboard.s.myPlayer = 4;
			} else {
				DelayedScoreboard.s.myPlayer = 5;
			}
			break;
		}

		if (GS.a.isNPCEnabled) {
			scoreBoards[3] = (GameObject)Instantiate (GS.a.gfxs.npcScoreboard, scoreboardParent);
			scoreBoards[3].GetComponent<ScorePanel> ().SetValues (DataHandler.NPCInteger, GS.a.myNPC.name, false, GS.a.myNPC.myColor);
			DataLogger.LogMessage (GS.a.myNPC.name + " scoreboard created");
		}


		for (int i = 0; i < 6; i++) {
			if (scoreBoards[i] != null) {
				print ("Scoreboard " + i.ToString () + " = " + scoreBoards[i].transform.Find ("Score Get Target").ToString ());
				scoreGetTargets[i] = scoreBoards[i].transform.Find ("Score Get Target");
			}
		}
		scoreGetTargets[DataHandler.s.myPlayerInteger] = DelayedScoreboard.s.transform.Find ("Score Get Target");
	}

	void UpdatePanels () {
		for (int i = 0; i < 4; i++) {
			GameObject sBoard = scoreBoards[i];
			if (sBoard != null) {
				sBoard.gameObject.GetComponent<ScorePanel> ().UpdateScore (allScores[i, 0], false);
			}
		}
	}

	public void AddScoreToOthers (int playerInt, int scoreType, int toAdd, bool isDelayed) {
		for (int i = 0; i < allScores.GetLength(0); i++) {
			if (i != playerInt) {
				AddScore (i, scoreType, toAdd, isDelayed, false);
			}
		}
	}

	public void AddScore (int playerInt, int scoreType, int toAdd, bool isDelayed) {
		AddScore (playerInt, scoreType, toAdd, isDelayed, true);
	}

	public void AddScore (int playerInt, int scoreType, int toAdd, bool isDelayed, bool careGameTypes) {
		isDelayed = false;
		char player = DataHandler.s.toChar (playerInt);

		AddToScoreArray (playerInt, scoreType, toAdd, isDelayed);

		if (careGameTypes) {
			switch (GS.a.myGameType) {
			case GameSettings.GameType.Singleplayer:
			case GameSettings.GameType.OneVOne:
			case GameSettings.GameType.Three_FreeForAll:
			case GameSettings.GameType.Four_FreeForAll:
				break;
			case GameSettings.GameType.Two_Coop:
				AddToScoreArray (4, scoreType, toAdd, isDelayed);
				DataHandler.s.SendScore (DataHandler.s.toChar (4), scoreType, allScores[4, scoreType], isDelayed);
				DataHandler.s.SendScore (DataHandler.s.toChar (4), 0, allScores[4, 0], isDelayed);
				break;
			case GameSettings.GameType.TwoVTwo:
				if (playerInt == 0 || playerInt == 1) {
					AddToScoreArray (4, scoreType, toAdd, isDelayed);
					DataHandler.s.SendScore (DataHandler.s.toChar (4), scoreType, allScores[4, scoreType], isDelayed);
					DataHandler.s.SendScore (DataHandler.s.toChar (4), 0, allScores[4, 0], isDelayed);
				} else {
					AddToScoreArray (5, scoreType, toAdd, isDelayed);
					DataHandler.s.SendScore (DataHandler.s.toChar (5), scoreType, allScores[5, scoreType], isDelayed);
					DataHandler.s.SendScore (DataHandler.s.toChar (5), 0, allScores[5, 0], isDelayed);
				}
				break;
			}
		}

		if (playerInt == DataHandler.s.myPlayerInteger) {
			for (int i = 0; i < toAdd; i++) {
				CharacterStuffController.s.ScoreAdded (scoreType);
			}
		}

		DataHandler.s.SendScore (player, scoreType, allScores[playerInt, scoreType], isDelayed);
		DataHandler.s.SendScore (player, 0, allScores[playerInt, 0], isDelayed);

		GameObjectiveFinishChecker.s.CheckReach ();
	}

	void AddToScoreArray (int id, int scoreType, int toAdd, bool isDelayed) {
		allScores[id, scoreType] += toAdd;
		allScores[id, scoreType] = (int)Mathf.Clamp (allScores[id, scoreType], 0, Mathf.Infinity);

		if (scoreType != 0) {
			allScores[id, 0] += toAdd;
			allScores[id, 0] = (int)Mathf.Clamp (allScores[id, 0], 0, Mathf.Infinity);
		}


		UpdateSBoard (id, isDelayed);
	}

	void UpdateSBoard (int id, bool isDelayed) {

		if (id == DelayedScoreboard.s.myPlayer)
			DelayedScoreboard.s.UpdateScore (allScores[id, 0], isDelayed);

		GameObject sBoard = scoreBoards[id];
		if (sBoard != null) {
			try {
				sBoard.gameObject.GetComponent<ScorePanel> ().UpdateScore (allScores[id, 0], isDelayed);
			} catch (System.Exception e) {
				DataLogger.LogMessage (e.Message + " - " + e.StackTrace);
			}
		}
	}

	public void ReceiveScore (char player, int scoreType, int totalScore, bool isDelayed) {
		try {
			DataLogger.LogMessage ("score received");
			int playerInt = DataHandler.s.toInt (player);

			allScores[playerInt, scoreType] = totalScore;
			/*if (scoreType == 0) {
				myScores[playerInt, 0] = totalScore;
			}*/

			UpdateSBoard (playerInt, isDelayed);
			GameObjectiveFinishChecker.s.CheckReach ();
		} catch (System.Exception e) {
			DataLogger.LogMessage (e.Message + " - " + e.StackTrace);
		}
	}

	public int GetScore (int playerid) {
		return GetScore (playerid, 0);
	}

	public int GetScore (int playerid, int type) {
		return allScores[playerid, type];
	}
}

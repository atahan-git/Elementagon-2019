using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class GameObjectiveMaster : MonoBehaviour {

	public static GameObjectiveMaster s;

	public bool isFinished = false;
	public bool isGamePlaying = false;

	public Text timerText;
	public float timer = 0f;

	public Objective[] winObjectives;
	public Objective[] loseObjectives;

	public GameObject winObjParent;
	public GameObject loseObjParent;

	public GameObject objGUIPrefab;

	void Awake () {
		s = this;
		isFinished = false;
	}

	private void Start () {
		InitializeObjectives ();
	}

	void InitializeObjectives () {
		winObjectives = GS.a.winObjectives;
		loseObjectives = GS.a.loseObjectives;

		for(int i = 0; i < winObjectives.Length; i++){
			Objective myObjective = winObjectives[i];
			myObjective.myObjectiveChecker.Initialize (myObjective);
			Instantiate (myObjective.customGUIObject != null ? myObjective.customGUIObject : objGUIPrefab, winObjParent.transform)
				.GetComponent<ObjectiveGUI> ().SetUpObjectiveGUI (myObjective);
		}
		for (int i = 0; i < loseObjectives.Length; i++) {
			Objective myObjective = loseObjectives[i];
			myObjective.myObjectiveChecker.Initialize (myObjective);
			Instantiate (myObjective.customGUIObject != null ? myObjective.customGUIObject : objGUIPrefab, loseObjParent.transform)
				.GetComponent<ObjectiveGUI> ().SetUpObjectiveGUI (myObjective);
		}
	}

	public void StartGame () {
		isGamePlaying = true;
	}
	

	private void Update () {
		if (!isFinished && isGamePlaying) {
			UpdateObjectiveStates (winObjectives);
			UpdateObjectiveStates (loseObjectives);

			timer += Time.deltaTime;
			timerText.text = TimeToString (timer);

			bool isLost = false;
			//***if there are no objectives we can never lose
			foreach (Objective myObjective in loseObjectives) {
				if (myObjective != null)
					isLost = isLost || myObjective.isDone;
			}

			if (isLost) {
				EndGame (WinnerID (false));
				return;
			}

			bool isWon = true;
			//***if there are no objectives we can never win
			if (winObjectives.Length == 0)
				isWon = false;

			foreach (Objective myObjective in winObjectives) {
				if(myObjective != null)
					isWon = isWon && myObjective.isDone;
			}

			if (isWon) {
				EndGame (WinnerID(true));
				return;
			}
		}
	}

	void UpdateObjectiveStates (Objective[] objectives) {
		foreach (Objective myObjective in objectives) {
			float value = myObjective.myObjectiveChecker.GetValue ();
			float requiredValue = myObjective.requiredValue;

			switch (myObjective.type) {

			case Objective.conditionType.ifTrue:
				if (value == 1) {
					myObjective.myObjectiveChecker.ObjectiveDone ();
					myObjective.isDone = true;
				} else
					myObjective.isDone = false;
				break;

			case Objective.conditionType.ifFalse:
				if (value == 0) {
					myObjective.myObjectiveChecker.ObjectiveDone ();
					myObjective.isDone = true;
				} else
					myObjective.isDone = false;
				break;

			case Objective.conditionType.ifAll:
				if (value == requiredValue) {
					myObjective.myObjectiveChecker.ObjectiveDone ();
					myObjective.isDone = true;
				} else
					myObjective.isDone = false;
				break;

			case Objective.conditionType.ifAny:
				if (value > 0) {
					myObjective.myObjectiveChecker.ObjectiveDone ();
					myObjective.isDone = true;
				} else
					myObjective.isDone = false;
				break;

			case Objective.conditionType.ifLessThan:
				if (value < requiredValue) {
					myObjective.myObjectiveChecker.ObjectiveDone ();
					myObjective.isDone = true;
				} else
					myObjective.isDone = false;
				break;

			case Objective.conditionType.ifLessThanOrEqual:
				if (value <= requiredValue) {
					myObjective.myObjectiveChecker.ObjectiveDone ();
					myObjective.isDone = true;
				} else
					myObjective.isDone = false;
				break;

			case Objective.conditionType.ifMoreThan:
				if (value > requiredValue) {
					myObjective.myObjectiveChecker.ObjectiveDone ();
					myObjective.isDone = true;
				} else
					myObjective.isDone = false;
				break;

			case Objective.conditionType.ifMoreThanOrEqual:
				if (value >= requiredValue) {
					myObjective.myObjectiveChecker.ObjectiveDone ();
					myObjective.isDone = true;
				} else
					myObjective.isDone = false;
				break;

			case Objective.conditionType.ifEqual:
				if (value == requiredValue) {
					myObjective.myObjectiveChecker.ObjectiveDone ();
					myObjective.isDone = true;
				} else
					myObjective.isDone = false;
				break;
			}
		}
	}

	public static string TimeToString (float _timer) {
		string minutes = Mathf.Floor (_timer / 60).ToString ("00");
		string seconds = Mathf.Floor (_timer % 60).ToString ("00"); 

		return minutes + ":" + seconds;
	}

	IEnumerator DelayedEndGame (int i, float delay) {
		DataLogger.LogMessage ("Delayed ending: " + delay.ToString());
		isGamePlaying = false;
		LocalPlayerController.isActive = false;

		yield return new WaitForSeconds (delay);
		EndGame (i);
	}


	public int WinnerID (bool isWon) {
		switch (GS.a.myGamePlayerType) {
		case GameSettings.GamePlayerTypes.Singleplayer:
		case GameSettings.GamePlayerTypes.OneVOne:
		case GameSettings.GamePlayerTypes.Three_FreeForAll:
		case GameSettings.GamePlayerTypes.Four_FreeForAll:
			if (isWon) {
				return DataHandler.s.myPlayerInteger < 2 ? DataHandler.s.myPlayerInteger : 5;
			} else {
				return DataHandler.s.myPlayerInteger < 2 ? 5 : DataHandler.s.myPlayerInteger;
			}
		case GameSettings.GamePlayerTypes.Two_Coop:
		case GameSettings.GamePlayerTypes.TwoVTwo:
			if (isWon) {
				return DataHandler.s.myPlayerInteger < 2 ? 4 : 5;
			} else {
				return DataHandler.s.myPlayerInteger < 2 ? 5 : 4;
			}
		}

		return -1;
	}

	int finisherId = -1;
	public void EndGame (int id){
		if (isFinished)
			return;

		DataLogger.LogMessage ("Game Finished >" + DataHandler.s.myPlayerInteger.ToString() + " - " + id.ToString());
		isFinished = true;
		isGamePlaying = false;
		finisherId = id;
		LocalPlayerController.isActive = false;

		NPCManager.s.StopAllNPCs ();

		bool isWon = CheckisWon ();

		try {
			/*if (_NPCBehaviour.activeNPC != null)
				_NPCBehaviour.activeNPC.isActive = false;*/

			PowerUpManager.s.DisablePowerUps ();

			if (GS.a.nextStage == null) {
				if (GoogleAPI.s.gameInProgress) {
					GoogleAPI.s.LeaveGame ();
				}
			}
		} catch (System.Exception e) {
			DataLogger.LogError (this.name, e);
		}

		if (isWon) {
			try {
				SaveMaster.LevelFinished (true, GS.a);
			} catch (System.Exception e){
				DataLogger.LogError ("Cant save level finished data: ", e);
			}

			if (GS.a.winDrops != null) {
				for(int i = 0; i< GS.a.winDrops.Length; i++) {
					ItemBase item = GS.a.winDrops[i];
					int itemAmount = 1;
					if (GS.a.windDropAmounts.Length < i) {
						if (GS.a.windDropAmounts[i] > 0)
							itemAmount = GS.a.windDropAmounts[i];
					}

					if (item != null) {
						InventoryMaster.s.Add (item, itemAmount);
					}
				}
			}

			if (GS.a.levelOutroDialog != null) {
				GameEndScreen.s.Endgame (finisherId, isWon, false);
			} else {
				RestOffTheEndingStuff (isWon);
			}

			if (GS.a.autoTransferHealthAcrossLevels) {
				int oldHealth = PlayerPrefs.GetInt (GS.a.name + GameSettings.autoTransferHealthAcrossLevelsPlayerPrefString, -1);
				int curHealth = ScoreBoardManager.s.allScores[DataHandler.s.myPlayerInteger, 0];
				if (curHealth > oldHealth)
					PlayerPrefs.SetInt (GS.a.name + GameSettings.autoTransferHealthAcrossLevelsPlayerPrefString, curHealth);

				print ("Health saved as " + curHealth.ToString() + " to " + GS.a.name + GameSettings.autoTransferHealthAcrossLevelsPlayerPrefString);
			}
		} else {
			GameEndScreen.s.Endgame (finisherId, isWon, true);
			InventoryMaster.s.ReduceEquipmentChargeLeft ();
		}
	}

	public static bool CheckisWon () {
		bool isWon = false;
		int id = s.finisherId;

		if (id == DataHandler.s.myPlayerInteger)
			isWon = true;

		if ((DataHandler.s.myPlayerInteger == 0 || DataHandler.s.myPlayerInteger == 1) && id == 4)
			isWon = true;

		if ((DataHandler.s.myPlayerInteger == 2 || DataHandler.s.myPlayerInteger == 3) && id == 5)
			isWon = true;

		if (id == -1 && GS.a.myGameRuleType == GameSettings.GameRuleTypes.Farm)
			isWon = true;

		return isWon;
	}

	public void TriggerEndingDialog () {
		DialogDisplayer.s.SetDialogScreenState (true);
		DialogTree.s.LoadFromAsset (GS.a.levelOutroDialog);
		DialogTree.s.StartDialog ();
	}

	void RestOffTheEndingStuff (bool isWon) {
		if (GS.a.nextStage == null) {
			GameEndScreen.s.Endgame (finisherId, isWon, true);
		} else {
			GameEndScreen.s.GetToNextStage ();
		}
	}

	public void OutroDialogEnded () {
		if (isFinished) {
			DialogDisplayer.s.SetDialogScreenState (false);
			RestOffTheEndingStuff (CheckisWon());
		}
	}

	public void DisconnectedFromGame () {
		if (!isFinished) {
			GameEndScreen.s.EndGameDisconnect ();
		}
	}

}

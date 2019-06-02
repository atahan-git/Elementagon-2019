using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class GameObjectiveFinishChecker : MonoBehaviour {

	public static GameObjectiveFinishChecker s;

	public bool isFinished = false;
	public bool isGamePlaying = false;
	public bool isTimed = false;

	public Text timerText;
	public float timer = 0f;

	int _turncount = 0;
	public int turnCount {
		get { return _turncount; }
		set {
			_turncount = value;
			CheckReach ();
		}
	}

	public Text objectiveText;

	void Awake () {
		s = this;
		isFinished = false;
		//print ("Checker " +gameObject.name);
	}
	private void Start () {
		string minutes = Mathf.Floor (timer / 60).ToString ("00");
		string seconds = Mathf.Floor (timer % 60).ToString ("00");

		timerText.text = minutes + ":" + seconds;

		UpdateObjectiveText ();
	}

	public void StartGame () {
		isGamePlaying = true;
	}

	bool anyNpcSpawned = false;
	private void Update () {
		if (!isFinished && isGamePlaying) {
			timer += isTimed ? -Time.deltaTime : Time.deltaTime;

			if (isTimed && timer <= 0) {
				timer = 0;
				EndGame (5);
			}

			string minutes = Mathf.Floor (timer / 60).ToString ("00");
			string seconds = Mathf.Floor (timer % 60).ToString ("00");

			timerText.text = minutes + ":" + seconds;

			if (GS.a.killAllNPC) {
				if (NPCManager.s.ActiveNPCS.Count > 0)
					anyNpcSpawned = true;

				if (anyNpcSpawned && NPCManager.s.ActiveNPCS.Count <= 0) {
					switch (GS.a.myGameType) {
					case GameSettings.GameType.Singleplayer:
						StartCoroutine (DelayedEndGame (0, 1f));
						break;
					case GameSettings.GameType.Two_Coop:
						StartCoroutine (DelayedEndGame (4,1f));
						break;
					}
				}
			}
		}
	}

	IEnumerator DelayedEndGame (int i, float delay) {
		DataLogger.LogMessage ("Delayed ending: " + delay.ToString());
		isGamePlaying = false;
		LocalPlayerController.isActive = false;

		yield return new WaitForSeconds (delay);
		EndGame (i);
	}

	void UpdateObjectiveText () {
		string objectiveString = GS.a.objectiveText;

		if (objectiveString == "") {
			switch (GS.a.myGameObjectiveType) {
			default:
			case GameSettings.GameObjectiveTypes.Standard:
				objectiveString = "Reach " + GS.a.scoreReach.ToString () + " score";
				break;
			case GameSettings.GameObjectiveTypes.Haggle:
				objectiveString = "Haggle your way to " + GS.a.scoreReach.ToString () + "+" + ScoreBoardManager.s.allScores[3, 0].ToString () + " coins";
				if (GS.a.myGameType != GameSettings.GameType.Singleplayer)
					DataLogger.LogError ("Implement finish checker counter for single player!");
				break;
			case GameSettings.GameObjectiveTypes.Health:
				objectiveString = "Kill your enemy before you die";
				break;
			}

			if (GS.a.timer > 0) {
				objectiveString += ", before the timer runs out";
				isTimed = true;
				timer = GS.a.timer;
			}
			if (GS.a.turns > 0)
				objectiveString += ", under " + turnCount.ToString () + "/" + GS.a.turns.ToString () + " turns";
		}
		objectiveText.text = objectiveString;
	}

	public void CheckReach (){
		if (!isFinished && isGamePlaying) {

			//GameObject sBoard = ScoreBoardManager.s.scoreBoards [i];
			//if (sBoard != null) {


			//print ("Checking reach: " + i.ToString () + " -> " + myArray[0].ToString ());

			UpdateObjectiveText ();

			if (GS.a.scoreReach > 0) {
				try {
					switch (GS.a.myGameObjectiveType) {
					default:
					case GameSettings.GameObjectiveTypes.Standard:
						switch (GS.a.myGameType) {
						case GameSettings.GameType.Singleplayer:
						case GameSettings.GameType.OneVOne:
						case GameSettings.GameType.Three_FreeForAll:
						case GameSettings.GameType.Four_FreeForAll:
							for (int i = 0; i < ScoreBoardManager.s.allScores.GetLength (0); i++) {
								if (ScoreBoardManager.s.allScores[i, 0] >= GS.a.scoreReach) {
									EndGame (i);
									return;
								}
							}
							break;
						case GameSettings.GameType.Two_Coop:
						case GameSettings.GameType.TwoVTwo:
							if (ScoreBoardManager.s.allScores[4, 0] >= GS.a.scoreReach) {
								EndGame (4);
								return;
							}
							if (ScoreBoardManager.s.allScores[5, 0] >= GS.a.scoreReach) {
								EndGame (5);
								return;
							}
							break;
						}
						break;
					case GameSettings.GameObjectiveTypes.Haggle:
						switch (GS.a.myGameType) {
						case GameSettings.GameType.Singleplayer:
							if (ScoreBoardManager.s.allScores[0, 0] >= GS.a.scoreReach + ScoreBoardManager.s.allScores[3, 0]) {
								EndGame (0);
								return;
							}
							break;
						case GameSettings.GameType.OneVOne:
							if (ScoreBoardManager.s.allScores[0, 0] >= GS.a.scoreReach + ScoreBoardManager.s.allScores[1, 0]) {
								EndGame (0);
								return;
							}
							break;
						case GameSettings.GameType.Two_Coop:
						case GameSettings.GameType.TwoVTwo:
							if (ScoreBoardManager.s.allScores[4, 0] >= GS.a.scoreReach + ScoreBoardManager.s.allScores[5, 0]) {
								EndGame (4);
								return;
							}
							break;
						case GameSettings.GameType.Three_FreeForAll:
						case GameSettings.GameType.Four_FreeForAll:
							DataLogger.LogError ("This Game type is not supported! " + GS.a.myGameObjectiveType.ToString () + " - " + GS.a.myGameType.ToString ());
							break;
						}
						break;
					case GameSettings.GameObjectiveTypes.Health:
						switch (GS.a.myGameType) {
						case GameSettings.GameType.Singleplayer:
							if (ScoreBoardManager.s.allScores[0, 0] <= 0) {
								EndGame (3);
								return;
							}
							if (ScoreBoardManager.s.allScores[3, 0] <= 0) {
								EndGame (0);
								return;
							}
							break;
						case GameSettings.GameType.OneVOne:
							if (ScoreBoardManager.s.allScores[0, 0] <= 0) {
								EndGame (1);
								return;
							}
							if (ScoreBoardManager.s.allScores[1, 0] <= 0) {
								EndGame (0);
								return;
							}
							break;
						case GameSettings.GameType.Two_Coop:
						case GameSettings.GameType.TwoVTwo:
							if (ScoreBoardManager.s.allScores[4, 0] <= 0) {
								EndGame (5);
								return;
							}
							if (ScoreBoardManager.s.allScores[5, 0] <= 0) {
								EndGame (4);
								return;
							}
							break;
						case GameSettings.GameType.Three_FreeForAll:
						case GameSettings.GameType.Four_FreeForAll:
							DataLogger.LogError ("This Game type is not supported! " + GS.a.myGameObjectiveType.ToString () + " - " + GS.a.myGameType.ToString ());
							break;
						}
						break;

					}
				} catch (System.Exception e) {
					DataLogger.LogMessage ("Problem Checking Score: " + GS.a.myGameType + " - " + e.Message + " - " + e.StackTrace);
				}
			}


			if (GS.a.turns > 0) {
				if (turnCount >= GS.a.turns) {
					switch (GS.a.myGameType) {
					case GameSettings.GameType.Singleplayer:
						EndGame (5);
						return;
					default:
						Debug.LogError ("Turn Limit not implemented for game type: " + GS.a.myGameType.ToString());
						break;
					}
				}
			}
		}
	}

	int finisherId = 0;
	public void EndGame (int id){
		if (isFinished)
			return;

		DataLogger.LogMessage ("Game Finished >" + DataHandler.s.myPlayerInteger.ToString() + " - " + id.ToString());
		isFinished = true;
		isGamePlaying = false;
		finisherId = id;
		LocalPlayerController.isActive = false;

		NPCManager.s.StopAllNPCs ();
		
		bool isWon = false;

		if (id == DataHandler.s.myPlayerInteger)
			isWon = true;

		if((DataHandler.s.myPlayerInteger == 0 || DataHandler.s.myPlayerInteger == 1) && id == 4)
			isWon = true;

		if ((DataHandler.s.myPlayerInteger == 2 || DataHandler.s.myPlayerInteger == 3) && id == 5)
			isWon = true;

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

			if (GS.a.levelOutroDialog != null) {
				DialogDisplayer.s.SetDialogScreenState (true);
				DialogTree.s.LoadFromAsset (GS.a.levelOutroDialog);
				DialogTree.s.StartDialog ();
			} else {
				RestOffTheEndingStuff ();
			}
		} else {
			GameObjectiveFinishCheckerGfx.s.Endgame (finisherId);
		}
	}

	void RestOffTheEndingStuff () {
		if (GS.a.nextStage == null) {
			GameObjectiveFinishCheckerGfx.s.Endgame (finisherId);
		} else {
			GameObjectiveFinishCheckerGfx.s.GetToNextStage ();
		}
	}

	public void OutroDialogEnded () {
		if (isFinished) {
			DialogDisplayer.s.SetDialogScreenState (false);
			RestOffTheEndingStuff ();
		}
	}

	public void DisconnectedFromGame () {
		if (!isFinished) {
			GameObjectiveFinishCheckerGfx.s.EndGameDisconnect ();
		}
	}

}

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

	public Text objectiveText;

	public List<NPCBase> ActiveNPCS = new List<NPCBase>();

	void Awake () {
		s = this;
		ActiveNPCS.Clear (); //this freaks me out... what if we leave older npcs alive somehow?
		isFinished = false;

		//print ("Checker " +gameObject.name);
	}
	private void Start () {
		string minutes = Mathf.Floor (timer / 60).ToString ("00");
		string seconds = Mathf.Floor (timer % 60).ToString ("00");

		timerText.text = minutes + ":" + seconds;

		string objectiveString = GS.a.objectiveText;

		if (objectiveString == "") {
			objectiveString = "Reach " + GS.a.scoreReach.ToString () + " score";
			if (GS.a.timer > 0) {
				objectiveString += " before the timer runs out";
				isTimed = true;
				timer = GS.a.timer;
			}
			if (GS.a.turns > 0)
				objectiveString += " under " + GS.a.turns.ToString () + " turns";
		}

		objectiveText.text = objectiveString;
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
				EndGame (4);
			}

			string minutes = Mathf.Floor (timer / 60).ToString ("00");
			string seconds = Mathf.Floor (timer % 60).ToString ("00");

			timerText.text = minutes + ":" + seconds;

			if (GS.a.killAllNPC) {
				if (ActiveNPCS.Count > 0)
					anyNpcSpawned = true;

				if (anyNpcSpawned && ActiveNPCS.Count <= 0) {
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

	public void CheckReach (){
		if (!isFinished && isGamePlaying) {

			//GameObject sBoard = ScoreBoardManager.s.scoreBoards [i];
			//if (sBoard != null) {


			//print ("Checking reach: " + i.ToString () + " -> " + myArray[0].ToString ());

			if (GS.a.scoreReach <= 0)
				return;

			try {
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
						if (ScoreBoardManager.s.allScores[4, 0] >= GS.a.scoreReach) {
							EndGame (4);
							return;
						}
					
					break;
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
			} catch (System.Exception e) {
				DataLogger.LogMessage ("Problem Checking Score: " + GS.a.myGameType + " - " + e.Message + " - " + e.StackTrace);
			}
			//}

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

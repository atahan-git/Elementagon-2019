using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndScreen : MonoBehaviour {

	public static GameEndScreen s;

	public Text nameText;
	public Text statusText;
	public GameObject endGameScreen;

	public GameObject winEffects;
	public GameObject lostEffects;

	public GameObject nextLevelScreen;

	public GameObject nextLevelButton;


	public GameObject finalEndButtons;
	public GameObject beforeFinalEndingDialogStoryButtons;

	// Use this for initialization
	void Start () {
		s = this;
		endGameScreen.SetActive (false);
		nextLevelScreen.SetActive (false);
		winEffects.SetActive (false);
		lostEffects.SetActive (false);

		GameSettings nextLevel = GS.s.NextLevelInChain ();
		nextLevelButton.SetActive (nextLevel != null);

		if (nextLevel != null) {
			nextLevelButton.GetComponentInChildren<Text> ().text = "Next Level: " + nextLevel.levelName;
		}
	}

	public void NextLevel () {
		GS.s.LoadNextLevelInChain ();
	}

	public void MainMenu () {
		SceneMaster.s.LoadMenu ();
	}

	public void Endgame (int id, bool isWon, bool isFinalEnd) {
		string name = "";
		bool setCorrectly = false;

		try {
			name = GoogleAPI.s.participants[id].DisplayName;
			setCorrectly = true;
		} catch {
			setCorrectly = false;
		}
		if (!setCorrectly) {
			switch (id) {
			case 0:
				name = "Blue Player";
				break;
			case 1:
				name = "Red Player";
				break;
			case 2:
				name = "Green Player";
				break;
			case 3:
				if (!GS.a.isNPCEnabled) {
					name = "Yellow Player";
				} else {
					name = GS.a.myNPCPrefab.name;
				}
				break;
			case 4:
			case 5:
				if (isWon)
					name = "Allies";
				else
					name = "Enemies";
				break;
			default:
				name = "Game Ended";
				break;
			}
		}

		
		if (isWon) {
			statusText.text = GS.a.winText.Length > 0 ? GS.a.winText : "You Win!";
			winEffects.SetActive (true);
		} else {
			statusText.text = GS.a.loseText.Length > 0 ? GS.a.loseText : "You Lose";
			lostEffects.SetActive (true);
			nextLevelButton.SetActive (false);
		}
		nameText.text = "Winner: " + name;

		if (isFinalEnd) {
			finalEndButtons.SetActive (true);
			beforeFinalEndingDialogStoryButtons.SetActive (false);
		} else {
			finalEndButtons.SetActive (false);
			beforeFinalEndingDialogStoryButtons.SetActive (true);
		}

		endGameScreen.SetActive (true);
	}

	public void TriggerEndingDialog () {
		endGameScreen.SetActive (false);
		GameObjectiveMaster.s.TriggerEndingDialog ();
	}


	public void GetToNextStage () {
		nextLevelScreen.SetActive (true);
	}

	public void NextStageButton () {
		SceneMaster.s.LoadPlayingLevel (GS.s.GetGameModeId (GS.a.nextStage));
	}

	public void EndGameDisconnect () {
		endGameScreen.SetActive (true);
		statusText.text = "Disconnected";
		lostEffects.SetActive (true);

		nameText.text = "We are sorry for the connection issue :( /nUnfortunatelly reconnecting isn't possible";
	} 

	public void Endgame (string name, bool isWon){
		if (isWon) {
			statusText.text = "You Win!";
		} else {
			statusText.text = "You Lose";
		}
		nameText.text = "Winner: " + name;

		endGameScreen.SetActive (true);
	}
}

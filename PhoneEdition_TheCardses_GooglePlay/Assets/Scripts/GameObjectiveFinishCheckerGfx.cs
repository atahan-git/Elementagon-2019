using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameObjectiveFinishCheckerGfx : MonoBehaviour {

	public static GameObjectiveFinishCheckerGfx s;

	public Text nameText;
	public Text statusText;
	public GameObject endGameScreen;

	public GameObject winEffects;
	public GameObject lostEffects;

	public GameObject nextLevelScreen;

	// Use this for initialization
	void Start () {
		s = this;
		endGameScreen.SetActive (false);
		nextLevelScreen.SetActive (false);
		winEffects.SetActive (false);
		lostEffects.SetActive (false);
	}
	

	public void MainMenu () {
		SceneMaster.s.LoadMenu ();
	}

	public void Endgame (int id, bool isWon) {
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
					name = GS.a.myNPC.name;
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
			statusText.text = "You Win!";
			winEffects.SetActive (true);
		} else {
			statusText.text = "You Lose";
			lostEffects.SetActive (true);
		}
		nameText.text = "Winner: " + name;

		endGameScreen.SetActive (true);
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

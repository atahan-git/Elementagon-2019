using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VC_LevelViewer : ViewController {
	
	public Text levelName;
	public Text levelDesc;
	public Transform playerCountParent;

	public GameObject allyPrefab;
	public GameObject enemyPrefab;

	GameSettings selected;

	Button[] myButs;
	// Use this for initialization
	void Awake () {
		myButs = GetComponentsInChildren<Button> (true);
		myButs[0].onClick.AddListener (Invite);
		myButs[1].onClick.AddListener (Quick);
		myButs[2].onClick.AddListener (SinglePlay);
		myButs[3].onClick.AddListener (Cancel);
	}

	public void Invite () {
		GoogleAPI.s.GetInvitationMatch (GS.s.GetGameModeId (selected));
	}

	public void Quick () {
		GoogleAPI.s.GetQuickMatch (GS.s.GetGameModeId (selected));
	}

	public void SinglePlay () {
		if (!waitForOnline) {
			SceneMaster.s.LoadPlayingLevel (GS.s.GetGameModeId (selected));
		} else {
			GoogleAPI.s.Login ();
		}
	}

	public void Cancel () {
		if (GoogleAPI.s.searchingForGame)
			GoogleAPI.s.CancelMatchSearch ();
		MenuMasterController.s.CloseOverlay ();
	}

	public void SelectLevel (GameSettings settings) {
		if (settings == null) {
			DataLogger.LogError ("Trying to select level with null settings");
			return;
		}

		selected = settings;

		levelName.text = settings.levelName;
		levelDesc.text = settings.levelDescription;

		if (selected.myGamePlayerType != GameSettings.GamePlayerTypes.Singleplayer) {
			if (GoogleAPI.s.isOnline) {
				SetButtonStates ();
				waitForOnline = false;
			} else {
				SetButtonStates ();
				waitForOnline = true;
				myButs[0].gameObject.SetActive (false);
				myButs[1].gameObject.SetActive (false);
				myButs[2].gameObject.SetActive (true);
				myButs[2].GetComponentInChildren<Text> ().text = "Login to play Online";
				GoogleAPI.s.Login ();
			}
		} else {
			SetButtonStates ();
			waitForOnline = false;
		}

		gameObject.SetActive (true);
	}

	bool waitForOnline = false;
	private void Update () {
		if (waitForOnline) {
			if (GoogleAPI.s.isOnline) {
				SetButtonStates ();
				waitForOnline = false;
			}
		}
	}

	public void SetButtonStates () {
		int childCount = playerCountParent.childCount;
		for (int i = childCount - 1; i >= 0; i--) {
			Destroy (playerCountParent.GetChild (i).gameObject);
		}

		myButs[0].gameObject.SetActive (true);
		myButs[1].gameObject.SetActive (true);
		myButs[2].gameObject.SetActive (false);
		myButs[2].GetComponentInChildren<Text> ().text = "Play";
		switch (selected.myGamePlayerType) {
		case GameSettings.GamePlayerTypes.Singleplayer:
			Instantiate (allyPrefab, playerCountParent);
			myButs[0].gameObject.SetActive (false);
			myButs[1].gameObject.SetActive (false);
			myButs[2].gameObject.SetActive (true);
			break;
		case GameSettings.GamePlayerTypes.Two_Coop:
			Instantiate (allyPrefab, playerCountParent);
			Instantiate (allyPrefab, playerCountParent);
			GoogleAPI.playerCount = 2;
			break;
		case GameSettings.GamePlayerTypes.OneVOne:
			Instantiate (allyPrefab, playerCountParent);
			Instantiate (enemyPrefab, playerCountParent);
			GoogleAPI.playerCount = 2;
			break;
		case GameSettings.GamePlayerTypes.TwoVTwo:
			Instantiate (allyPrefab, playerCountParent);
			Instantiate (allyPrefab, playerCountParent);
			Instantiate (enemyPrefab, playerCountParent);
			Instantiate (enemyPrefab, playerCountParent);
			GoogleAPI.playerCount = 4;
			break;
		case GameSettings.GamePlayerTypes.Three_FreeForAll:
			Instantiate (allyPrefab, playerCountParent);
			Instantiate (enemyPrefab, playerCountParent);
			Instantiate (enemyPrefab, playerCountParent);
			GoogleAPI.playerCount = 3;
			break;
		case GameSettings.GamePlayerTypes.Four_FreeForAll:
			Instantiate (allyPrefab, playerCountParent);
			Instantiate (enemyPrefab, playerCountParent);
			Instantiate (enemyPrefab, playerCountParent);
			Instantiate (enemyPrefab, playerCountParent);
			GoogleAPI.playerCount = 4;
			break;
		}
	}
}

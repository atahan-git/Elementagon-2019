using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStarter : MonoBehaviour {

	public static GameStarter s;

	public delegate void myDelegate();
	public myDelegate LevelBeginCall;

	public GameObject beginScreen;
	public GameObject startFadeIn;
	public GameObject waitForOthersScreen;

	public Text beginText;
	public Image beginImg;

	public AnimatedSpriteController myBg;

	//public float fadeStayTime = 0.4f;
	public float beginStayTime = 3f;

	public bool isOpeningFinished = false;

	static bool[] readyPlayersInOtherStage = new bool[0];
	public bool[] readyPlayers = new bool[0];
	bool isWaitingForPlayers = false;

	void Awake (){
		s = this;
		try {
			startFadeIn.SetActive (true);
			beginScreen.SetActive (false);
			waitForOthersScreen.SetActive (false);
			LocalPlayerController.isActive = false;
			/*foreach (MonoBehaviour mono in toCall) {
				if (mono != null) {
					if (!(mono is LocalPlayerController))
						mono.enabled = false;
				}
			}*/
		} catch (System.Exception e) {
			DataLogger.LogError (this.name, e);
		}

		if (GS.a == null)
			FindObjectOfType<GS> ().Awake ();

		if (GS.a.bgAnimation != null)
			myBg.SetAnimation (GS.a.bgAnimation);
		else if (GS.a.bgSprite != null)
			myBg.SetSprite (GS.a.bgSprite);

		GS.a.SetUpCards ();

		Invoke ("LateBegin", 0.1f);

		switch (GS.a.myGameType) {
		case GameSettings.GameType.OneVOne:
		case GameSettings.GameType.Two_Coop:
			readyPlayers = new bool[2];
			break;
		case GameSettings.GameType.TwoVTwo:
		case GameSettings.GameType.Four_FreeForAll:
			readyPlayers = new bool[4];
			break;
		case GameSettings.GameType.Three_FreeForAll:
			readyPlayers = new bool[3];
			break;
		}


		if (readyPlayersInOtherStage.Length >= 0) {
			readyPlayersInOtherStage.CopyTo (readyPlayers, 0);
		}

		if (GS.a.nextStage != null) {
			readyPlayersInOtherStage = new bool[readyPlayers.Length];
		}

		if (GS.a.customObject != null)
			Instantiate (GS.a.customObject);

		DataLogger.LogMessage ("GameStarter setup complete");
	}

	void LateBegin () {
		/*for (int i = 0; i < GS.a.startingHand.Length; i++) {
			if(GS.a.startingHand[i] != 0)
				ScoreBoardManager.s.AddScore (DataHandler.s.myPlayerInteger, i, GS.a.startingHand[i], false);
		}*/
	}




	/*
	 * 
	 *	Actual Start vvv 
	 * 
	 */

	public void Start () {
		BeginDialog ();
		//Invoke ("CloseBeginScreen", beginStayTime);
		//Invoke ("ActivateTheOthers", beginStayTime);
	}

	void BeginDialog (){
		if (GS.a.levelIntroDialog != null) {
			DialogTree.s.LoadFromAsset (GS.a.levelIntroDialog);
			DialogTree.s.StartDialog ();
		} else {
			print ("No opening dialog detected -> " + GS.a.PresetName);
			IntroDialogEnd ();
		}
	}

	public void IntroDialogEnd () {
		if (isOpeningFinished)
			return;


		if (GoogleAPI.s.gameInProgress) {
			DataHandler.s.SendDialogEnd ();
			readyPlayers[DataHandler.s.myPlayerInteger] = true;
			waitForOthersScreen.SetActive (true);
			isWaitingForPlayers = true;
		} else {
			BeginScreenBegin ();
		}
	}

	public void ReceivePlayerReady (int playerInt) {
		if (readyPlayers[playerInt] == false)
			readyPlayers[playerInt] = true;
		else
			readyPlayersInOtherStage[playerInt] = true;
	}

	private void Update () {
		if (isWaitingForPlayers) {
			bool isAllDone = true;
			for (int i = 0; i < readyPlayers.Length; i++) {
				if (!readyPlayers[i])
					isAllDone = false;
			}

			if (isAllDone) {
				isWaitingForPlayers = false;
				waitForOthersScreen.SetActive (false);
				BeginScreenBegin ();
			}
		}
	}

	public void BeginScreenBegin () {
		beginScreen.SetActive (true);
		beginText.text = GS.a.startingText;
		beginImg.sprite = GS.a.startingImage;
		Invoke ("BeginScreenEnd", beginStayTime);
	}

	public void BeginScreenEnd () {
		beginScreen.SetActive (false);
		isOpeningFinished = true;
		ActivateTheOthers ();
	}

	void ActivateTheOthers (){
		try {
			GameObjectiveFinishChecker.s.StartGame ();

			if (LevelBeginCall != null)
				LevelBeginCall.Invoke ();

			LocalPlayerController.isActive = true;
			/*foreach (MonoBehaviour mono in toCall) {
				if (mono != null) {
					if (!(mono is LocalPlayerController))
						mono.enabled = true;
				}
			}*/
			if (GS.a.isNPCEnabled) {
				NPCManager.s.SpawnNPCPeriodic ();
			}

		} catch (System.Exception e) {
			DataLogger.LogError (this.name, e);
		}
	}
}

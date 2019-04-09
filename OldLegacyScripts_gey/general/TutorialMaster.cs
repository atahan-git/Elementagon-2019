using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMaster : MonoBehaviour {

	public GridSettings grid4x2;
	public GridSettings grid8x4;

	public CardSettings noDragon;

	public GameSettings defSettings;

	TutorialText tut;

	void Start () {
		GS.a = defSettings;
		CardHandler.s.enabled = false;
		CardHandler.s.DeleteCards ();

		tut = TutorialText.s;

		StartCoroutine (Stage1 ());
	}

	int curScore = 0;
	void Update () {
		curScore = ScoreBoardManager.s.GetScore (0);
		if (firstSelect == false) {
			if (LocalPlayerController.s.GiveMem ()[0] != null) {
				firstSelect = true;
			}
		}
		if (secondSelect == false) {
			if (LocalPlayerController.s.GiveMem ()[1] != null) {
				secondSelect = true;
			}
		}

		if (curScore > 0)
			firstMatch = true;
		if (curScore == 4)
			got4match = true;

		if (ScoreBoardManager.s.GetScore (0, 8) > 0 && !isStarted[0]) {
			StartCoroutine (EarthDragon ());
			isStarted[0] = true;
		}

		if (ScoreBoardManager.s.GetScore (0, 10) > 0 && !isStarted[1]) {
			StartCoroutine (IceDragon ());
			isStarted[1] = true;
		}

		if (ScoreBoardManager.s.GetScore (0, 11) > 0 && !isStarted[2]) {
			StartCoroutine (LightDragon ());
			isStarted[2] = true;
		}

		if (ScoreBoardManager.s.GetScore (0, 12) > 0 && !isStarted[3]) {
			StartCoroutine (NetherDragon ());
			isStarted[3] = true;
		}

		if (ScoreBoardManager.s.GetScore (0, 13) > 0 && !isStarted[4]) {
			StartCoroutine (PoisonDragon ());
			isStarted[4] = true;
		}

		if (ScoreBoardManager.s.GetScore (0, 14) > 0 && !isStarted[5]) {
			StartCoroutine (ShadowDragon ());
			isStarted[5] = true;
		}

		if (isUsed[0] == true &&
		   isUsed[1] == true &&
		   isUsed[2] == true &&
		   isUsed[3] == true &&
		   isUsed[4] == true) {
			isUsed[0] = false;
			AddDragons (12);
		}
	}

	bool[] isUsed = new bool[5];
	bool[] isStarted = new bool[6];

	public bool firstSelect = false;
	public bool secondSelect = false;
	public bool firstMatch = false;
	public bool got4match = false;
	IEnumerator Stage1 () {
		firstSelect = false;
		secondSelect = false;
		firstMatch = false;
		got4match = false;

		yield return new WaitForSeconds (2f);
		tut.SetText ("Welcome to the tutorial!");

		yield return new WaitForSeconds (2f);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		tut.SetText ("When you see >> you can tap to continue");

		yield return new WaitForSeconds (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		tut.SetText ("You will now learn how to play the game.");

		yield return new WaitForSeconds (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		tut.SetText ("This game is all about matching cards.");

		yield return new WaitForSeconds (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		CardHandler.s.SetUpGrid ();
		LocalPlayerController.isActive = false;

		tut.SetText ("These are the cards you have to match.");

		yield return new WaitForSeconds (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		tut.SetText ("Tap them to reveal their contents.");
		LocalPlayerController.isActive = true;

		ShuffleWithMatches (1);

		yield return new WaitUntil (() => firstSelect);
		LocalPlayerController.s.GiveMem ()[0].cardType = 2;


		tut.SetText ("When you select a card it will glow with your color.");
		PausePlayer (true);

		yield return new WaitForSeconds (1f);

		yield return new WaitForSeconds (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		tut.SetText ("You can select another card to try to match it");
		PausePlayer (false);


		yield return new WaitUntil (() => secondSelect);
		LocalPlayerController.isActive = false;
		PausePlayer (true);
		yield return new WaitForSecondsRealtime (0.2f);
		Time.timeScale = 0;


		tut.SetText ("You can see that they don't match");
		LocalPlayerController.isActive = false;


		yield return new WaitForSecondsRealtime (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSecondsRealtime (0.2f);

		tut.SetText ("After opening two cards, they will get deselected after a short notice");

		yield return new WaitForSecondsRealtime (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		PausePlayer (false);
		LocalPlayerController.isActive = false;
		Time.timeScale = 1;

		yield return new WaitForSeconds (1f);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);


		tut.SetText ("Go ahead and try to match two cards now");
		LocalPlayerController.s.DeselectAll ();
		ShuffleWithMatches (2);
		LocalPlayerController.isActive = true;

		yield return new WaitUntil (() => firstMatch);
		LocalPlayerController.isActive = false;
		yield return new WaitForSeconds (1f);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		tut.SetText ("Cards disappear after you match them.");
		PausePlayer (true);

		yield return new WaitForSeconds (2f);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		tut.SetText ("The disappeared cards come back after some time, with different colors");

		yield return new WaitForSeconds (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		tut.SetText ("Match 3 more cards to continue.");
		PausePlayer (false);

		yield return new WaitUntil (() => got4match);

		tut.SetText ("Well Done!");

		yield return new WaitForSeconds (4f);

		StartCoroutine (Stage2 ());
	}


	IEnumerator Stage2 () {
		GS.a.gridSettings = grid8x4;
		GS.a.cardSettings = noDragon;

		CardHandler.s.SetUpGrid ();
		CardHandler.s.InitializeFirstStartingCards ();

		tut.SetText ("There are 7 different elements in total");
		PausePlayer (true);

		yield return new WaitForSeconds (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		tut.SetText ("Fire, Earth, Ice, Shadow, Nether, Poison and Light");

		yield return new WaitForSeconds (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		tut.SetText ("Get a total score of 7 to continue");
		yield return new WaitForSeconds (1f);
		PausePlayer (false);

		yield return new WaitUntil (() => curScore >= 7);
		PausePlayer (true);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		tut.SetText ("Congratulations!");

		yield return new WaitForSeconds (1f);
		yield return new WaitForSeconds (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		tut.SetText ("There are also dragon cards");

		yield return new WaitForSeconds (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		StartCoroutine (FireDragon ());
	}

	IEnumerator FireDragon () {
		LocalPlayerController.s.DeselectAll ();
		CardHandler.s.SetUpGrid ();
		CardHandler.s.InitializeFirstStartingCards ();

		IndividualCard myDragon = AddRandomPairOfType (9);
		LocalPlayerController.s.SelectCard (myDragon);

		yield return new WaitForSeconds (0.5f);

		tut.SetText ("Dragon cards can be matched with their same kind just like other cards");

		yield return new WaitForSeconds (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		tut.SetText ("Try to find its pair now");
		PausePlayer (false);

		while (ScoreBoardManager.s.GetScore (0, 9) <= 0) {
			yield return 0;
		}
		LocalPlayerController.isActive = false;
		tut.SetText ("");
		yield return new WaitForSeconds (1f);

		tut.SetText ("You can see on the left that you got a dragon");
		PausePlayer (false);
		LocalPlayerController.isActive = false;

		yield return new WaitForSeconds (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		tut.SetText ("Click on it to activate it");

		yield return new WaitUntil (() => LocalPlayerController.s.isPowerUp);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		tut.SetText ("Some dragon powers like the Fire Dragon's requires you to select a card to use it.");
		PausePlayer (false);

		yield return new WaitUntil (() => !LocalPlayerController.s.isPowerUp);
		yield return new WaitForSeconds (2f);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		tut.SetText ("Try to find other dragon cards now");

		AddDragons (8);
		AddDragons (14);
		AddDragons (10);
	}

	IEnumerator EarthDragon () {

		while (ScoreBoardManager.s.GetScore (0, 8) != 0) {
			yield return 0;
		}

		tut.SetText ("For each card you select, the Earth Dragon selects another card randomly for you");

		yield return new WaitUntil (() => !LocalPlayerController.s.isPowerUp);
		PowerUpManager.s.canActivatePowerUp = false;
		PausePlayer (true);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		tut.SetText ("Earth Dragon's power only lasts for a certain amount of uses");

		yield return new WaitForSecondsRealtime (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSecondsRealtime (0.2f);

		tut.SetText ("then it runs out and gets deactivated");

		yield return new WaitForSecondsRealtime (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSecondsRealtime (0.2f);

		tut.SetText ("Keep on searching for new dragons!");

		PausePlayer (false);
		PowerUpManager.s.canActivatePowerUp = true;
		AddDragons (13);
		isUsed[0] = true;
	}

	IEnumerator ShadowDragon () {

		while (ScoreBoardManager.s.GetScore (0, 14) != 0) {
			yield return 0;
		}

		tut.SetText ("Shadow Dragon is a bit different than the Fire Dragon");
		PausePlayer (true);
		Time.timeScale = 0;

		yield return new WaitForSecondsRealtime (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSecondsRealtime (0.2f);

		tut.SetText ("After activating you can open as many cards as you can for a limited time");

		yield return new WaitForSecondsRealtime (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSecondsRealtime (0.2f);

		tut.SetText ("When the timer ends you will get a point for each match");

		yield return new WaitForSecondsRealtime (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSecondsRealtime (0.2f);

		tut.SetText ("Tap to unpause the game and select as many cards as you can before the timer runs out!");

		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		PausePlayer (false);
		Time.timeScale = 1;

		yield return new WaitForSecondsRealtime (0.2f);

		tut.SetText ("Quickly select cards now!");


		yield return new WaitUntil (() => !LocalPlayerController.s.isPowerUp);
		PowerUpManager.s.canActivatePowerUp = false;
		PausePlayer (true);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		tut.SetText ("Shadow Dragon is very strong when used properly");

		yield return new WaitForSecondsRealtime (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSecondsRealtime (0.2f);

		tut.SetText ("Now try to find the other dragons!");

		PausePlayer (false);
		PowerUpManager.s.canActivatePowerUp = true;
		isUsed[1] = true;
	}

	IEnumerator IceDragon () {
		while (ScoreBoardManager.s.GetScore (0, 10) != 0) {
			yield return 0;
		}

		PowerUpManager.s.canActivatePowerUp = false;
		//PowerUpManager.s.ReceiveEnemyPowerUpActions (0, -1, -1, PowerUpManager.PowerUpType.Ice, PowerUpManager.ActionType.Enable);

		tut.SetText ("Oops you just froze");

		yield return new WaitForSeconds (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		tut.SetText ("Normally Ice Dragon freezes all other players but you");

		yield return new WaitForSeconds (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		tut.SetText ("However, since you are the only player you just froze");

		yield return new WaitForSeconds (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		tut.SetText ("You can't open cards while frozen");

		yield return new WaitForSeconds (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		tut.SetText ("Fortunately you won't stay frozen for too long");

		yield return new WaitForSeconds (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		tut.SetText ("Wait until you unfreeze");

		yield return new WaitForSeconds (2f);

		//PowerUpManager.s.ReceiveEnemyPowerUpActions (0, -1, -1, PowerUpManager.PowerUpType.Ice, PowerUpManager.ActionType.Disable);

		yield return new WaitForSeconds (2f);

		tut.SetText ("Now go on and find new dragons!");

		PowerUpManager.s.canActivatePowerUp = true;
		AddDragons (11);
		isUsed[2] = true;
	}

	IEnumerator PoisonDragon () {
		while (ScoreBoardManager.s.GetScore (0, 13) != 0) {
			yield return 0;
		}

		tut.SetText ("Select a card to poison it");

		yield return new WaitUntil (() => !LocalPlayerController.s.isPowerUp);
		PowerUpManager.s.canActivatePowerUp = false;
		PausePlayer (true);
		tut.SetText ("");
		yield return new WaitForSeconds (0.5f);
		Time.timeScale = 0;

		tut.SetText ("The next person who opens this card will get poisoned");

		yield return new WaitForSecondsRealtime (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSecondsRealtime (0.2f);

		tut.SetText ("Be sure to remember its position before it closes!");

		yield return new WaitForSecondsRealtime (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSecondsRealtime (0.2f);

		Time.timeScale = 1;
		PausePlayer (false);
		PowerUpManager.s.canActivatePowerUp = false;
		LocalPlayerController.isActive = false;

		yield return new WaitForSeconds (1f);
		LocalPlayerController.isActive = true;

		tut.SetText ("Try to open it yourself and see what happens to your score");
		int lastScore = ScoreBoardManager.s.GetScore (0, 0);

		bool check = true;

		while (check) {
			if (ScoreBoardManager.s.GetScore (0, 0) < lastScore) {
				check = false;
				break;
			}
			if (LocalPlayerController.s.GiveMem ()[0] != null) {
				PausePlayer (true);
				tut.SetText ("One does not simply not listen to the tutorial");

				yield return new WaitForSeconds (1f);
				tut.TapToCont (true);
				yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
				tut.TapToCont (false);
				tut.SetText ("");
				yield return new WaitForSeconds (0.2f);

				tut.SetText ("Now go ahead and open the poisoned card.");

				yield return new WaitForSeconds (0.5f);
				PausePlayer (false);

				LocalPlayerController.s.DeselectAll ();
			}

			yield return 0;
		}

		LocalPlayerController.isActive = false;

		PausePlayer (true);
		Time.timeScale = 0;

		tut.SetText ("Look closely at your score");

		yield return new WaitForSecondsRealtime (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		yield return new WaitForSecondsRealtime (0.2f);

		PausePlayer (false);
		LocalPlayerController.isActive = false;
		Time.timeScale = 1;

		yield return new WaitForSeconds (2f);

		tut.SetText ("As you can see when you open it you lose 5 points - ouch!");
		PausePlayer (true);

		yield return new WaitForSeconds (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		tut.SetText ("Next time, try not open the poison card!");

		yield return new WaitForSeconds (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		tut.SetText ("Keep the dragon search going!");
		PowerUpManager.s.canActivatePowerUp = true;
		PausePlayer (false);

		isUsed[3] = true;
	}

	IEnumerator NetherDragon () {
		while (ScoreBoardManager.s.GetScore (0, 12) != 0) {
			yield return 0;
		}

		tut.SetText ("Nether Dragon has one of the most unique effects");
		PausePlayer (true);
		Time.timeScale = 0;

		yield return new WaitForSecondsRealtime (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSecondsRealtime (0.2f);

		tut.SetText ("After activation it will reset all unmatched cards");

		yield return new WaitForSecondsRealtime (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSecondsRealtime (0.2f);

		tut.SetText ("and give you one point for each pair of matched cards");

		yield return new WaitForSecondsRealtime (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSecondsRealtime (0.2f);

		tut.SetText ("It will also remove poison cards so it is useful");

		yield return new WaitForSecondsRealtime (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSecondsRealtime (0.2f);

		tut.SetText ("Now tap to see the show");

		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		PausePlayer (false);
		Time.timeScale = 1;

		yield return new WaitUntil (() => !LocalPlayerController.s.isPowerUp);
		PowerUpManager.s.canActivatePowerUp = false;
		LocalPlayerController.isActive = false;
		tut.SetText ("");
		yield return new WaitForSeconds (3f);

		PausePlayer (true);
		tut.SetText ("This was the end of the tutorial");

		yield return new WaitForSeconds (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		tut.SetText ("You can now play against the AI or other players through the menu");

		yield return new WaitForSeconds (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		tut.SetText ("If you forget or want more info about any of the dragon powers just check the reference menu");

		yield return new WaitForSeconds (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		tut.SetText ("Have fun playing!");
		yield return new WaitForSeconds (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		yield return new WaitForSeconds (0.2f);

		SceneMaster.s.LoadMenu ();
	}

	IEnumerator LightDragon () {
		while (ScoreBoardManager.s.GetScore (0, 11) != 0) {
			yield return 0;
		}

		tut.SetText ("Light Dragon is like Ice Dragon");
		PausePlayer (true);
		PowerUpManager.s.canActivatePowerUp = false;

		yield return new WaitForSeconds (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		tut.SetText ("But instead of keeping a player from opening cards,");

		yield return new WaitForSeconds (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		tut.SetText ("it makes them unable to activate dragon powers");

		yield return new WaitForSeconds (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		tut.SetText ("For a limited time of course");

		yield return new WaitForSeconds (0.3f);
		tut.TapToCont (true);
		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
		tut.TapToCont (false);
		tut.SetText ("");
		yield return new WaitForSeconds (0.2f);

		tut.SetText ("Continue looking for dragons!");
		PausePlayer (false);


		PowerUpManager.s.canActivatePowerUp = true;
		isUsed[4] = true;
	}

	void PausePlayer (bool isPause) {
		LocalPlayerController.isActive = !isPause;
		tut.Pause (isPause);
		/*if (isPause) {
			print ("player is unactive");
		} else {
			print ("player is active");
		}*/
	}

	void ShuffleWithMatches (int cardTypes) {

		CardHandler.s.SetUpGrid ();

		List<int> myCards = new List<int> ();

		int totalCards = GS.a.gridSettings.gridSizeX * GS.a.gridSettings.gridSizeY;

		for (int i = 0; i < totalCards; i += 2) {
			int rand = Random.Range (0, cardTypes);
			rand++;
			myCards.Add (rand);
			myCards.Add (rand);
		}

		int[] finalList = myCards.ToArray ();
		RandFuncs.Shuffle (finalList);

		int n = 0;
		for (int x = 0; x < GS.a.gridSettings.gridSizeX; x++) {
			for (int y = 0; y < GS.a.gridSettings.gridSizeY; y++) {
				CardHandler.s.allCards[x, y].cardType = finalList[n];
				n++;
			}
		}
	}

	IndividualCard AddRandomPairOfType (int cardType) {

		int x1 = Random.Range (2, CardHandler.s.allCards.GetLength (0) - 3);
		int y1 = Random.Range (1, CardHandler.s.allCards.GetLength (1) - 1);

		IndividualCard card = CardHandler.s.allCards[x1, y1];
		card.cardType = cardType;

		int x2 = Random.Range (2, CardHandler.s.allCards.GetLength (0) - 3);
		int y2 = Random.Range (1, CardHandler.s.allCards.GetLength (1) - 1);

		while (x1 == x2) {
			x2 = Random.Range (2, CardHandler.s.allCards.GetLength (0) - 3);
		}

		while (y1 == y2) {
			y2 = Random.Range (1, CardHandler.s.allCards.GetLength (1) - 1);
		}

		print (x2.ToString () + " - " + y2.ToString ());
		CardHandler.s.allCards[x2, y2].cardType = cardType;

		return card;
	}


	void AddDragons (int dragontype) {
		StartCoroutine (_AddDragons (dragontype));
	}

	IEnumerator _AddDragons (int dragontype) {
		int x1 = Random.Range (0, CardHandler.s.allCards.GetLength (0));
		int y1 = Random.Range (0, CardHandler.s.allCards.GetLength (1));

		while (CardHandler.s.allCards[x1, y1].cardType > 7 || !CardHandler.s.allCards[x1, y1].isSelectable) {
			x1 = Random.Range (0, CardHandler.s.allCards.GetLength (0));
			y1 = Random.Range (0, CardHandler.s.allCards.GetLength (1));
			yield return 0;
		}

		CardHandler.s.allCards[x1, y1].cardType = dragontype;


		int x2 = Random.Range (0, CardHandler.s.allCards.GetLength (0));
		int y2 = Random.Range (0, CardHandler.s.allCards.GetLength (1));


		while (CardHandler.s.allCards[x2, y2].cardType > 7 || x1 == x2 || y1 == y2 || !CardHandler.s.allCards[x2, y2].isSelectable) {
			x2 = Random.Range (0, CardHandler.s.allCards.GetLength (0));
			y2 = Random.Range (0, CardHandler.s.allCards.GetLength (1));
			yield return 0;
		}

		CardHandler.s.allCards[x2, y2].cardType = dragontype;
		yield return 0;
	}
}

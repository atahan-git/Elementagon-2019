using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VC_MultiplayerMenuController : ViewController {

	public static VC_MultiplayerMenuController s;

	public Button loginButton;
	public Text loginButtonText;

	public Image middleImage;
	public Text middleText;

	public Button backButton;

	public Button upButton;
	public Button downButton;

	public Button upButton2;
	public Text button2midText;
	public Button downButton2;

	public Text toolTip;

	public Button joinQuickButton;
	public Button joinInvitationButton;
	public Button joinAcceptInviteButton;
	public Button cancelButton;

	void OnSceneWasLoaded () {
		s = this;
	}

	void Start () {
		s = this;
		searchingGamePanel.SetActive (false);
	}

	void Update () {
		s = this;
	}


	public int playerCount = 2;
	public int gameMode = 0;

	public void Login () {
		GoogleAPI.s.Login ();
		UpdateMenu ();
	}

	public void JoinAcceptInvite () {
		AcceptInv ();
	}

	public void JoinInvitation () {
		GetMatch (1);
	}

	public void JoinQuick () {
		GetMatch (0);
	}

	public void GetMatch (int mode) {
		GoogleAPI.playerCount = playerCount;

		switch (mode) {
			case 0:
				GoogleAPI.s.GetQuickMatch (gameMode);
				break;
			case 1:
				GoogleAPI.s.GetInvitationMatch (gameMode);
				break;
			default:
				goto case 0;
		}

	}

	public void Cancel () {
		GoogleAPI.s.CancelMatchSearch ();
	}

	public void Up () {
		playerCount++;
		playerCount = (int)Mathf.Clamp (playerCount, 2, 4);
		UpdateMenu ();
	}

	public void Down () {
		playerCount--;
		playerCount = (int)Mathf.Clamp (playerCount, 2, 4);
		UpdateMenu ();
	}

	public void Up2 () {
		gameMode++;
		gameMode = (int)Mathf.Clamp (gameMode, 0, 1);
		UpdateMenu ();
	}

	public void Down2 () {
		gameMode--;
		gameMode = (int)Mathf.Clamp (gameMode, 0, 1);
		UpdateMenu ();
	}

	public void AcceptInv () {
		GoogleAPI.s.AcceptInvitation ();
	}


	/*public void CampaignSelect () {
		SceneMaster.s.LoadSinglePLayerLevel ();
	}*/


	//--------------------------------------------------------

	public void UpdateMenu () {

		toolTip.text = "Select Player Count";
		middleText.text = playerCount.ToString ();
		loginButtonText.text = "Login";
		backButton.interactable = true;

		switch (playerCount) {
			case 2:
				downButton.interactable = false;
				upButton.interactable = true;
				break;
			case 3:
				downButton.interactable = true;
				upButton.interactable = true;
				break;
			case 4:
				downButton.interactable = true;
				upButton.interactable = false;
				break;
		}

		switch (gameMode) {
			case 0:
				button2midText.text = "Tap";
				downButton2.interactable = false;
				upButton2.interactable = true;
				break;
			case 1:
				button2midText.text = "Slide";
				downButton2.interactable = true;
				upButton2.interactable = false;
				break;
		}

		if (GoogleAPI.s.isOnline) {
			loginButton.interactable = false;
			loginButtonText.text = "Logged in";
		} else {
			loginButton.interactable = true;
			loginButtonText.text = "Login";
		}

		if (GoogleAPI.s.canPlay) {
			joinQuickButton.interactable = true;
			joinInvitationButton.interactable = true;
			joinAcceptInviteButton.interactable = true;
			cancelButton.interactable = false;
		} else {
			joinQuickButton.interactable = false;
			joinInvitationButton.interactable = false;
			joinAcceptInviteButton.interactable = false;
			upButton.interactable = false;
			downButton.interactable = false;
			upButton2.interactable = false;
			downButton2.interactable = false;
			cancelButton.interactable = true;
		}
	}

	[Space]
	public GameObject searchingGamePanel;

	public static void SetSearchingPanelState (bool state) {
		if (s != null)
			s.searchingGamePanel.SetActive (state);
	}

	public void CancelSearching () {
		searchingGamePanel.SetActive (false);
		if (GoogleAPI.s.searchingForGame)
			GoogleAPI.s.CancelMatchSearch ();
	}
}

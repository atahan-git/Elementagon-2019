using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public GameObject mainMenu;
	public GameObject campaignMenu;
	public GameObject multiplayerMenu;

	[Header("Multiplayer join menu stuff")]

	public static MainMenu s;
	
	public Button leftButton;
	public Text leftButtonText;

	public Button rightButton;
	public Text rightButtonText;

	public Image middleImage;
	public Text middleText;

	public Button backButton;

	public Button upButton;
	public Button downButton;

	public Button upButton2;
	public Text button2midText;
	public Button downButton2;

	public Text toolTip;

	public Button LeftJoinButton;
	public Button RightJoinButton;
	public Button UpJoinButton;
	public Button cancelButton;

	public Button[] campaignButtons = new Button[7];

	void OnSceneWasLoaded (){
		s = this;
	}

	void Start (){
		s = this;
	}

	void Update (){
		s = this;
	}


	/*public void Left () {
		MenuManager.s.Left ();
	}

	public void LeftJoin () {
		MenuManager.s.AcceptInv ();
	}

	public void RightJoin () {
		MenuManager.s.GetMatch (1);
	}

	public void UpJoin () {
		MenuManager.s.GetMatch (0);
	}

	public void Cancel () {
		MenuManager.s.Cancel ();
	}

	public void Up () {
		MenuManager.s.Up ();
	}

	public void Down () {
		MenuManager.s.Down ();
	}

	public void Up2 () {
		MenuManager.s.Up2 ();
	}

	public void Down2 () {
		MenuManager.s.Down2 ();
	}

	public void Back () {
		MenuManager.s.MainMenuSelect ();
	}

	public void Singleplayer () {
		MenuManager.s.CampaignMenuSelect ();
	}

	public void Multiplayer () {
		MenuManager.s.MultiplayerMenuSelect ();
	}

	public void CampaignSelect (int id){
		MenuManager.s.CampaignSelect (id);
	}

	public void Debug (){
		DataLogger.ToggleDebugSettings ();
	}

	public void Tutorial (){
		MenuManager.s.Tutorial ();
	}

	public void ResetProgress (){
		SaveMaster.ResetProgress ();
	}

	public void HackLevels (){
		SaveMaster.UnlockAll ();
	}*/
}

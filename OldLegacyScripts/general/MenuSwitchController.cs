using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSwitchController : MonoBehaviour {

	public static MenuSwitchController s;

	public GameObject mainMenu;
	public GameObject multiplayerMenu;

	[Space]
	public UI_EnterAnimation[] animatedMenus;

	private void Awake () {
		s = this;
	}

	void OnSceneWasLoaded () {
		s = this;
	}

	void Start () {
		s = this;

		activeMenu = mainMenu;
		mainMenu.SetActive (true);
		multiplayerMenu.SetActive (false);

		foreach (UI_EnterAnimation menu in animatedMenus) {
			if (menu != null) {
				menu.gameObject.SetActive (false);
				menu.Setup ();
			}
		}

		int lastMenuState = PlayerPrefs.GetInt ("LastMenuState", 0);
		switch (lastMenuState) {
		case 0:
			MainMenuSelect ();
			break;
		case 1:
			MultiplayerMenuSelect ();
			break;
		default:
			lastMenuState = lastMenuState - 2;
			if (lastMenuState > 0 && lastMenuState < animatedMenus.Length) {
				OpenAnimatedMenu (lastMenuState);
			}
			break;
		}
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (activeMenu == mainMenu)
				Application.Quit ();
			else
				MainMenuSelect ();
		}
	}

	[HideInInspector]
	public GameObject activeMenu;

	public void MainMenuSelect () {
		activeMenu.SetActive (false);
		mainMenu.SetActive (true);
		activeMenu = mainMenu;
		PlayerPrefs.SetInt ("LastMenuState", 0);
	}

	public void MultiplayerMenuSelect () {
		activeMenu.SetActive (false);
		multiplayerMenu.SetActive (true);
		activeMenu = multiplayerMenu;
		if(MultiplayerMenuController.s != null)
		MultiplayerMenuController.s.UpdateMenu ();
		PlayerPrefs.SetInt ("LastMenuState", 1);
	}

	public void OpenAnimatedMenu (int id) {
		if (animatedMenus[id] != null)
			animatedMenus[id].OpenAnimation ();
		PlayerPrefs.SetInt ("LastMenuState", id+2);
	}

	public void CloseAnimatedMenu (int id) {
		if (animatedMenus[id] != null)
			animatedMenus[id].CloseAnimation ();
	}

	public void Back () {
		MainMenuSelect ();
	}
}

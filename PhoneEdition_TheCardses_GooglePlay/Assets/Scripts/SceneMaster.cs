using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMaster : MonoBehaviour {

	public static SceneMaster s;

	public bool levelLoadLock = false;

	public GameObject loadingScreen;

	public int curScene = 0;

	void Awake (){
		if (loadingScreen != null)
			loadingScreen.SetActive (false);

		if (s != null && s != this) {
			Destroy (this.gameObject);
			return;
		} else {
			s = this;
		}

		curScene = SceneManager.GetActiveScene ().buildIndex;
	}

	private void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (GetSceneID () != menuId)
				LoadMenu ();
			else
				Application.Quit ();
		}
	}

	public const int menuId = 0;
	public void LoadMenu (){
		SaveMaster.s.Save ();
		if(GoogleAPI.s.gameInProgress)
			GoogleAPI.s.LeaveGame ();
		LoadLevel (menuId);
	}

	/*public const int multiplayerId = 1;
	public void LoadMultiplayer (){
		GS.s.SetMultiplayer();
		LoadLevel (multiplayerId);
	}*/

	public const int playingLevelId = 1;
	public void LoadPlayingLevel (int settingsId){
		//GoogleAPI.gameMode = 0;
		//GS.s.ChangeGameMode(id+1);
		//LoadLevel(singleStartId + id);
		try {
			GS.s.ChangeGameMode (settingsId);
			LoadLevel (playingLevelId);
		} catch (System.Exception e) {
			DataLogger.LogError ("Cant load level with id: " + settingsId.ToString (), e);
		}
	}

	/*public void LoadTutorial (){
		GoogleAPI.gameMode = 0;
		LoadLevel (2);
	}*/

	void LoadLevel (int id) {

		if (!levelLoadLock) {
			try {
				loadingScreen.SetActive (true);
				DataLogger.LogMessage ("Loading level " + id.ToString ());
				//print ("Loading level " + id.ToString ());
				SceneManager.LoadSceneAsync (id);
				levelLoadLock = true;
			} catch {
				DataLogger.LogMessage ("Level " + id.ToString () + " doesn't exist");
			}
		}

	}

	public delegate void myLevelLoadDelegate (int id);
	public myLevelLoadDelegate _OnLevelWasLoaded;
	void OnLevelWasLoaded (){
		loadingScreen.SetActive (false);
		levelLoadLock = false;
		curScene = SceneManager.GetActiveScene ().buildIndex;

		if (_OnLevelWasLoaded != null)
			_OnLevelWasLoaded.Invoke (curScene);
	}

	public static int GetSceneID (){
		return SceneManager.GetActiveScene ().buildIndex;
	}
}

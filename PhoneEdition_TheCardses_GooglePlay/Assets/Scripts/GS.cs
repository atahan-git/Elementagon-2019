using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GS : MonoBehaviour {

	public static GS s;
	public static GameSettings a; //active

	public string activeGameMode;
	public int activeGameModeID;
	public GameSettings defaultMode;

	public bool isDebug = false;
	public GameSettings debugMode;

	[Space]

	public GameSettings[] allModes;

	public GameSettingsArray[] levelChains;

	[System.Serializable]
	public class GameSettingsArray {
		public GameSettings[] LevelChain;
	}
		

	// Use this for initialization
	public void Awake () {
		if (s != null && s != this) {
			Destroy (this.gameObject);
			return;
		} else {
			s = this;
		}

		for (int i = 0; i <allModes.Length; i++) {
			if(allModes[i] != null)
			allModes[i].id = i;
		}


		ChangeGameMode (defaultMode);

#if UNITY_EDITOR
		if (isDebug) {
			ChangeGameMode (debugMode);
			isDebug = false;
		}

		//print (NextLevelInChain ());
#endif

		GetComponent<SceneMaster> ()._OnLevelWasLoaded += LevelWasLoaded;
	}

	/*public void SetMultiplayer () {
		ChangeGameMode (multiplayerMode);
	}*/

	public void ChangeGameMode (GameSettings mode) {
		a = null;
		a = mode.Copy ();
		activeGameMode = a.name;
		activeGameModeID = a.id;
		DataLogger.LogMessage ("Active Game Settings: " + a.name);
	}

	public void ChangeGameMode (int id) {
		a = null;
		try {
			ChangeGameMode (allModes[id]);
		} catch {
			DataLogger.LogError ("Unknown mode id: " + id.ToString());
			a = defaultMode.Copy ();
		}
	}

	public int GetGameModeId (GameSettings mode) {
		try {
			if (mode != null)
				return mode.id;
			else
				return -1;
		} catch (System.Exception e) {
			if(mode != null)
				DataLogger.LogError ("Cant get id of mode: " + mode.ToString (), e);
			else
				DataLogger.LogError ("Mode you provided is null", e);
			return -1;
		}
	}

	public void LevelWasLoaded (int id) {
		if (id == SceneMaster.menuId)
			ChangeGameMode (defaultMode);
	}

	public void LoadNextLevelInChain () {
		GameSettings nextLevel = NextLevelInChain ();

		if (nextLevel != null) {
			SceneMaster.s.LoadPlayingLevel (nextLevel.id);
		} else {
			DataLogger.LogError ("Trying to load the next level in chain with empty chain! " + a.name);
		}
	}

	public GameSettings NextLevelInChain () {
		DataLogger.LogMessage ("Trying to find next level in chain");
		if (activeGameModeID != defaultMode.id) {
			int theIndex = -1;
			int n = 0;
			foreach (GameSettingsArray arr in levelChains) {
				for (int i = 0; i < arr.LevelChain.Length; i++) {
					if (arr.LevelChain[i] != null)
						print(arr.LevelChain[i].name + "-" + arr.LevelChain[i].id.ToString() + "---" + activeGameMode + "-" + activeGameModeID.ToString());
						if (arr.LevelChain[i].id == activeGameModeID)
							theIndex = i;
				}

				if (theIndex != -1) {
					if (theIndex + 1 < arr.LevelChain.Length) {
						if (arr.LevelChain[theIndex + 1] != null) {
							DataLogger.LogMessage("Next Level in chain found! " + n.ToString() + " - " + theIndex.ToString());
							return arr.LevelChain[theIndex + 1];
						} 
					}

					//we found our level but the next level is not set
					DataLogger.LogMessage("Next Level in chain NOT FOUND " + activeGameMode);
					return null;
				}
				n++;
			}
		}

		//we didnt find our level
		DataLogger.LogMessage ("Next Level in chain NOT FOUND " + activeGameMode);
		return null;
	}

	void Update (){
#if UNITY_EDITOR
		if (isDebug) {
			ChangeGameMode (debugMode);
			isDebug = false;
			CardTypeRandomizer.s.Initialize ();
		}
#endif
	}
		
}
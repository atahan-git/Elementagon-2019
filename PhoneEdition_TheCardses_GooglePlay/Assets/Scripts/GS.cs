using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GS : MonoBehaviour {

	public static GS s;
	public static GameSettings a; //active

	public string activeGameMode;
	public GameSettings defaultMode;

	public bool isDebug = false;
	public GameSettings debugMode;

	[Space]

	public GameSettings[] allModes;

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

		activeGameMode = defaultMode.PresetName;
		a = defaultMode.Copy();


		if (isDebug) {
			activeGameMode = debugMode.PresetName;
			a = debugMode.Copy ();
			isDebug = false;
		}


		GetComponent<SceneMaster> ()._OnLevelWasLoaded += LevelWasLoaded;
	}

	/*public void SetMultiplayer () {
		ChangeGameMode (multiplayerMode);
	}*/

	public void ChangeGameMode (GameSettings mode) {
		a = null;
		a = mode.Copy ();
		DataLogger.LogMessage ("Active Game Settings: " + a.PresetName);
	}

	public void ChangeGameMode (int id) {
		a = null;
		try {
			a = allModes[id].Copy ();
			activeGameMode = a.name;
			DataLogger.LogMessage ("Active Game Settings: " + a.PresetName);
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

	void Update (){
		if (isDebug) {
			activeGameMode = debugMode.PresetName;
			a = debugMode.Copy();
			isDebug = false;
			CardTypeRandomizer.s.Initialize ();
		}
	}
		
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenuController : MonoBehaviour {

	public GameSettings introductionLevel;
	public static SettingsMenuController s;

	void Start () {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		s = this;

		if (PlayerPrefs.GetInt ("FirstLevelDone", 0) == 0) {
			SceneMaster.s.LoadPlayingLevel (GS.s.GetGameModeId(introductionLevel));
		}
	}

	public void ResetProgress () {
		SaveMaster.ResetProgress ();
	}

	public void HackLevels () {
		SaveMaster.UnlockAll ();
	}
}

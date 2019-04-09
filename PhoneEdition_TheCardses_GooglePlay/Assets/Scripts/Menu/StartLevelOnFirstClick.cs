using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartLevelOnFirstClick : MonoBehaviour {

	public GameSettings myLevel;

	Button myBut;

	// Use this for initialization
	void Start () {
		myBut = GetComponent<Button> ();

		if (!SaveMaster.isLevelDone (myLevel)) {
			myBut.onClick.RemoveAllListeners ();
			myBut.onClick.AddListener (OpenLevel);
		}
	}

	public void OpenLevel () {
		SceneMaster.s.LoadPlayingLevel (myLevel.id);
	}
}

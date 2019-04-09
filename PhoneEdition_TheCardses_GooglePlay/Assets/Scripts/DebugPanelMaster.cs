using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanelMaster : MonoBehaviour {

	Toggle[] stuff;

	// Use this for initialization
	void Start () {
		SpawnMore (5);
		stuff = GetComponentsInChildren<Toggle> ();
		SetText (0, "isActive");
		SetText (1, "canSelect");
		SetText (2, "isPowerUp");
		SetText (3, "isEnabled");
		SetText (4, "isSelectedOne");
	}

	void SpawnMore (int amount){
		GameObject myToggle = transform.GetChild (0).gameObject;

		for (int i = 0; i < amount - 1; i++) {
			GameObject ins = (GameObject)Instantiate (myToggle, transform);
		}
	}

	void SetText (int i, string txt){
		stuff [i].GetComponentInChildren<Text> ().text = txt;
	}
	
	// Update is called once per frame
	void Update () {
		SetValue (0, LocalPlayerController.isActive);
		SetValue (1, LocalPlayerController.s.canSelect);
		SetValue (2, LocalPlayerController.s.isPowerUp);
		SetValue (3, LocalPlayerController.s.enabled);
		//SetValue (4, LocalPlayerController.s.GiveMem()[0] != null);
	}

	void SetValue (int i, bool value){
		stuff [i].isOn = value;
	}
}

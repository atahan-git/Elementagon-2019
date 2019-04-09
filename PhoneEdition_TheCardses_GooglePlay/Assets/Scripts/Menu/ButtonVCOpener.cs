using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonVCOpener : MenuTriggerableStuffMaster.Triggerable {

	public ViewController myVC;

	public GameSettings unlockReq;

	public GameSettings lOpenOnFirstClick;

	public DialogTreeAsset dOpenOnFirstClick;

	Button myBut;

	// Use this for initialization
	void Start () {
		myBut = GetComponent<Button> ();

		if (lOpenOnFirstClick != null) {
			if (!SaveMaster.isLevelDone (lOpenOnFirstClick)) {
				myBut.onClick.RemoveAllListeners ();
				myBut.onClick.AddListener (() => SceneMaster.s.LoadPlayingLevel (lOpenOnFirstClick.id));
			}
		}

		if (dOpenOnFirstClick != null) {
			if (!MenuTriggerableStuffMaster.IsTriggerDone (this)) {
				myBut.onClick.RemoveAllListeners ();
				myBut.onClick.AddListener (() => SceneMaster.s.LoadPlayingLevel (lOpenOnFirstClick.id));
			}
		}

		if (unlockReq != null) {
			if (!SaveMaster.isLevelDone (unlockReq)) {
				gameObject.SetActive (false);
			}
		}
	}


	public void Open () {
		MenuMasterController.s.GoToPlace (myVC);
	}
}

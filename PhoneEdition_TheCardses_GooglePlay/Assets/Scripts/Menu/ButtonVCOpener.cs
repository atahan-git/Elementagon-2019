using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonVCOpener : MonoBehaviour {

	public ViewController myVC;

	public enum DisableMode { Disable, SayLocked }
	[Space]
	[Space]
	public DisableMode myDisMode = DisableMode.Disable;
	public GameSettings unlockReq;

	[Space]
	[Tooltip ("Leave -1 for not checking")]
	public int questDecisionLockId = -1;
	[Tooltip ("0 is for undecided quests")]
	public int questDecisionReqValue = -1;

	[Space]
	public GameSettings completeLevelBeforeOpeningMenu;

	Button myBut;

	// Use this for initialization
	void Start () {
		myBut = GetComponent<Button> ();

		if (completeLevelBeforeOpeningMenu != null) {
			if (!SaveMaster.isLevelDone (completeLevelBeforeOpeningMenu)) {
				myBut.onClick.RemoveAllListeners ();
				myBut.onClick.AddListener (() => SceneMaster.s.LoadPlayingLevel (completeLevelBeforeOpeningMenu.id));
			}
		}

		if (!UnlockRequirementKeeper.isUnlocked (unlockReq, questDecisionLockId, questDecisionReqValue)) {
			if (myDisMode == DisableMode.Disable) {
				gameObject.SetActive (false);
			} else {
				try {
					GetComponent<TMPro.TextMeshProUGUI> ().text = "Locked";
				} catch { }
			}
		}
	}


	public void Open () {
		MenuMasterController.s.GoToPlace (myVC);
	}
}

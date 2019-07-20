using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelHolder : MonoBehaviour {

	public GameSettings mySettings;
	public GameSettings unlockReq;
	public GameSettings unlockReqAlt;

	[Tooltip("Leave -1 for not checking")]
	public int questDecisionLockId = -1;
	public float questDecisionReqValue = -1;

	[Space]

	public Button myButton;
	public TextMeshProUGUI myText;
	public Image mybutImg;


	public Text nameText;
	public Text descriptionText;
	public Image startingImage;


	public Sprite finishedButImg;
	public Sprite lockedButImg;
	public Sprite unlockedButImg;

	public bool isInitialized = false;
	public bool isUnlocked = true;
	// Use this for initialization
	public void Start () {
		if (myText != null)
			myText.text = mySettings.levelShortName;
		if (nameText != null)
			nameText.text = mySettings.levelName;
		if (descriptionText != null)
			descriptionText.text = mySettings.levelDescription;
		if (startingImage != null)
			startingImage.sprite = mySettings.startingImage;

		if (mySettings != null) {
			GameSettings myFinalLevel = mySettings;
			while (myFinalLevel.nextStage != null) {
				myFinalLevel = myFinalLevel.nextStage;
			}

			if (SaveMaster.isLevelDone (myFinalLevel)) {
				myButton.interactable = true;
				isUnlocked = true;
				mybutImg.sprite = finishedButImg;
			} else {
				myButton.interactable = true;
				isUnlocked = true;
				mybutImg.sprite = unlockedButImg;
			}

		}

		if (unlockReq != null) {
			bool isAlt = true;
			if (unlockReqAlt != null) {
				isAlt = SaveMaster.isLevelDone (unlockReqAlt);
			}

			if (!SaveMaster.isLevelDone (unlockReq) || !isAlt) {
				myButton.interactable = false;
				isUnlocked = false;
				mybutImg.sprite = lockedButImg;
			}
		}

		if (questDecisionLockId != -1) {
			try {
				if (SaveMaster.s.mySave.questDecisions.Length < questDecisionLockId) {
					float[] temp = SaveMaster.s.mySave.questDecisions;
					SaveMaster.s.mySave.questDecisions = new float[questDecisionLockId];
					temp.CopyTo (SaveMaster.s.mySave.questDecisions, 0);
				}
				if (SaveMaster.s.mySave.questDecisions[questDecisionLockId] != questDecisionReqValue) {
					myButton.interactable = false;
					isUnlocked = false;
					mybutImg.sprite = lockedButImg;
				}
			} catch {
				DataLogger.LogError ("Problem Checking Quest Decision Choice " + questDecisionLockId.ToString());
			}
		}
		isInitialized = true;
	}

	public void TriggerLevelCompleteAnimation () {

	}

	public GameObject levelUnlockedEffects;
	public void TriggerLevelUnlockedAnimation () {
		levelUnlockedEffects.SetActive (true);
	}

	public void ClickButton () {
		//GetComponentInParent<VC_LevelViewer> ().SelectLevel (mySettings);
		((VC_LevelViewer)MenuMasterController.s.OpenOverlay (0)).SelectLevel(mySettings);
	}
}

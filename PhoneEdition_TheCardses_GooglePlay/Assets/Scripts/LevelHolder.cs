using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelHolder : MonoBehaviour {

	public GameSettings mySettings;
	public GameSettings unlockReq;

	public Button myButton;
	public Text myText;
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
			int unlockReqId = GS.s.GetGameModeId (unlockReq);

			if (!SaveMaster.isLevelDone (unlockReq)) {
				myButton.interactable = false;
				isUnlocked = false;
				mybutImg.sprite = lockedButImg;
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

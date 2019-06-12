using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoranCustomScript : MonoBehaviour {


	public GameObject finishButton;
	GameObject otherPowerButton;

	RadialChargeImage fire;
	RadialChargeImage earth;

	public GameObject tutPanel;
	public GameObject powerSelect;
	public GameObject cardSelect;

	public GameObject decidePanel;

	public GameObject youCanExitHerePanel;
	// Use this for initialization
	void Start () {
		DialogTree.s.myCustomTriggers[0] += StartElementTrial;
		DialogTree.s.myCustomTriggers[1] += EndGame;
		CharacterStuffController.s.buttonStateHijack += ButtonStateHiJack;
		CharacterStuffController.s.isHijacked = true;
		CharacterStuffController.s.powerUpDisabledCallback += PowerUpDisabledCallback;

		tutPanel.SetActive (false);
		decidePanel.SetActive (false);
		youCanExitHerePanel.SetActive (false);

		SetUpButtons ();
	}

	public void SetUpButtons () {
		Destroy (DownPanelReferenceHolder.s.potions);
		Destroy (DownPanelReferenceHolder.s.equipments);
		Destroy (DownPanelReferenceHolder.s.objective.transform.GetChild (0).gameObject);
		Destroy (DownPanelReferenceHolder.s.backToMenu);

		otherPowerButton = Instantiate (DownPanelReferenceHolder.s.power, DownPanelReferenceHolder.s.power.transform.parent);
		otherPowerButton.transform.SetSiblingIndex (1);

		finishButton = Instantiate (finishButton, DownPanelReferenceHolder.s.objective.transform);
		finishButton.GetComponent<Button> ().onClick.AddListener (EndTrying);

		fire = DownPanelReferenceHolder.s.power.GetComponentInChildren<RadialChargeImage> ();
		earth = otherPowerButton.GetComponentInChildren<RadialChargeImage> ();

		fire.GetComponent<Button> ().onClick.AddListener (ActivateFire);
		earth.GetComponent<Button> ().onClick.AddListener (ActivateEarth);

		print ("Changed button listeners");

		fire.SetUp (GS.a.gfxs.cardSprites[2 + 7], 1f, "Fire", 2);
		earth.SetUp (GS.a.gfxs.cardSprites[1 + 7], 1f, "Earth", 1);
	}

	public void StartElementTrial () {
		DialogDisplayer.s.SetDialogScreenState (false);
		LocalPlayerController.isActive = true;
		LocalPlayerController.s.canSelect = false;

		tutPanel.SetActive (true);
		cardSelect.SetActive (false);
		powerSelect.SetActive (true);
	}


	bool isDoneFire = false;
	public void ActivateFire () {
		LocalPlayerController.s.canSelect = true;
		DataLogger.LogMessage ("Activating fire");
		lastActiveFire = true;
		PowerUpManager.s.EnablePowerUp (PowerUpManager.PUpTypes.equipment, 4, 2, 4, 1);

		cardSelect.SetActive (true);
		powerSelect.SetActive (false);

		isDoneFire = true;
	}

	bool isDoneEarth = false;
	public void ActivateEarth () {
		LocalPlayerController.s.canSelect = true;
		DataLogger.LogMessage ("Activating earth");
		lastActiveFire = false;
		PowerUpManager.s.EnablePowerUp (PowerUpManager.PUpTypes.equipment, 14, 1, 4, 1);

		cardSelect.SetActive (true);
		powerSelect.SetActive (false);

		isDoneEarth = true;
	}

	bool lastActiveFire;
	public void ButtonStateHiJack (float _maxCharge, float _curCharge, bool _canActivate, bool _isActive) {
		RadialChargeImage myButton = null;
		if (lastActiveFire)
			myButton = fire;
		else
			myButton = earth;

		if (_maxCharge == -5) {
			_maxCharge = myButton.maxCharge;
			tutPanel.SetActive (false);
			GameObjectiveFinishChecker.s.isGamePlaying = true;
		}


		myButton.SetState (_maxCharge, _curCharge, _canActivate, _isActive);
		
	}

	public void PowerUpDisabledCallback () {
		StartCoroutine (ChargeUpButton(lastActiveFire));
		if (isDoneEarth && isDoneFire && !canDisExit) {
			Invoke ("ActivateYouCanExit", 0.5f);
			Invoke ("CanDisableCanExit", 1f);
		}
	}

	void ActivateYouCanExit () {
		youCanExitHerePanel.SetActive (true);
	}

	bool canDisExit = false;
	void CanDisableCanExit () {
		canDisExit = true;
	}

	private void Update () {
		if (canDisExit) {
			if (Input.GetMouseButtonDown (0)) {
				youCanExitHerePanel.SetActive (false);
			}
		}
	}

	IEnumerator ChargeUpButton (bool isFire) {
		float charge = 0;
		RadialChargeImage myButton = isFire ? fire : earth;

		while (charge <= 1f) {
			myButton.SetState (1, charge,false, false);
			charge += Time.deltaTime/5f;
			yield return null;
		}

		myButton.SetState (1, 1, true, false);

		yield return null;
	}


	public void EndTrying () {
		tutPanel.SetActive (false);
		LocalPlayerController.isActive = false;
		decidePanel.SetActive (true);
		GameObjectiveFinishChecker.s.isGamePlaying = false;
	}

	//1 = earth, 2 = fire
	public void ChoosePower (int id) {
		if (SaveMaster.s.mySave.unlockedElementLevels == null)
			SaveMaster.s.mySave.unlockedElementLevels = new int[7];

		if (SaveMaster.s.mySave.unlockedElementLevels.Length == 0)
			SaveMaster.s.mySave.unlockedElementLevels = new int[7];

		if (SaveMaster.s.mySave.unlockedElementLevels[id] == 0)
			SaveMaster.s.mySave.unlockedElementLevels[id] = 1;

		InventoryMaster.s.selectedElement = id;
		InventoryMaster.s.elementLevel = 1;

		SaveMaster.s.Save ();

		decidePanel.SetActive (false);
		DialogDisplayer.s.SetDialogScreenState (true);
		DialogDisplayer.s.NextDialog ();
	}

	public GameSettings doranLevel;
	public void EndGame () {
		SaveMaster.LevelFinished (true, doranLevel);
		SceneMaster.s.LoadMenu ();
	}
}

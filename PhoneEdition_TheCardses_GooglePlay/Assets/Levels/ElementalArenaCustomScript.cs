using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class ElementalArenaCustomScript : MonoBehaviour {

	GameObject powerButtonOne;
	GameObject powerButtonTwo;

	RadialChargeImage imgButtonOne;
	RadialChargeImage imgButtonTwo;

	int pupOne = -1;
	int pupTwo = -1;


	bool isFirstSetButtonOne;
	bool lastActiveButtonOne;

	private void Start () {
		DataLogger.LogMessage ("Elemental Arena Script Activating...");

		ScoreBoardManager.s.AddScoreHook += ScoreIsAdded;

		CharacterStuffController.s.buttonStateHijack += ButtonStateHiJack;
		CharacterStuffController.s.isHijacked = true;
		CharacterStuffController.s.powerUpDisabledCallback += PowerUpDisabledCallback;
		SetUpButtons ();

		DataLogger.LogMessage ("Elemental Arena Script Activated Succesfully");
	}

	public void SetUpButtons () {
		Destroy (DownPanelReferenceHolder.s.potions);
		Destroy (DownPanelReferenceHolder.s.equipments);

		powerButtonTwo = Instantiate (DownPanelReferenceHolder.s.power, DownPanelReferenceHolder.s.power.transform.parent);
		powerButtonTwo.transform.SetSiblingIndex (1);

		powerButtonOne = DownPanelReferenceHolder.s.power;


		imgButtonOne = powerButtonOne.GetComponentInChildren<RadialChargeImage> ();
		imgButtonTwo = powerButtonTwo.GetComponentInChildren<RadialChargeImage> ();

		imgButtonOne.GetComponent<Button> ().onClick.AddListener (ActivateButtonOne);
		imgButtonTwo.GetComponent<Button> ().onClick.AddListener (ActivateButtonTwo);

		print ("Changed button listeners");

		imgButtonOne.SetUp (CharacterStuffController.s.noEquipmentSprite, 1, "Not Equipped", 0);
		imgButtonTwo.SetUp (CharacterStuffController.s.noEquipmentSprite, 1, "Not Equipped", 0);
	}

	public void ActivateButtonOne () {
		lastActiveButtonOne = true;
		ActivatePup (pupOne);
	}

	public void ActivateButtonTwo () {
		lastActiveButtonOne = false;
		ActivatePup (pupTwo);
	}

	//---------------------------
	// 8 = Earth Dragon
	// 9 = Fire Dragon
	//10 = Ice Dragon
	//11 = Light Dragon
	//12 = Nether Dragon
	//13 = Poison Dragon
	//14 = Shadow Dragon
	//---------------------------

	public void ActivatePup (int id) {
		switch (id) {
		case 8:
			ActivateEarth ();
			break;
		case 9:
			ActivateFire ();
			break;
		case 10:
			ActivateIce ();
			break;
		case 11:
			ActivateLight ();
			break;
		case 12:
			ActivateNether ();
			break;
		case 13:
			ActivatePoison ();
			break;
		case 14:
			ActivateShadow ();
			break;
		}
	}

	public void ActivateEarth () {
		DataLogger.LogMessage ("Activating earth");
		PowerUpManager.s.EnablePowerUp (PowerUpManager.PUpTypes.equipment, 14, 1, 4, 1);
	}

	public void ActivateFire () {
		DataLogger.LogMessage ("Activating fire");
		PowerUpManager.s.EnablePowerUp (PowerUpManager.PUpTypes.equipment, 4, 2, 4, 1);
	}
	
	public void ActivateIce () {
		return;
		DataLogger.LogMessage ("Activating ice");
		PowerUpManager.s.EnablePowerUp (PowerUpManager.PUpTypes.equipment, 14, 1, 4, 1);
	}

	public void ActivateLight () {
		return;
		DataLogger.LogMessage ("Activating light");
		PowerUpManager.s.EnablePowerUp (PowerUpManager.PUpTypes.equipment, 14, 1, 4, 1);
	}

	public void ActivateNether () {
		return;
		DataLogger.LogMessage ("Activating nether");
		PowerUpManager.s.EnablePowerUp (PowerUpManager.PUpTypes.equipment, 14, 1, 4, 1);
	}

	public void ActivatePoison () {
		return;
		DataLogger.LogMessage ("Activating poison");
		PowerUpManager.s.EnablePowerUp (PowerUpManager.PUpTypes.equipment, 14, 1, 4, 1);
	}

	public void ActivateShadow () {
		return;
		DataLogger.LogMessage ("Activating shadow");
		PowerUpManager.s.EnablePowerUp (PowerUpManager.PUpTypes.equipment, 14, 1, 4, 1);
	}

	public void ButtonStateHiJack (float _maxCharge, float _curCharge, bool _canActivate, bool _isActive) {
		RadialChargeImage myButton = null;
		if (lastActiveButtonOne)
			myButton = imgButtonOne;
		else
			myButton = imgButtonTwo;

		if (_maxCharge == -5) {
			_maxCharge = myButton.maxCharge;
		}


		myButton.SetState (_maxCharge, _curCharge, _canActivate, _isActive);

	}

	public void PowerUpDisabledCallback () {
		StartCoroutine (ChargeUpButton (lastActiveButtonOne));
	}
	

	IEnumerator ChargeUpButton (bool isOne) {
		float charge = 0;
		RadialChargeImage myButton = isOne ? imgButtonOne : imgButtonTwo;

		while (charge <= 1f) {
			myButton.SetState (1, charge, false, false);
			charge += Time.deltaTime / 5f;
			yield return null;
		}

		myButton.SetState (1, 1, true, false);

		yield return null;
	}





	void ScoreIsAdded (int playerInt, int scoreElementType, int toAdd, bool isDelayed, bool careGameTypes) {
		if (playerInt == DataHandler.s.myPlayerInteger) {
			if (scoreElementType > 7 && scoreElementType <=14) {
				if (pupOne == -1) {
					SetButtonElement (true, scoreElementType);
					pupOne = scoreElementType;

					if (pupTwo == -1)
						isFirstSetButtonOne = true;
					else
						isFirstSetButtonOne = false;

				} else if (pupTwo == -1) {
					SetButtonElement (false, scoreElementType);
					pupTwo = scoreElementType;

				} else {
					SetButtonElement (isFirstSetButtonOne, scoreElementType);
					if (isFirstSetButtonOne)
						pupOne = scoreElementType;
					else
						pupTwo = scoreElementType;

					isFirstSetButtonOne = !isFirstSetButtonOne;
				}
			}
		}
	}

	//---------------------------
	// 8 = Earth Dragon
	// 9 = Fire Dragon
	//10 = Ice Dragon
	//11 = Light Dragon
	//12 = Nether Dragon
	//13 = Poison Dragon
	//14 = Shadow Dragon
	//---------------------------

	void SetButtonElement (bool isButtonOne, int type) {

		/*imgButtonOne.SetUp (GS.a.gfxs.cardSprites[2 + 7], 1f, "Fire", 2);
		imgButtonTwo.SetUp (GS.a.gfxs.cardSprites[1 + 7], 1f, "Earth", 1);*/

		RadialChargeImage myBut = isButtonOne ? imgButtonOne : imgButtonTwo;

		string elementName = "Error";

		switch (type) {
		case 8:
			elementName = "Earth";
			break;
		case 9:
			elementName = "Fire";
			break;
		case 10:
			elementName = "Ice";
			break;
		case 11:
			elementName = "Light";
			break;
		case 12:
			elementName = "Nether";
			break;
		case 13:
			elementName = "Poison";
			break;
		case 14:
			elementName = "Shadow";
			break;
		}

		myBut.SetUp (GS.a.gfxs.cardSprites[type], 1f, elementName, type - 7);
	}
}
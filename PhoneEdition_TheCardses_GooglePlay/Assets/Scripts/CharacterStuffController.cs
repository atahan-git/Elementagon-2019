using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStuffController : MonoBehaviour {

	public static CharacterStuffController s;

	public GameObject potionsScreen;
	public GameObject potionsParent;
	public GameObject gridItem;

	public RadialChargeImage powerButton;
	public bool isActivePower = true;
	public RadialChargeImage equipmentButton;
	public bool isActiveEquipment;

	public Sprite noEquipmentSprite;

	Equipment myEquipment;

	public bool isHijacked = false;

	// Use this for initialization
	void Awake () {
		s = this;
		potionsScreen.SetActive (false);
		Invoke ("SetUpButtons", 0.1f);
	}

	[Tooltip ("//--------CARD TYPES---------\n// 0 = any type\n// 1-7 = normal cards\n// 8-14 = dragons\n//---------------------------\n// 1 = Earth\n// 2 = Fire\n// 3 = Ice\n// 4 = Light\n// 5 = Nether\n// 6 = Poison\n// 7 = Shadow\n//---------------------------\n// 8 = Earth Dragon\n// 9 = Fire Dragon\n//10 = Ice Dragon\n//11 = Light Dragon\n//12 = Nether Dragon\n//13 = Poison Dragon\n//14 = Shadow Dragon\n//---------------------------")]
	public int[] curEquipmentChargeReq = new int[32];
	public int maxEquipmentCharge;
	[Tooltip ("//--------CARD TYPES---------\n// 0 = any type\n// 1-7 = normal cards\n// 8-14 = dragons\n//---------------------------\n// 1 = Earth\n// 2 = Fire\n// 3 = Ice\n// 4 = Light\n// 5 = Nether\n// 6 = Poison\n// 7 = Shadow\n//---------------------------\n// 8 = Earth Dragon\n// 9 = Fire Dragon\n//10 = Ice Dragon\n//11 = Light Dragon\n//12 = Nether Dragon\n//13 = Poison Dragon\n//14 = Shadow Dragon\n//---------------------------")]
	public int[] PowerChargeReq = new int[32];
	[Tooltip ("//--------CARD TYPES---------\n// 0 = any type\n// 1-7 = normal cards\n// 8-14 = dragons\n//---------------------------\n// 1 = Earth\n// 2 = Fire\n// 3 = Ice\n// 4 = Light\n// 5 = Nether\n// 6 = Poison\n// 7 = Shadow\n//---------------------------\n// 8 = Earth Dragon\n// 9 = Fire Dragon\n//10 = Ice Dragon\n//11 = Light Dragon\n//12 = Nether Dragon\n//13 = Poison Dragon\n//14 = Shadow Dragon\n//---------------------------")]
	public int[] curPowerChargeReq = new int[32];
	
	public int maxPowerCharge;

	public void SetUpButtons () {
		if (isHijacked)
			return;

		try {
			foreach (InventoryMaster.InventoryPotion myItem in InventoryMaster.s.myPotions) {
				if (myItem != null)
					((GameObject)Instantiate (gridItem, potionsParent.transform)).GetComponent<ItemGridDisplay> ().SetUp (myItem);
			}

			if (InventoryMaster.s.elementLevel > 0) {
				PowerChargeReq[/*InventoryMaster.s.selectedElement + 1*/17] = InventoryMaster.s.elementLevel;
				maxPowerCharge = InventoryMaster.s.elementLevel;
				powerButton.SetUp (GS.a.gfxs.cardSprites[InventoryMaster.s.selectedElement + 1 + 7], maxPowerCharge, "Power", InventoryMaster.s.selectedElement + 1);
				isActivePower = true;
			} else {
				powerButton.SetUp (noEquipmentSprite, 1, "Not Equipped", 0);
				isActivePower = false;
			}

			if (InventoryMaster.s.activeEquipment != null) {
				myEquipment = (Equipment)InventoryMaster.s.activeEquipment.item;
				maxEquipmentCharge = AddUpIntArray (myEquipment.chargeReq);
				equipmentButton.SetUp (myEquipment.sprite, maxEquipmentCharge, myEquipment.name, myEquipment.elementalType);
				isActiveEquipment = true;

				if (PowerUpManager.s.equipmentPUps[(int)myEquipment.myType] is IPassive) {
					((IPassive)PowerUpManager.s.equipmentPUps[(int)myEquipment.myType]).Enable (myEquipment.elementalType, myEquipment.power);
					isActiveEquipment = false;
				}

			} else {
				isActiveEquipment = false;
				equipmentButton.SetUp (noEquipmentSprite, 1, "Not Equipped", 0);
			}

			UpdateChargeReqs ();
		} catch (System.Exception e) {
			//DataLogger.LogMessage ("Data processing failed " + myCommand.ToString (), true);
			DataLogger.LogError (this.name, e);
		}
	}

	int AddUpIntArray (int[] arr) {
		int total = 0;
		foreach (int i in arr) {
			total += i;
		}
		return total;
	}

	public void ScoreAdded (int scoreType) {
		print ("Score added: " + scoreType.ToString ());
		if (isHijacked)
			return;

		if (curEquipmentChargeReq[scoreType] > 0) {
			curEquipmentChargeReq[scoreType]--;
		} else if (curEquipmentChargeReq[0] > 0) {
			curEquipmentChargeReq[0]--;
		}

		if (curPowerChargeReq[scoreType] > 0) {
			curPowerChargeReq[scoreType]--;
		} else if (curPowerChargeReq[0] > 0) {
			curPowerChargeReq[0]--;
		}

		UpdateChargeReqs ();
	}

	public void InstantActivate () {
		curEquipmentChargeReq = new int[32];
		curPowerChargeReq = new int[32];
		UpdateChargeReqs ();
	}

	public void UpdateChargeReqs () {
		if (isHijacked) {
			return;
		}

		if (!equipmentButton.isActive && isActiveEquipment) {
			bool canActivate = AddUpIntArray (curEquipmentChargeReq) <= 0;
			equipmentButton.SetState (maxEquipmentCharge, maxEquipmentCharge - AddUpIntArray (curEquipmentChargeReq), canActivate, false);
			equipmentButton.myReqs.SetUp (curEquipmentChargeReq);
		}
		if (!powerButton.isActive & isActivePower) {
			bool canActivate = AddUpIntArray (curPowerChargeReq) <= 0;
			powerButton.SetState (maxPowerCharge, maxPowerCharge - AddUpIntArray (curPowerChargeReq), canActivate, false);
			powerButton.myReqs.SetUp (curPowerChargeReq);
		}
	}

	public bool isLastActivePower = false;
	public void EnableEquipment () {
		if (isHijacked) {
			DataLogger.LogMessage ("Enable Equipment got hijacked");
			return;
		}

		if (equipmentButton.canActivate) {
			isLastActivePower = false;
			if (!DataLogger.isDebugMode)
				((Equipment)InventoryMaster.s.activeEquipment.item).chargeReq.CopyTo (curEquipmentChargeReq, 0);
			Equipment myEq = (Equipment)InventoryMaster.s.activeEquipment.item;
			PowerUpManager.s.EnablePowerUp (PowerUpManager.PUpTypes.equipment, 
				(int)myEq.myType, 
				myEq.elementalType, 
				myEq.power, 
				myEq.amount);
		}
	}

	// power to type mapping:
	/* 0 -> 14
	 * 1 -> 4
	 * 
	 * 
	 * 
	 */
	public void EnablePower () {
		if (isHijacked) {
			DataLogger.LogMessage ("Enable Power got hijacked");
			return;
		}

		if (powerButton.canActivate) {
			isLastActivePower = true;
			if (!DataLogger.isDebugMode)
				PowerChargeReq.CopyTo (curPowerChargeReq, 0);
			PowerUpManager.s.EnablePowerUp (PowerUpManager.PUpTypes.equipment, 
				ConverElementToType(InventoryMaster.s.selectedElement), 
				InventoryMaster.s.selectedElement + 1, 
				InventoryMaster.s.elementLevel, 1);
		}
	}

	public int ConverElementToType (int elementType) {
		switch (elementType) {
		case 0:
			return 14;
		case 1:
			return 4;
		default:
			return 4;
		}
	}

	public delegate void ButtonStateDelegate (float _maxCharge, float _curCharge, bool _canActivate, bool _isActive);
	public ButtonStateDelegate buttonStateHijack;
	public void SetActiveButtonState (float _maxCharge, float _curCharge, bool _canActivate, bool _isActive) {
		if (buttonStateHijack != null) {
			buttonStateHijack.Invoke (_maxCharge, _curCharge, _canActivate, _isActive);
		}
		if (isHijacked)
			return;

		RadialChargeImage myButton = null;
		if (isLastActivePower)
			myButton = powerButton;
		else
			myButton = equipmentButton;

		if (_maxCharge == -5) 
			_maxCharge = myButton.maxCharge;
		

		myButton.SetState (_maxCharge, _curCharge, _canActivate, _isActive);
	}

	public delegate void myDelegate ();
	public myDelegate powerUpDisabledCallback;
	public void PowerUpDisabledCallback () {
		if (powerUpDisabledCallback != null)
			powerUpDisabledCallback.Invoke ();
	}

	public void ActivatePotion (InventoryMaster.InventoryItem theItem) {
		if (isHijacked)
			return;

		InventoryMaster.s.Remove (theItem);
	}


	public void OpenPotionsScreen () {
		if (isHijacked)
			return;
		potionsScreen.SetActive (true);
		LocalPlayerController.s.canSelect = false;
	}

	public void HidePotionsScreen () {
		if (isHijacked)
			return;
		potionsScreen.SetActive (false);
		LocalPlayerController.s.canSelect = true;
	}
}

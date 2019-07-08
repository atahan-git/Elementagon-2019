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
	public bool isEquippedPower = false;
	public RadialChargeImage equipmentButton;
	public bool isEquippedEquipment = false;

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
			DrawPotionScreen ();

			int elementLevel = GS.a.overridePower ? GS.a.elementLevel : InventoryMaster.s.elementLevel;
			int selectedElement = GS.a.overridePower ? GS.a.selectedElement : InventoryMaster.s.selectedElement;
			if (elementLevel > 0) {
				PowerChargeReq[/*InventoryMaster.s.selectedElement + 1*/17] = elementLevel;
				maxPowerCharge = elementLevel;
				powerButton.SetUp (GS.a.gfxs.cardSprites[selectedElement + 1 + 7], maxPowerCharge, "Power", selectedElement + 1);
				isEquippedPower = true;
			} else {
				powerButton.SetUp (noEquipmentSprite, 1, "Not Equipped", 0);
				isEquippedPower = false;
			}
			InventoryMaster.InventoryEquipment myEq = (GS.a.overrideEquipment ? new InventoryMaster.InventoryEquipment (GS.a.equipment, 1) : InventoryMaster.s.activeEquipment);
			if (myEq != null) {
				myEquipment = (Equipment)myEq.item;
				maxEquipmentCharge = AddUpIntArray (myEquipment.chargeReq);
				equipmentButton.SetUp (myEquipment.sprite, maxEquipmentCharge, myEquipment.name, myEquipment.elementalType);
				isEquippedEquipment = true;

				if (PowerUpManager.s.equipmentPUps[(int)myEquipment.myType] is IPassive) {
					((IPassive)PowerUpManager.s.equipmentPUps[(int)myEquipment.myType]).Enable (myEquipment.elementalType, myEquipment.power);
					isEquippedEquipment = false;
				}

			} else {
				isEquippedEquipment = false;
				equipmentButton.SetUp (noEquipmentSprite, 1, "Not Equipped", 0);
			}

			UpdateChargeReqs ();
		} catch (System.Exception e) {
			//DataLogger.LogMessage ("Data processing failed " + myCommand.ToString (), true);
			DataLogger.LogError (this.name, e);
		}
	}

	public void DrawPotionScreen () {
		int childCount = potionsParent.transform.childCount;
		for (int i = childCount-1; i >= 0; i--) {
			Destroy (potionsParent.transform.GetChild(i).gameObject);
		}
		foreach (InventoryMaster.InventoryPotion myItem in GS.a.overridePotions ? GS.a.potions.ConvertToInventory () : InventoryMaster.s.myPotions.ToArray ()) {
			if (myItem != null)
				Instantiate (gridItem, potionsParent.transform).GetComponent<ItemGridDisplay> ().SetUp (myItem);
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
		//print ("Score added: " + scoreType.ToString ());
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

		if (!equipmentButton.isActive && isEquippedEquipment) {
			bool canActivate = AddUpIntArray (curEquipmentChargeReq) <= 0;
			equipmentButton.SetState (maxEquipmentCharge, maxEquipmentCharge - AddUpIntArray (curEquipmentChargeReq), canActivate, false);
			equipmentButton.myReqs.SetUp (curEquipmentChargeReq);
		}
		if (!powerButton.isActive & isEquippedPower) {
			bool canActivate = AddUpIntArray (curPowerChargeReq) <= 0;
			powerButton.SetState (maxPowerCharge, maxPowerCharge - AddUpIntArray (curPowerChargeReq), canActivate, false);
			powerButton.myReqs.SetUp (curPowerChargeReq);
		}
	}

	public bool lastActivatedButton = false;
	public bool isLastActivePower = false;
	public void EnableEquipment () {
		if (isHijacked) {
			DataLogger.LogMessage ("Enable Equipment got hijacked");
			return;
		}

		if (equipmentButton.canActivate) {
			isLastActivePower = false;
			lastActivatedButton = true;
			Equipment myEq = (Equipment)(GS.a.overrideEquipment ? new InventoryMaster.InventoryEquipment (GS.a.equipment, 1) : InventoryMaster.s.activeEquipment).item;
			if (!DataLogger.isDebugMode)
				(myEq).chargeReq.CopyTo (curEquipmentChargeReq, 0);
			PowerUpManager.s.EnablePowerUp (PowerUpManager.PUpTypes.equipment, 
				(int)myEq.myType, 
				myEq.elementalType, 
				myEq.power, 
				myEq.amount);
		}
	}

	
	public void EnablePower () {
		if (isHijacked) {
			DataLogger.LogMessage ("Enable Power got hijacked");
			return;
		}

		if (powerButton.canActivate) {
			isLastActivePower = true;
			lastActivatedButton = true;
			if (!DataLogger.isDebugMode)
				PowerChargeReq.CopyTo (curPowerChargeReq, 0);
			PowerUpManager.s.EnablePowerUp (PowerUpManager.PUpTypes.equipment, 
				ConverElementToType(GS.a.overridePower ? GS.a.selectedElement : InventoryMaster.s.selectedElement),
				(GS.a.overridePower ? GS.a.selectedElement : InventoryMaster.s.selectedElement) + 1,
				GS.a.overridePower ? GS.a.elementLevel : InventoryMaster.s.elementLevel, 1);
		}
	}

	// 1 = Earth
	// 2 = Fire
	// 3 = Ice
	// 4 = Light
	// 5 = Nether
	// 6 = Poison
	// 7 = Shadow
	// power to type mapping:
	/* 1 -> 14 
	 * 2 -> 4
	 * 3 -> 15
	 * 4 -> 9
	 * 5 -> 16
	 * 6 -> 7
	 * 7 -> 17
	 */
	 //because the dragon powers may match to pups of different ids we must match them
	public int ConverElementToType (int elementType) {
		switch (elementType+1) {
		case 1:
			return 14;
		case 2:
			return 4;
		case 3:
			return 15;
		case 4:
			return 9;
		case 5:
			return 16;
		case 6:
			return 7;
		case 7:
			return 17;
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
		if (!lastActivatedButton)
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
		DataLogger.LogMessage ("Potion Activated! " + theItem.item.name);
		if (isHijacked)
			return;

		Potion myPot = (Potion)theItem.item;
		PowerUpManager.s.EnablePowerUp (PowerUpManager.PUpTypes.potion,
				(int)myPot.myType,
				myPot.elementalType,
				-1,
				myPot.amount);

		if (!GS.a.overridePotions)
			InventoryMaster.s.Remove (theItem);
		else {
			List<Potion> myPotions = new List<Potion> (GS.a.potions);
			myPotions.Remove ((Potion)theItem.item);
			GS.a.potions = myPotions.ToArray ();
		}
	}

	public bool isPotionScreenOpen = false;
	public void OpenPotionsScreen () {
		if (isHijacked)
			return;
		potionsScreen.SetActive (true);
		isPotionScreenOpen = true;
		LocalPlayerController.s.canSelect = false;
	}

	public void HidePotionsScreen () {
		if (isHijacked)
			return;
		potionsScreen.SetActive (false);
		isPotionScreenOpen = false;
		LocalPlayerController.s.canSelect = true;
	}
}

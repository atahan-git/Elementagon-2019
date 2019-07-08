using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMaster : MonoBehaviour {

	public static InventoryMaster s;

	public bool cheatInventory = false;

	public List<InventoryEquipment> myEquipments = new List<InventoryEquipment> ();
	public List<InventoryIngredient> myIngredients = new List<InventoryIngredient> ();
	public List<InventoryPotion> myPotions = new List<InventoryPotion> ();


	public InventoryEquipment _activeEquipment; 
	public InventoryEquipment activeEquipment {
		get {
			if (_activeEquipment != null) {
				if (_activeEquipment.item == null)
					_activeEquipment = null;
			}
			return _activeEquipment;
		}
		set {
			_activeEquipment = value;
		}
	}


	[Tooltip ("//0-7 = dragons\n//---------------------------\n//0 = Earth Dragon\n//1 = Fire Dragon\n//2 = Ice Dragon\n//3 = Light Dragon\n//4 = Nether Dragon\n//5 = Poison Dragon\n//6 = Shadow Dragon\n//---------------------------")]
	public int selectedElement = -1;
	public int elementLevel = 0;

	[Space]

	public Equipment[] allEquipments;
	public Ingredient[] allIngredients;
	public Potion[] allPotions;

	[Space]
	public Recipe[] allRecipes;


	public void Start () {
		if (s != null && s != this) {
			Destroy (this.gameObject);
			return;
		} else {
			s = this;
		}

		bool isThere = false;
		if (SaveMaster.toSave != null) {
			foreach (SaveMaster.SaveDelegate del in SaveMaster.toSave.GetInvocationList ()) {
				if (del == Save) {
					isThere = true;
					break;
				}
			}
		}

		if(!isThere)
			SaveMaster.toSave += Save;

		if (SaveMaster.s.mySave.activeEquipment != "none")
			activeEquipment = myEquipments.Find(x => x.item.name == SaveMaster.s.mySave.activeEquipment);
		else
			activeEquipment = null;
		
		selectedElement = SaveMaster.s.mySave.selectedElement;
		elementLevel = SaveMaster.s.mySave.elementLevel;

		if (!cheatInventory) {
			myEquipments = new List<InventoryEquipment>(SaveMaster.s.mySave.myEquipments.ConvertToInventory ());
			myPotions = new List<InventoryPotion> (SaveMaster.s.mySave.myPotions.ConvertToInventory ());
		} else {
			print ("copying all items");
			foreach (Equipment equipment in allEquipments) {
				if (equipment != null)
					myEquipments.Add (new InventoryEquipment (equipment, 5));
			}
			foreach (Ingredient ingredient in allIngredients) {
				if (ingredient != null) {
					myIngredients.Add (new InventoryIngredient (ingredient, 5));
				}
			}
			foreach (Potion potion in allPotions) {
				if (potion != null)
					myPotions.Add (new InventoryPotion (potion, 5));
			}
		}

		/*if (SceneMaster.s.curScene == 0) {
			MenuSwitchController.s.GetComponent<CharacterMenuController> ().SetEquipped ();
			ElementSelector.s.UpdateElementSliders ();
		}*/
	}


	//this is only done at the end of the game
	public void ReduceEquipmentChargeLeft () {
		DataLogger.LogMessage ("Reducing equipment charge");
		if (activeEquipment != null) {
			DataLogger.LogMessage ("Equipment " + activeEquipment.item.name + " charge reduced from "
				+ (activeEquipment.chargesLeft).ToString () + " to "
				+ (activeEquipment.chargesLeft-1).ToString ());
			ItemDurabilityLossScreen.s.ShowDurabilityLoss (activeEquipment);
			Remove (activeEquipment);
		}
	}


	//----------------- helpers

	public void EquipItem (InventoryEquipment toEquip) {
		if (toEquip == null)
			return;
		activeEquipment = toEquip;
		SaveMaster.s.Save ();

		if (SceneMaster.s.curScene == 0) {
			if (VC_CharacterMenuController.s != null)
				VC_CharacterMenuController.s.SetEquipped ();
		} else {
			CharacterStuffController.s.SetUpButtons ();
		}
	}

	public void Add (ItemBase toAdd, int amount) {
		DataLogger.LogMessage ("Item added: " + toAdd.name);
		InventoryItem myInvItem;
		if (toAdd is Equipment) {
			InventoryEquipment myEq = myEquipments.Find (x => x.item.name == toAdd.name);
			myInvItem = new InventoryEquipment ((Equipment)toAdd, amount);
			if (myEq != null) {
				myEq.chargesLeft += amount;
			} else {
				myEquipments.Add ((InventoryEquipment)myInvItem);
			}
		} else if (toAdd is Ingredient) {
			InventoryIngredient myIng = myIngredients.Find (x => x.item.name == toAdd.name);
			myInvItem = new InventoryIngredient ((Ingredient)toAdd, amount);
			if (myIng != null) {
				myIng.chargesLeft += amount;
			} else {
				myIngredients.Add ((InventoryIngredient)myInvItem);
			}
		} else {
			InventoryPotion myPot = myPotions.Find (x => x.item.name == toAdd.name);
			myInvItem = new InventoryPotion ((Potion)toAdd, amount);
			if (myPot != null) {
				myPot.chargesLeft += amount;
			} else {
				myPotions.Add ((InventoryPotion)myInvItem);
			}
		}

		if (ItemGainedScreen.s != null)
			ItemGainedScreen.s.ShowGainedItem (myInvItem);

		SaveMaster.s.Save ();
	}

	public void Remove (InventoryItem toRemove) {
		Remove (toRemove.item, 1);
	}

	public void Remove (ItemBase toRemove) {
		Remove (toRemove, 1);
	}

	public void Remove (ItemBase toRemove, int amount) {
		InventoryItem myInvItem;
		if (toRemove is Equipment) {
			InventoryEquipment myEq = myEquipments.Find (x => x.item.name == toRemove.name);
			myInvItem = new InventoryEquipment ((Equipment)toRemove, toRemove.durability);

			bool didRemove = false;
			if (myEq != null) {
				myEq.chargesLeft -= amount;
				if (myEq.chargesLeft <= 0) {
					myEquipments.Remove (myEq);
					didRemove = true;
				}
			} else {
				DataLogger.LogError ("Cant remove item! " + toRemove.name + " - " + amount.ToString());
			}

			if (didRemove) {
				if (toRemove.name == activeEquipment.item.name) {
					activeEquipment = null;
				}
			}
		} else if (toRemove is Ingredient) {
			InventoryIngredient myIng = myIngredients.Find (x => x.item.name == toRemove.name);
			myInvItem = new InventoryIngredient ((Ingredient)toRemove, toRemove.durability);
			if (myIng != null) {
				myIng.chargesLeft -= amount;
				if (myIng.chargesLeft <= 0)
					myIngredients.Remove (myIng);
			} else {
				DataLogger.LogError ("Cant remove item! " + toRemove.name + " - " + amount.ToString ());
			}
		} else {
			InventoryPotion myPot = myPotions.Find (x => x.item.name == toRemove.name);
			myInvItem = new InventoryPotion ((Potion)toRemove, toRemove.durability);
			if (myPot != null) {
				myPot.chargesLeft -= amount;
				if (myPot.chargesLeft <= 0)
					myPotions.Remove (myPot);
			} else {
				DataLogger.LogError ("Cant remove item! " + toRemove.name + " - " + amount.ToString ());
			}
		}
	}

	public int Count (ItemBase item) {
		List<InventoryMaster.InventoryItem> myItems = new List<InventoryMaster.InventoryItem> ();
		if (item is Equipment){
			foreach (InventoryMaster.InventoryEquipment eq in InventoryMaster.s.myEquipments.FindAll (s => s.item.name == item.name)) {
				myItems.Add (eq);
			}
		} else if (item is Ingredient) {
			foreach (InventoryMaster.InventoryIngredient eq in InventoryMaster.s.myIngredients.FindAll (s => s.item.name == item.name)) {
				myItems.Add (eq);
			}
		} else {
			foreach (InventoryMaster.InventoryPotion eq in InventoryMaster.s.myPotions.FindAll (s => s.item.name == item.name)) {
				myItems.Add (eq);
			}
		}
		int count = 0;
		foreach (InventoryMaster.InventoryItem it in myItems) {
			//curAmounts[i] += item.chargesLeft;
			count += it.chargesLeft;
		}
		return count;
	}

	public bool CraftItem (Recipe recipe) {
		bool canCraft = true;

		for (int i = 0; i < recipe.requiredItems.Length; i++) {
			if (Count (recipe.requiredItems[i]) < recipe.requiredAmounts[i])
				canCraft = false;
		}

		if (canCraft) {
			for (int i = 0; i < recipe.requiredItems.Length; i++) {
				for (int n = 0; n < recipe.requiredAmounts[i]; n++)
					Remove (recipe.requiredItems[i]);
			}
			
			Add (recipe.resultingItem, recipe.resultingAmount);

			return true;
		} else {
			return false;
		}
	}

	public void ClearInventory () {
		DataLogger.LogError ("Inventory Cleared");
		myEquipments = new List<InventoryEquipment> ();
		myIngredients = new List<InventoryIngredient> ();
		myPotions = new List<InventoryPotion> ();
		activeEquipment = null;

		if (SceneMaster.s.curScene == 0) {
			if(VC_CharacterMenuController.s != null)
				VC_CharacterMenuController.s.SetEquipped ();
		} else {
			CharacterStuffController.s.SetUpButtons ();
		}

		Save ();
	}

	public void Save () {
		if (activeEquipment != null)
			SaveMaster.s.mySave.activeEquipment = activeEquipment.item.name;
		else
			SaveMaster.s.mySave.activeEquipment = "none";
		myEquipments.TrimExcess ();
		SaveMaster.s.mySave.myEquipments = myEquipments.ToArray ().ConvertToSave ();
		myIngredients.TrimExcess ();
		SaveMaster.s.mySave.myIngredients = myIngredients.ToArray ().ConvertToSave ();
		myPotions.TrimExcess ();
		SaveMaster.s.mySave.myPotions = myPotions.ToArray ().ConvertToSave ();

		SaveMaster.s.mySave.selectedElement = selectedElement;
		SaveMaster.s.mySave.elementLevel = elementLevel;
	}

	[System.Serializable]
	public abstract class InventoryItem {
		public ItemBase item;
		public int chargesLeft;
	}

	[System.Serializable]
	public class InventoryPotion : InventoryItem {
		public InventoryPotion (Potion _potion, int _chargesLeft) {
			item = _potion;
			chargesLeft = _chargesLeft;
		}

		public SaveData.SavePotion ConvertToSave () {
			return new SaveData.SavePotion (item.ID, chargesLeft);
		}
	}

	[System.Serializable]
	public class InventoryEquipment : InventoryItem {

		public InventoryEquipment (Equipment _equipment, int _chargesLeft) {
			item = _equipment;
			chargesLeft = _chargesLeft;
		}

		public SaveData.SaveEquipment ConvertToSave () {
			return new SaveData.SaveEquipment (item.ID, chargesLeft);
		}
	}

	[System.Serializable]
	public class InventoryIngredient : InventoryItem {

		public InventoryIngredient (Ingredient _equipment, int _chargesLeft) {
			item = _equipment;
			chargesLeft = _chargesLeft;
		}

		public SaveData.SaveIngredient ConvertToSave () {
			return new SaveData.SaveIngredient (item.ID, chargesLeft);
		}
	}
}



public static class ArrayConverters {
	public static SaveData.SaveEquipment[] ConvertToSave (this InventoryMaster.InventoryEquipment[] InvArr) {
		SaveData.SaveEquipment[] SavArr = new SaveData.SaveEquipment[InvArr.Length];

		for(int i = 0; i < InvArr.Length; i++) {
			SavArr[i] = InvArr[i].ConvertToSave ();
		}

		return SavArr;
	}

	public static SaveData.SaveIngredient[] ConvertToSave (this InventoryMaster.InventoryIngredient[] InvArr) {
		SaveData.SaveIngredient[] SavArr = new SaveData.SaveIngredient[InvArr.Length];

		for (int i = 0; i < InvArr.Length; i++) {
			SavArr[i] = InvArr[i].ConvertToSave ();
		}

		return SavArr;
	}

	public static SaveData.SavePotion[] ConvertToSave (this InventoryMaster.InventoryPotion[] InvArr) {
		SaveData.SavePotion[] SavArr = new SaveData.SavePotion[InvArr.Length];

		for (int i = 0; i < InvArr.Length; i++) {
			SavArr[i] = InvArr[i].ConvertToSave ();
		}

		return SavArr;
	}

	public static InventoryMaster.InventoryEquipment[]  ConvertToInventory (this SaveData.SaveEquipment[] SavArr) {
		InventoryMaster.InventoryEquipment[] InvArr = new InventoryMaster.InventoryEquipment[SavArr.Length];

		for (int i = 0; i < SavArr.Length; i++) {
			InvArr[i] = SavArr[i].ConvertToInventory ();
		}

		return InvArr;
	}

	public static InventoryMaster.InventoryIngredient[] ConvertToInventory (this SaveData.SaveIngredient[] SavArr) {
		InventoryMaster.InventoryIngredient[] InvArr = new InventoryMaster.InventoryIngredient[SavArr.Length];

		for (int i = 0; i < SavArr.Length; i++) {
			InvArr[i] = SavArr[i].ConvertToInventory ();
		}

		return InvArr;
	}

	public static InventoryMaster.InventoryPotion[] ConvertToInventory (this SaveData.SavePotion[] SavArr) {
		InventoryMaster.InventoryPotion[] InvArr = new InventoryMaster.InventoryPotion[SavArr.Length];

		for (int i = 0; i < SavArr.Length; i++) {
			InvArr[i] = SavArr[i].ConvertToInventory ();
		}

		return InvArr;
	}


	public static InventoryMaster.InventoryPotion[] ConvertToInventory (this Potion[] SavArr) {
		InventoryMaster.InventoryPotion[] InvArr = new InventoryMaster.InventoryPotion[SavArr.Length];

		for (int i = 0; i < SavArr.Length; i++) {
			InvArr[i] = new InventoryMaster.InventoryPotion (SavArr[i], 1);
		}

		return InvArr;
	}
}
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

		if (SaveMaster.s.mySave.activeEquipment != null)
			activeEquipment = SaveMaster.s.mySave.activeEquipment.ConvertToInventory ();
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
					myEquipments.Add (new InventoryEquipment (equipment, equipment.durability));
			}
			foreach (Ingredient ingredient in allIngredients) {
				if (ingredient != null) {
					for (int i = 0; i < 5; i++)
						myIngredients.Add (new InventoryIngredient (ingredient, ingredient.durability));
				}
			}
			foreach (Potion potion in allPotions) {
				if (potion != null)
					myPotions.Add (new InventoryPotion (potion, potion.durability));
			}
		}

		/*if (SceneMaster.s.curScene == 0) {
			MenuSwitchController.s.GetComponent<CharacterMenuController> ().SetEquipped ();
			ElementSelector.s.UpdateElementSliders ();
		}*/
	}

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
	
	public void Add (ItemBase toAdd) {
		DataLogger.LogMessage ("Item added: " + toAdd.name);
		if (toAdd is Equipment) {
			myEquipments.Add (new InventoryEquipment ((Equipment)toAdd, toAdd.durability));
		} else if (toAdd is Ingredient) {
			myIngredients.Add (new InventoryIngredient ((Ingredient)toAdd, toAdd.durability));
		} else {
			myPotions.Add (new InventoryPotion ((Potion)toAdd, toAdd.durability));
		}
		if (ItemGainedScreen.s != null)
			ItemGainedScreen.s.ShowGainedItem (toAdd);

		
		SaveMaster.s.Save ();
	}

	public bool Remove (InventoryItem toRemove) {
		bool result = false;
		/*if (toRemove.chargesLeft > 1) {
			toRemove.chargesLeft -= 1;
			result = true;
		} else {*/
			if (toRemove is InventoryEquipment) {
				result = myEquipments.Remove ((InventoryEquipment)toRemove);
			} else if (toRemove is InventoryIngredient) {
				result = myIngredients.Remove ((InventoryIngredient)toRemove);
			} else{
				result = myPotions.Remove ((InventoryPotion)toRemove);
			}
		//}
		SaveMaster.s.Save ();
		return result;
	}

	public bool Remove (ItemBase toRemove) {
		if (toRemove is Equipment) {
			return myEquipments.Remove (myEquipments.Find (s => s.item.name == toRemove.name));
		} else if (toRemove is Ingredient) {
			return myIngredients.Remove (myIngredients.Find (s => s.item.name == toRemove.name));
		} else {
			return myPotions.Remove (myPotions.Find (s => s.item.name == toRemove.name));
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
			count += 1;
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

			for (int i = 0; i < recipe.resultingAmount; i++)
				Add (recipe.resultingItem);

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
			SaveMaster.s.mySave.activeEquipment = activeEquipment.ConvertToSave ();
		else
			SaveMaster.s.mySave.activeEquipment = new SaveData.SaveEquipment (-1, -1);
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
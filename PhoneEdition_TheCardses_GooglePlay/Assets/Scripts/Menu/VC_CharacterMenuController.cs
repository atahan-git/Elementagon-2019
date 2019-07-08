using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VC_CharacterMenuController : ViewController {

	public static VC_CharacterMenuController s;

	public GameObject itemList;
	public Text itemListName;
	public GameObject gridItemParent;
	public GameObject gridItem;

	public Image activeItemIcon;
	public TextMeshProUGUI activeItemName;

	/*public Image activePowerIcon;
	public TextMeshProUGUI activePowerName;*/

	public Sprite noEquipmentSprite;

	public VC_CharacterMenuController () {
		s = this;
	}

	private void OnDestroy () {
		s = null;
	}


	// Use this for initialization
	void Start () {
		itemList.SetActive (false);
		Invoke ("SetEquipped", 0.1f);
	}

	public void SetEquipped () {
		try {
			if (InventoryMaster.s.activeEquipment != null) {
				activeItemIcon.sprite = InventoryMaster.s.activeEquipment.item.sprite;
				activeItemName.text = InventoryMaster.s.activeEquipment.item.name;
			} else {
				activeItemIcon.sprite = noEquipmentSprite;
				activeItemName.text = "Not Equipped";
			}

			/*if (InventoryMaster.s.elementLevel >= 0) {
				activePowerIcon.sprite = GS.a.gfxs.cardSprites[InventoryMaster.s.selectedElement + 1 + 7];
				activePowerName.text = "Power";
			} else {
				activePowerIcon.sprite = noEquipmentSprite;
				activePowerName.text = "Not Equipped";
			}*/
		} catch (System.Exception e) {
			DataLogger.LogError (this.name, e);
		}

		craftingPanel.SetActive (false);
		itemList.SetActive (false);
	}

	public void OpenItemList (int type) {
		Clear ();

		switch(type) {
		case 0:
			foreach (InventoryMaster.InventoryEquipment myItem in InventoryMaster.s.myEquipments ) {
				if (myItem != null)
					(Instantiate (gridItem, gridItemParent.transform)).GetComponent<ItemGridDisplay> ().SetUp (myItem);
			}
			itemListName.text = "Equipments";
			break;
		case 1:
			foreach (InventoryMaster.InventoryIngredient myItem in InventoryMaster.s.myIngredients) {
				if (myItem != null)
					(Instantiate (gridItem, gridItemParent.transform)).GetComponent<ItemGridDisplay> ().SetUp (myItem);
			}
			itemListName.text = "Ingredients";
			break;
		case 2:
			foreach (InventoryMaster.InventoryPotion myItem in InventoryMaster.s.myPotions) {
				if (myItem != null)
					(Instantiate (gridItem, gridItemParent.transform)).GetComponent<ItemGridDisplay> ().SetUp (myItem);
			}
			itemListName.text = "Potions";
			break;
			
		}

		itemList.SetActive (true);
	}

	public void Clear () {
		int childCount = gridItemParent.transform.childCount;

		for (int i = childCount - 1; i >= 0; i--) {
			Destroy (gridItemParent.transform.GetChild (i).gameObject);
		}
	}

	public void ItemListBack () {
		itemList.SetActive (false);
	}

	//-------------------------------Crafting
	public GameObject craftingPanel;
	public GameObject craftingInfoPrefab;
	public Transform craftingInfoParent;
	public void OpenCraftingScreen () {
		int childCount = craftingInfoParent.childCount;
		for (int i = childCount - 1; i >= 0; i--) {
			Destroy (craftingInfoParent.GetChild (i).gameObject);
		}

		for (int id = 0; id < InventoryMaster.s.allRecipes.Length; id++) {
			Recipe myRec = InventoryMaster.s.allRecipes[id];
			int[] curAmounts = new int[myRec.requiredItems.Length];
			for (int i = 0; i < myRec.requiredItems.Length; i++) {
				curAmounts[i] = InventoryMaster.s.Count (myRec.requiredItems[i]);
			}

			Sprite[] reqSprites = new Sprite[myRec.requiredItems.Length];
			for (int i = 0; i < reqSprites.Length; i++) {
				reqSprites[i] = myRec.requiredItems[i].sprite;
			}

			GameObject myCraftInfo = Instantiate (craftingInfoPrefab, craftingInfoParent);
			myCraftInfo.GetComponent<ItemInfoDisplay> ().SetUp (
				myRec.resultingItem.sprite, myRec.resultingItem.name, myRec.resultingItem.description, myRec.resultingAmount,
				curAmounts, myRec.requiredAmounts, reqSprites
				);

			int temp = id;
			myCraftInfo.GetComponentInChildren<Button> ().onClick.AddListener (() => CraftItem (temp));
		}

		craftingPanel.SetActive (true);
	}

	public void CraftingScreenBack () {
		craftingPanel.SetActive (false);
	}

	public void CraftItem (int buttonId) {
		print (buttonId);
		if (InventoryMaster.s.CraftItem (InventoryMaster.s.allRecipes[buttonId])) {
			OpenCraftingScreen ();
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CustomCharacterStartScreen : MonoBehaviour {
	public static CustomCharacterStartScreen s;

	public GameObject screenPanel;

	[Space]
	public Image charPortrait;
	public TextMeshProUGUI charName;
	public TextMeshProUGUI charDescription;

	[Space]
	public GameObject powerOverride;
	public Image powerSprite;
	public TextMeshProUGUI powerName;
	public TextMeshProUGUI powerLevel;

	[Space]
	public GameObject potionOverride;
	public GameObject itemGridParent;
	public GameObject itemGridDisplayPrefab;

	[Space]
	public GameObject equipmentOverride;
	public ItemInfoDisplay equipmentDisplay;

	// Start is called before the first frame update
	void Start () {
		s = this;
		screenPanel.SetActive (false);

		if (GS.a.customCharacterLevel) {
			if (GS.a.charSprite != null)
				charPortrait.sprite = GS.a.charSprite;
			charName.text = GS.a.charName;
			charDescription.text = GS.a.charDescription;


			powerOverride.SetActive (true);
			if (GS.a.elementLevel > 0) {
				powerSprite.sprite = GS.a.gfxs.cardSprites[GS.a.selectedElement + 1 + 7];
				powerName.text = ElementTypeToDragonType (GS.a.selectedElement);
				powerLevel.text = "Level: " + GS.a.elementLevel;
			} else {
				powerSprite.sprite = CharacterStuffController.s.noEquipmentSprite;
				powerName.text = "No Power";
				powerLevel.text = "";
			}


			potionOverride.SetActive (true);
			for (int i = 0; i < GS.a.potions.Length; i++) {
				int amount = 1;
				if (i < GS.a.potionsAmounts.Length)
					amount = GS.a.potionsAmounts[i] > 0 ?  GS.a.potionsAmounts[i] : amount;
				else {
					int[] temp = new int[GS.a.potions.Length];
					GS.a.potionsAmounts.CopyTo (temp, 0);
					GS.a.potionsAmounts = temp;
				}
				if (amount > 0)
					Instantiate (itemGridDisplayPrefab, itemGridParent.transform).GetComponent<ItemGridDisplay> ().SetUp (new InventoryMaster.InventoryPotion (GS.a.potions[i], amount));
			}


			equipmentOverride.SetActive (true);
			equipmentDisplay.SetUp (new InventoryMaster.InventoryEquipment (GS.a.equipment, 1));
		}
	}

	public void EnableScreen () {
		screenPanel.SetActive (true);
	}

	public void DisableScreen (){
		screenPanel.SetActive (false);
		GameStarter.s.CustomCharacterStatsShowEnd ();
	}

	string ElementTypeToDragonType (int elementType) {
		switch (elementType) {
		case 0:
			return "Earth";
		case 1:
			return "Fire";
		case 2:
			return "Ice";
		case 3:
			return "Light";
		case 4:
			return "Nether";
		case 5:
			return "Poison";
		case 6:
			return "Shadow";
		default:
			return "Power";
		}
	}

}

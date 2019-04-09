using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailsScreen : MonoBehaviour {

	public static ItemDetailsScreen s;

	public GameObject panel;

	public Image sprite;
	public ItemInfoDisplay tooltip;
	public Text itemName;

	public InventoryMaster.InventoryItem myItem;

	public GameObject equipButton;

	private void Start () {
		s = this;
		tooltip.shouldAnimate = false;
		Hide ();
	}

	public void Show (InventoryMaster.InventoryItem item) {
		myItem = item;
		itemName.text = myItem.item.name;
		sprite.sprite = myItem.item.sprite;
		if (myItem.item is Equipment) {
			tooltip.SetUp (myItem.item.description, ((Equipment)myItem.item).chargeReq);
			equipButton.SetActive (true);
		} else {
			tooltip.SetUp (myItem.item.description);
			equipButton.SetActive (false);
		}

		panel.SetActive (true);
	}

	public void Equip () {
		if (myItem is InventoryMaster.InventoryEquipment) {
			InventoryMaster.s.EquipItem ((InventoryMaster.InventoryEquipment)myItem);
			Hide ();
		}
	}

	public void Hide () {
		panel.SetActive (false);
	}
}

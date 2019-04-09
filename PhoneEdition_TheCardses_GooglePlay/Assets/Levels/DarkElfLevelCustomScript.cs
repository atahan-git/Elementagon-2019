using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkElfLevelCustomScript : MonoBehaviour {


	// Use this for initialization
	void Start () {
		if (InventoryMaster.s.myEquipments.Count > 0) {
			return;
		}

		
		//ItemGainedScreen.s.itemGainedCall += ItemGained;
		ItemGainedScreen.s.itemGainedScreenClosedCall += EquipItemGained;

		Invoke ("LateStart", 1f);
		Invoke ("NotFinishedInTime", 120f);
	}

	void LateStart () {
		List<IndividualCard> myCards = CardHandler.s.GetRandomizedSelectabeCardList ();
		myCards[0].cBase = Instantiate (GS.a.defCard);
		myCards[0].cBase.cardType = GS.a.itemTypesStartIndex;
		myCards[1].cBase = myCards[1].cBase;
	}

	/*bool isSelfInvoke = false;
	public void ItemGained () {
		isSelfInvoke = true;
		ItemGainedScreen.s.Ok ();

		DialogTree.s.callWhenDone.AddListener (ItemGainedDialogEnded);
	}

	public void ItemGainedDialogEnded () {
		ItemGainedScreen.s.ShowGainedItem (InventoryMaster.s.myEquipments[0].item);
	}*/

	public void EquipItemGained () {
		/*if (isSelfInvoke) {
			isSelfInvoke = false;
			return;
		}*/
		InventoryMaster.s.EquipItem(InventoryMaster.s.myEquipments[0]);
	}

	public DialogTreeAsset notFinishedinTimeDialog;

	public void NotFinishedInTime () {
		LocalPlayerController.isActive = false;
		DialogTree.s.LoadFromAsset (notFinishedinTimeDialog);
		GameObjectiveFinishChecker.s.isGamePlaying = false;

		DialogTree.s.myCustomTriggers[0] += FireBall;
		DialogTree.s.myCustomTriggers[1] += Finish;
		DialogTree.s.StartDialog ();
	}

	public void FireBall () {
		foreach (PowerUpBase p in PowerUpManager.s.equipmentPUps) {
			if (p is PowerUp_MatchArea) {
				p.power = 4;
				p.amount = 1;
				p.Activate (GameObjectiveFinishChecker.s.ActiveNPCS[0].myCurrentOccupation);
			}
		}
		
	}

	public void Finish () {
		GameObjectiveFinishChecker.s.EndGame (0);
	}
}

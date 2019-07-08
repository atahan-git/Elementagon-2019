using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDurabilityLossScreen : MonoBehaviour {

	public static ItemDurabilityLossScreen s;

	public GameObject myPanel;

	public GameObject myDetailsPrefab;
	GameObject myDetailsObject;
	public TextMeshProUGUI beforeAmount;
	public GameObject itemExplodedSprite;

	int openScreensCount = 0;

	private void Awake () {
		s = this;
	}

	// Use this for initialization
	void Start () {
		myPanel.SetActive (false);
	}

	public delegate void myDelegate ();
	public myDelegate itemGainedCall;
	public void ShowDurabilityLoss (InventoryMaster.InventoryItem item) {

		beforeAmount.text = (item.chargesLeft + 1).ToString () + "x";
		itemExplodedSprite.SetActive (item.chargesLeft == 0);
		myDetailsObject = Instantiate (myDetailsPrefab, myPanel.transform);
		myDetailsObject.GetComponentInChildren<ItemInfoDisplay> ().SetUp (item);
		myDetailsObject.SetActive (true);
		openScreensCount++;


		myPanel.SetActive (true);


		if (itemGainedCall != null)
			itemGainedCall.Invoke ();

		if (LocalPlayerController.s != null) {
			LocalPlayerController.s.canSelect = false;
			playerFlag = true;
		}
		/*if (GameObjectiveFinishChecker.s != null && GS.a.myGameType == GameSettings.GameType.Singleplayer) {
			GameObjectiveFinishChecker.s.isGamePlaying = false;
			gameFlag = true;
		}*/
	}
	bool playerFlag = false;
	//bool gameFlag = false;

	public myDelegate itemGainedScreenClosedCall;
	public void Ok () {
		myDetailsObject = myPanel.transform.GetChild (myPanel.transform.childCount - 1).gameObject;
		Destroy (myDetailsObject);
		openScreensCount--;

		if (openScreensCount <= 0) {
			myPanel.SetActive (false);
			if (itemGainedScreenClosedCall != null)
				itemGainedScreenClosedCall.Invoke ();

			if (LocalPlayerController.s != null && playerFlag) {
				LocalPlayerController.s.canSelect = true;
				playerFlag = false;
			}
		}
		/*if (GameObjectiveFinishChecker.s != null && gameFlag) {
			GameObjectiveFinishChecker.s.isGamePlaying = true;
			gameFlag = false;
		}*/
	}
}

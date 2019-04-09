using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemGridDisplay : MonoBehaviour {

    public InventoryMaster.InventoryItem myItem;

    public static bool isShowingTooltip;

	public GameObject tooltipPrefab;
    GameObject toolTip;
    public Vector3 offset;

    public Image myIcon;
	public TextMeshProUGUI myName;

    // Use this for initialization
    public void SetUp (InventoryMaster.InventoryItem _item) {
		myItem = _item;

		if (toolTip != null)
			Destroy (toolTip);

        toolTip = Instantiate (tooltipPrefab, GetComponentInParent<CanvasScaler> ().transform);
        toolTip.GetComponentInChildren<TextMeshProUGUI> ().text = myItem.item.description;
        toolTip.SetActive (false);
        myIcon.sprite = myItem.item.sprite;

		myName.text = _item.item.name;
	}


	private void OnDestroy () {
		if (toolTip != null)
			Destroy (toolTip);
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButton (0) && isPointerInside) {
			if (!isShowingTooltip)
				ShowTooltip ();

			toolTip.transform.position = Input.mousePosition + offset;
		} else {
			if (isShowingTooltip) {
				isShowingTooltip = false;
				toolTip.SetActive (false);
			}
		}
    }

	bool isPointerInside = false;
    public void PointerEnter () {
		//print ("pointer enter");
		isPointerInside = true;
	}

	public void PointerExit () {
		//print ("pointer exit");
		isPointerInside = false;
	}

	void ShowTooltip () {
        //print ("show tooltip");
        isShowingTooltip = true;
        toolTip.SetActive (true);
    }

	public void Click () {
		if (SceneMaster.s.curScene == 0) {
			ItemDetailsScreen.s.Show (myItem);
		} else {
			CharacterStuffController.s.ActivatePotion (myItem);
		}
	}
}

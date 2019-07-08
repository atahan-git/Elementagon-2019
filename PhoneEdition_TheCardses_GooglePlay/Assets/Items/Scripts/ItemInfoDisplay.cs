using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfoDisplay : MonoBehaviour {

	public bool shouldAnimate = true;

    public float enableTime = 0.1f;

	[Space]
	public Image icon;
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI descriptionText;
	public TextMeshProUGUI amountLeftText;

	[Space]
	public Transform reqsParent;
	public GameObject reqsObj;

	public void Clear () {
		int childCount = reqsParent.childCount;

		for (int i = childCount -1; i >= 0; i--) {
			Destroy (reqsParent.GetChild(i).gameObject);
		}
	}

	public void SetUp (Sprite _icon, string _name, string _description, int amountLeft, int[] amounts,int[]reqs, Sprite[] reqsSprites) {
		if (icon != null)
			icon.sprite = _icon;

		if (nameText != null)
			nameText.text = _name;

		if (descriptionText != null)
			descriptionText.text = _description;

		if (amountLeftText != null) {
			amountLeftText.text = amountLeft.ToString () + "x";
		}

		Clear ();
		for (int i = 0; i < reqs.Length; i++) {
			if (reqs[i] > 0) {
				((GameObject)Instantiate (reqsObj, reqsParent)).GetComponent<ItemRequirement> ().SetUp (reqsSprites[i], amounts[i], reqs[i]);
			}
		}
	}

	public void SetUp (int[] reqs) {
		string myDescription = "";
		if (descriptionText != null)
			myDescription = descriptionText.text;
		SetUp (myDescription,reqs);
	}

	public void SetUp (string description, int[] reqs) {
		Clear ();
		for (int i = 0; i < reqs.Length; i++) {
			if (reqs[i] > 0) {
				((GameObject)Instantiate (reqsObj, reqsParent)).GetComponent<ItemRequirement> ().SetUp (i, reqs[i]);
			}
		}
		if(descriptionText != null)
			descriptionText.text = description;
	}

	public void SetUp (string description) {
		Clear ();
		if (descriptionText != null)
			descriptionText.text = description;
	}

	public void SetUp (InventoryMaster.InventoryItem item) {
		SetUp (item.item.sprite, item.item.name, item.item.description, item.chargesLeft, new int[0], new int[0], new Sprite[0]);
	}

	private void OnEnable () {
		if (shouldAnimate) {
			transform.localScale = new Vector3 (1, 0, 1);
			StartCoroutine (ChangeScale ());
		} else {
			transform.localScale = new Vector3 (1, 1, 1);
		}
    }

    IEnumerator ChangeScale () {

        while (transform.localScale.y < 1f) {
            transform.localScale = new Vector3 (1, Mathf.MoveTowards (transform.localScale.y, 1f, (1f / enableTime) * Time.deltaTime), 1);
            yield return null;
        }
		
        yield return null;
    }

    private void OnDisable () {
		if (shouldAnimate) {
			StopAllCoroutines ();
		}
    }
}

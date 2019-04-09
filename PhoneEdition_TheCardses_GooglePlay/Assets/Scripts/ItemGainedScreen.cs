using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemGainedScreen : MonoBehaviour {

	public static ItemGainedScreen s;

	public GameObject myPanel;

	public GameObject myDetailsPrefab;
	GameObject myDetailsObject;
	public Image myImg;
	public Text myName;
	public Text myDesc;

	private void Awake () {
		s = this;
	}

	// Use this for initialization
	void Start () {
		myPanel.SetActive (false);
	}

	public delegate void myDelegate ();
	public myDelegate itemGainedCall;
	public void ShowGainedItem (ItemBase item) {

		myImg.sprite = item.sprite;
		myName.text = item.name;
		myDesc.text = item.description;

		myDetailsObject = Instantiate (myDetailsPrefab, myPanel.transform);
		myDetailsObject.SetActive (true);


		myPanel.SetActive (true);


		if (itemGainedCall != null)
			itemGainedCall.Invoke ();
	}


	public myDelegate itemGainedScreenClosedCall;
	public void Ok () {
		myPanel.SetActive (false);
		Destroy(myDetailsObject);

		if (itemGainedScreenClosedCall != null)
			itemGainedScreenClosedCall.Invoke ();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour {

	[Header("View Controller")]

	public ViewController[] children;

	//[HideInInspector]
	public int[] myId;

	public bool isShown = false;

	public bool openChildAsDefault;

	public void Show () {
		gameObject.SetActive (true);
		isShown = true;
	}

	public void Hide () {
		gameObject.SetActive (false);
		isShown = false;
	}

	public virtual void TransitionShow () {
		if (!isShown)
			Show ();
	}

	public virtual void TransitionHide () {
		if (isShown)
			Hide ();
	}
}

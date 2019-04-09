using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialText : MonoBehaviour {

	public static TutorialText s;

	Text tx;
	public Image myImg;
	public GameObject tapToContObj;
	// Use this for initialization
	void Awake () {
		s = this;
		tx = GetComponent<Text> ();
		Pause (false);
	}
	
	public void SetText (string text){
		tx.text = text;
	}

	void Update (){
		/*if (Input.GetMouseButtonDown (0)) {
			tapToContObj.SetActive (false);
		}*/
	}

	public void Pause (bool isPause){
		if (isPause) {
			myImg.color = new Color (1, 1, 1, 0.8f);
		} else {
			myImg.color = new Color (1, 1, 1, 0);
		}
	}

	public void TapToCont (bool isOn) {
		tapToContObj.SetActive (isOn);
	}
}

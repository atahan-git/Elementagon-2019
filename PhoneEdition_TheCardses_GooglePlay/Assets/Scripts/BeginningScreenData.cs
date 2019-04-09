using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginningScreenData : MonoBehaviour {

	[TextArea]
	public string text;
	public Sprite image;

	// Use this for initialization
	void Awake () {
		Text txt = GameObject.Find ("Daragon Text").GetComponent<Text> ();
		Image img = GameObject.Find ("Daragon Image").GetComponent<Image> ();

		txt.text = text;
		img.sprite = image;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_FirstTimeStuff : MonoBehaviour {

	public static Tutorial_FirstTimeStuff s;

	public GameObject myPanel;
	public Text myText;


	bool isCombo = false;
	bool isNoPointCard = false;

	// Use this for initialization
	void Start () {
		s = this;
		myPanel.SetActive (false);

		isCombo = PlayerPrefs.GetInt ("isCombo", 0) == 1;
		isNoPointCard = PlayerPrefs.GetInt ("isNoPointCard", 0) == 1;
	}

	public static void ComboCheck () {
		if (s != null)
			s.StartCoroutine (s._ComboCheck ());
	}

	IEnumerator _ComboCheck () {
		if (!isCombo) {
			isCombo = true;
			PlayerPrefs.SetInt ("isCombo", 1);

			LocalPlayerController.isActive = false;
			GameObjectiveMaster.s.isGamePlaying = false;

			myPanel.SetActive (true);

			myText.text = "Kombo Yaptın! Eğer sürekli kart eşleştirirsen kombo miktarını arttırabilirsin. Puansız kartları açmak kombonu bozmaz.";

			yield return new WaitUntil (() => Input.GetMouseButtonDown(0));

			LocalPlayerController.isActive = true;
			GameObjectiveMaster.s.isGamePlaying = true;

			myPanel.SetActive (false);
		}
	}


	public static void NoPointCards () {
		if (s != null)
			s.StartCoroutine(s._NoPointCards ());
	}

	IEnumerator _NoPointCards () {
		if (!isNoPointCard) {
			isNoPointCard = true;
			PlayerPrefs.SetInt ("isNoPointCard", 1);

			LocalPlayerController.isActive = false;
			GameObjectiveMaster.s.isGamePlaying = false;

			myPanel.SetActive (true);

			myText.text = "Bazı kartlar eşleştirince puan vermezler. Ama yine de puan veren kartları bulmak için onları eşleştirebilirsin.";

			yield return new WaitUntil (() => Input.GetMouseButtonDown (0));

			LocalPlayerController.isActive = true;
			GameObjectiveMaster.s.isGamePlaying = true;

			myPanel.SetActive (false);
		}
	}
}

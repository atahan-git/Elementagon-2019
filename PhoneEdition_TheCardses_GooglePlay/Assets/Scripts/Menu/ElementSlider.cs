using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementSlider : MonoBehaviour {

	public int myID = -1;

	public int myValue = 0;

	Slider mySlider;

	public Color defColor = Color.white;
	public Color selectedColor = Color.green;

	public Image myImg;

	private void Start () {
		mySlider = GetComponentInChildren<Slider> ();
		UpdateColor ();
	}

	public void SetUp (int maxValue, int id){
		mySlider = GetComponentInChildren<Slider>();
		myID = id;
		mySlider.GetComponent<RectTransform> ().sizeDelta = new Vector2 (
			mySlider.GetComponent<RectTransform> ().sizeDelta.x*(SaveMaster.s.mySave.unlockedElementLevels[id]/ 4f), 
			mySlider.GetComponent<RectTransform> ().sizeDelta.y);
		mySlider.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (
			mySlider.GetComponent<RectTransform> ().anchoredPosition.x * (SaveMaster.s.mySave.unlockedElementLevels[id] / 4f),
			mySlider.GetComponent<RectTransform> ().anchoredPosition.y);
		mySlider.maxValue = maxValue;

		myImg.sprite = GS.a.gfxs.cardSprites[id + 1 + 7];
	}

	bool dontTriggerNextChange = false;
	public void SetValue (int value) {
		if (mySlider == null) {
			mySlider = GetComponentInChildren<Slider> ();
		}

		if (value != (int)mySlider.value) {
			dontTriggerNextChange = true;
			
			myValue = value;
			mySlider.value = value;
			UpdateColor ();
		}
	}

	public void ValueChanged () {
		if (dontTriggerNextChange) {
			dontTriggerNextChange = false;
			return;
		}
		//print ("Value changed " + myID.ToString());
		myValue = (int)mySlider.value;
		ElementSelector.s.ValueChanged (myID, myValue);
		UpdateColor ();
	}

	void UpdateColor () {
		if (myValue > 0) {
			GetComponent<Image> ().color = selectedColor;
		} else {
			GetComponent<Image> ().color = defColor;
		}
	}
}

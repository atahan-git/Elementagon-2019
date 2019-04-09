using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSelector : MonoBehaviour {

	public static ElementSelector s;

	ElementSlider [] mySliders = new ElementSlider [7];

	public Transform sliderParent;

	public GameObject sliderPrefab;

	bool valLock = false;

	// Use this for initialization
	void Awake () {
		s = this;

		for (int i = 0; i < 7; i++) {
			if (SaveMaster.s.mySave.unlockedElementLevels[i] > 0) {
				mySliders[i] = Instantiate (sliderPrefab, sliderParent).GetComponent<ElementSlider>();
				mySliders[i].SetUp (SaveMaster.s.mySave.unlockedElementLevels[i], i);
			}
		}

		Invoke ("UpdateElementSliders", 0.1f);
	}

	public void UpdateElementSliders () {
		for (int i = 0; i < 7; i++) {
			if (i == InventoryMaster.s.selectedElement) {
				if (mySliders[i] != null) {
					mySliders[i].SetValue (InventoryMaster.s.elementLevel);
				} else {
					InventoryMaster.s.selectedElement = -1;
				}
			} else {
				if (mySliders[i] != null) {
					mySliders[i].SetValue (0);
				}
			}
		}
	}


	public void ValueChanged (int id, int level) {
		for (int i = 0; i < 7; i++) {
			if (i != id) {
				if(mySliders[i] != null)
				mySliders[i].SetValue (0);
			}
		}

		InventoryMaster.s.elementLevel = level;
		InventoryMaster.s.selectedElement = id;
		if (VC_CharacterMenuController.s != null)
			VC_CharacterMenuController.s.SetEquipped ();
	}
}

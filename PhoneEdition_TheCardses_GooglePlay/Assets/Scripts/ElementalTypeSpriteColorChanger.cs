using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalTypeSpriteColorChanger : MonoBehaviour {

	public SpriteRenderer[] myRends;
	public ParticleSystem[] myParts;

	public void ChangeColor (Color myColor) {
		if (myColor.a <= 0.5f)
			myColor = PowerUpManager.s.dragonColors[0];

		foreach (SpriteRenderer ren in myRends) {
			if (ren != null) {
				ren.color = myColor;
			}
		}

		foreach (ParticleSystem sys in myParts) {
			if (sys != null) {
				sys.startColor = myColor;
			}
		}
	}
}

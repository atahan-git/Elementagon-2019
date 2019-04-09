using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalTypeSpriteColorChanger : MonoBehaviour {

	public SpriteRenderer[] myRends;
	public ParticleSystem[] myParts;

	public void ChangeColor (int elementalType) {
		foreach (SpriteRenderer ren in myRends) {
			if (ren != null) {
				ren.color = PowerUpManager.s.genericColors[elementalType];
			}
		}

		foreach (ParticleSystem sys in myParts) {
			if (sys != null) {
				sys.startColor = PowerUpManager.s.genericColors[elementalType];
			}
		}
	}
}

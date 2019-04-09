using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemRequirement : MonoBehaviour {

	Vector2 animSize = new Vector2 (20,20);
	Vector2 spriteSize = new Vector2 (11.87f, 17.511f);
	Vector2 imgSize = new Vector2 (13f, 13f);

	public void SetUp (int type, int req) {
		GetComponentInChildren<TextMeshProUGUI> ().text = "x" + req.ToString ();
		if (GS.a.gfxs.cardAnimations[type] != null && !GS.a.gfxs.isSpriteBased) {
			GetComponentInChildren<AnimatedSpriteController> ().enabled = true;
			GetComponentInChildren<AnimatedSpriteController> ().SetAnimation (GS.a.gfxs.cardAnimations[type]);
			GetComponentInChildren<AnimatedSpriteController> ().gameObject.GetComponent<Image> ().GetComponent<RectTransform> ().sizeDelta = animSize;
		} else {
			GetComponentInChildren<AnimatedSpriteController> ().enabled = true;
			GetComponentInChildren<AnimatedSpriteController> ().SetSprite (GS.a.gfxs.cardSprites[type]);
			GetComponentInChildren<AnimatedSpriteController> ().gameObject.GetComponent<Image> ().GetComponent<RectTransform> ().sizeDelta = spriteSize;
		}
	}


	public void SetUp (Sprite img, int amount, int req) {
		GetComponentInChildren<TextMeshProUGUI> ().text = amount.ToString() + "/" + req.ToString ();

		GetComponentInChildren<AnimatedSpriteController> ().enabled = true;
		GetComponentInChildren<AnimatedSpriteController> ().SetSprite (img);
		GetComponentInChildren<AnimatedSpriteController> ().gameObject.GetComponent<Image> ().GetComponent<RectTransform> ().sizeDelta = imgSize;
	}
}

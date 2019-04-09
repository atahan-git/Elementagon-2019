using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteFade : MonoBehaviour {

	public bool fadeAtStart = true;
	public bool startFadein = true;
	public float fadeInTime = 0.2f;
	public float fadeOutTime = 0.4f;
	float fadeSpeed;

	float targetAlpha;
	SpriteRenderer myRend;
	Image myImg;
	
	// Use this for initialization
	void Start () {
		myRend = GetComponent<SpriteRenderer> ();
		myImg = GetComponent<Image> ();

		if (myRend == null && myImg == null) {
			this.enabled = false;
			return;
		}
		if (fadeAtStart) {
			if (startFadein) {
				targetAlpha = GetColor().a;

				SetColor(new Color (GetColor ().r, GetColor().g, GetColor().b, 0));
			} else {
				targetAlpha = 0f;

				SetColor (new Color (GetColor ().r, GetColor().g, GetColor().b, 1));
			}
			fadeSpeed = Mathf.Abs (targetAlpha - GetColor ().a) / fadeInTime;
			StartCoroutine (Fade ());
		}
	}

	IEnumerator Fade () {
		while (Mathf.Abs (targetAlpha - GetColor().a) > 0) {
				SetColor (new Color (GetColor ().r, GetColor ().g, GetColor ().b, Mathf.MoveTowards (GetColor ().a, targetAlpha, fadeSpeed * Time.deltaTime)));
			yield return null;
		}

		SetColor (new Color (GetColor ().r, GetColor ().g, GetColor ().b, targetAlpha));

		yield return null;
	}


	public void FadeOut () {
		targetAlpha = 0;
		StopAllCoroutines ();

		fadeSpeed = Mathf.Abs (targetAlpha - GetColor().a) / fadeOutTime;
		StartCoroutine (Fade ());
	}

	public void SetColor (Color myColor) {
		if (myRend)
			myRend.color = myColor;
		else
			myImg.color = myColor;
	}

	public Color GetColor () {
		if (myRend)
			return myRend.color;
		else
			return myImg.color;
	}
}

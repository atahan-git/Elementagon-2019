using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAnimator : MonoBehaviour {

    public bool isOpen = false;
    public bool isMatched = false;
    public bool justRotate = false;

	public bool isRevealed {
		get {
			return _isRevealed;
		}
		set {
			_isRevealed = value;
			UpdateRevealState ();
		}
	}
	public bool _isRevealed;


	Quaternion openRotation = Quaternion.Euler (0, 0, 0);
	Quaternion closeRotation = Quaternion.Euler (0, 180, 0);

	float openTime = 10f/60f;

	AudioPlayer aud;
	IndividualCard card;

	public SpriteRenderer cardBack;
	public SpriteRenderer front;

	void Start () {
		aud = GetComponent<AudioPlayer> ();
		card = GetComponent<IndividualCard> ();
		transform.rotation = isOpen ? openRotation : closeRotation;
	}

	public void SetOpenState (bool state) {
		aud = GetComponent<AudioPlayer> ();
		card = GetComponent<IndividualCard> ();
		isOpen = state;

		if (!isMatched) {
			InstantSetAlphaStates ();
			StopAllCoroutines ();
			if (isOpen) {
				aud.OpenSound ();
				StartCoroutine (RotateTo (openRotation, openTime));
			} else {
				aud.CloseSound ();
				StartCoroutine(RotateTo (closeRotation, openTime));
			}
		}
	}

	public void SetUnmatched () {
		isMatched = false;
		transform.rotation = closeRotation;
		StartCoroutine (ChangeAlpha (1f, 60f / 60f, false));
	}

	IEnumerator RotateTo (Quaternion rot, float time) {
		float speed = Quaternion.Angle (transform.rotation, rot) / time;
		//print ("Speed: " + speed.ToString() + " - deltaTime: " + Time.deltaTime + " - Degrees Delta: " + (speed * Time.deltaTime).ToString() + " - Rotating from: " + transform.rotation.ToString() + " -> " + rot.ToString() + " - ==>> " + Quaternion.Angle (transform.rotation, rot).ToString());
		while (Quaternion.Angle (transform.rotation, rot) > 0.1) {
			transform.rotation = Quaternion.RotateTowards (transform.rotation, rot, speed * Time.deltaTime);
			yield return null;
		} 
		transform.rotation = rot;
		yield return null;
	}

	bool _RotateTo (Quaternion rot, float angle, float time) {
		float speed = angle / time;
		//print ("Speed: " + speed.ToString() + " - deltaTime: " + Time.deltaTime + " - Degrees Delta: " + (speed * Time.deltaTime).ToString() + " - Rotating from: " + transform.rotation.ToString() + " -> " + rot.ToString() + " - ==>> " + Quaternion.Angle (transform.rotation, rot).ToString());
		if (Quaternion.Angle (transform.rotation, rot) > 0.1) {
			transform.rotation = Quaternion.RotateTowards (transform.rotation, rot, speed * Time.deltaTime);
			return true;
		} else {
			transform.rotation = rot;
			return false;
		}
	}

	public void TriggerMatched () {
		StopAllCoroutines ();
		InstantSetAlphaStates ();
		StartCoroutine (StartMatch ());
	}

	IEnumerator StartMatch () {
		isMatched = true;
		aud.MatchSound ();

		StartCoroutine (ChangeAlpha (0f, (60f / 60f), true));
		transform.rotation = openRotation;

		Quaternion rot1, rot2, rot3;
		rot1 = Quaternion.Euler (0, 120 * 1, 0);
		rot2 = Quaternion.Euler (0, 120 * 2, 0);
		rot3 = Quaternion.Euler (0, 120 * 3, 0);
		while (_RotateTo (rot1, 120, (25f / 60f) / 3f)) { yield return null; }
		while (_RotateTo (rot2, 120, (25f / 60f) / 3f)) { yield return null; }
		while (_RotateTo (rot3, 120, (25f / 60f) / 3f)) { yield return null; }


		rot1 = Quaternion.Euler (0, 360 + 120 * 1, 0);
		rot2 = Quaternion.Euler (0, 360 + 120*2, 0);
		rot3 = Quaternion.Euler (0, 360 + 120 * 3, 0);
		while (_RotateTo (rot1, 120, (35f / 60f) / 3f)) { yield return null; }
		while (_RotateTo (rot2, 120, (35f / 60f) / 3f)) { yield return null; }
		while (_RotateTo (rot3, 120, (35f / 60f) / 3f)) { yield return null; }

		yield return null;
	}

	IEnumerator ChangeAlpha (float to, float time, bool isBoth) {
		float speed = Mathf.Abs (cardBack.color.a - to) / time;
		while (Mathf.Abs (cardBack.color.a - to) > 0.01f) {
			cardBack.color = new Color (cardBack.color.r, cardBack.color.g, cardBack.color.b, Mathf.MoveTowards (cardBack.color.a, to, speed * Time.deltaTime));
			if (isBoth) front.color = new Color (front.color.r, front.color.g, front.color.b, Mathf.MoveTowards (front.color.a, to, speed * Time.deltaTime));
			yield return null;
		}
		front.color = new Color (front.color.r, front.color.g, front.color.b, to);
		cardBack.color = new Color (cardBack.color.r, cardBack.color.g, cardBack.color.b, to);
		yield return null;
	}

	public void TriggerJustRotate () {
		StopAllCoroutines ();
		InstantSetAlphaStates ();
		transform.rotation = closeRotation;
		StartCoroutine (JustRotate());
		RotateTo (Quaternion.Euler (0, -180, 0), 25f / 60f);
    }

	IEnumerator JustRotate () {
		front.color = new Color (front.color.r, front.color.g, front.color.b, 1);
		cardBack.color = new Color (cardBack.color.r, cardBack.color.g, cardBack.color.b, 1);

		Quaternion rot1, rot2, rot3;
		rot1 = Quaternion.Euler (0, 180 - 120 * 1, 0);
		rot2 = Quaternion.Euler (0, 180 - 120 * 2, 0);
		rot3 = Quaternion.Euler (0, 180 - 120 * 3, 0);
		while (_RotateTo (rot1, 120, (25f / 60f) / 3f)) { yield return null; }
		while (_RotateTo (rot2, 120, (25f / 60f) / 3f)) { yield return null; }
		while (_RotateTo (rot3, 120, (25f / 60f) / 3f)) { yield return null; }
	}

	Coroutine OtherReveal;
	void UpdateRevealState () {
		if (OtherReveal != null)
			StopCoroutine (OtherReveal);

		OtherReveal = StartCoroutine (Reveal());
	}

	IEnumerator Reveal () {
		if (isMatched) {
			front.color = new Color (front.color.r, front.color.g, front.color.b, 0);
			cardBack.color = new Color (cardBack.color.r, cardBack.color.g, cardBack.color.b, 0);
		} else {
			front.color = new Color (front.color.r, front.color.g, front.color.b, 1);
			cardBack.color = new Color (cardBack.color.r, cardBack.color.g, cardBack.color.b, 1);
		}

		while (Mathf.Abs (cardBack.color.a - (isRevealed ? 0f : 1f)) > 0.01f) {
			cardBack.color = new Color (cardBack.color.r, cardBack.color.g, cardBack.color.b, Mathf.MoveTowards (cardBack.color.a, (isRevealed ? 0f : 1f), 2f * Time.deltaTime));
			yield return null;
		}

		cardBack.color = new Color (cardBack.color.r, cardBack.color.g, cardBack.color.b, (isRevealed ? 0f : 1f));
		yield return null;
	}

	void InstantSetAlphaStates () {
		if (isMatched) {
			front.color = new Color (front.color.r, front.color.g, front.color.b, 0);
			cardBack.color = new Color (cardBack.color.r, cardBack.color.g, cardBack.color.b, 0);
		} else {
			front.color = new Color (front.color.r, front.color.g, front.color.b, 1);
			cardBack.color = new Color (cardBack.color.r, cardBack.color.g, cardBack.color.b, 1);
		}

		if (isRevealed) {
			cardBack.color = new Color (cardBack.color.r, cardBack.color.g, cardBack.color.b, 0);
		}
	}
}
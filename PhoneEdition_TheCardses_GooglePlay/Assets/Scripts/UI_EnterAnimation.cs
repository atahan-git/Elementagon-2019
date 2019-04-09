using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EnterAnimation : MonoBehaviour {

	public bool isTransitionEffect = false;
	public bool isUpPosition = false;
	public bool isDownPos = false;

	static Image transitionEffect;
	static Transform upPos;
	static Transform downPos;

	[Space]
	public Transform customUpPos;
	public Transform customDownPos;

	// Use this for initialization
	void Awake () {
		if (isTransitionEffect) {
			transitionEffect = GetComponent<Image> ();
			transitionEffect.enabled = false;
		}
		if (isUpPosition)
			upPos = transform;
		if (isDownPos)
			downPos = transform;
		
	}

	public void Setup () {
		if (customUpPos == null)
			customUpPos = upPos;

		if (customDownPos == null)
			customDownPos = downPos;
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void OpenAnimation () {
		gameObject.SetActive (true);
		StopAllCoroutines ();
		StartCoroutine (OpenAnim());
	}

	public void CloseAnimation () {
		StopAllCoroutines ();
		StartCoroutine (CloseAnim());
	}

	float enterSpeed = 10f;
	float colorSpeed = 5f;
	float darkestAlpha = 0.7f;
	IEnumerator OpenAnim () {
		transitionEffect.enabled = true;
		transform.position = customUpPos.position;
		transitionEffect.color = new Color (0, 0, 0, 0);

		transitionEffect.gameObject.transform.SetSiblingIndex (transform.GetSiblingIndex () - 1);

		while (Mathf.Abs (transform.position.y - customDownPos.position.y) > 1f) {
			transform.position = Vector3.Lerp (transform.position, customDownPos.position, enterSpeed * Time.deltaTime);
			transform.position = Vector3.MoveTowards (transform.position, customDownPos.position, enterSpeed*50 * Time.deltaTime);
			transitionEffect.color = new Color (0, 0, 0, Mathf.MoveTowards (transitionEffect.color.a, darkestAlpha, colorSpeed * Time.deltaTime));

			yield return null;
		}

		transform.position = customDownPos.position;
		transitionEffect.color = new Color (0, 0, 0, darkestAlpha);

		yield return null;
	}

	float exitSpeed = 10f;
	IEnumerator CloseAnim () {
		transitionEffect.enabled = true;
		transform.position = customDownPos.position;
		transitionEffect.color = new Color (0, 0, 0, 0);

		while (Mathf.Abs (transform.position.y - customUpPos.position.y) > 1f) {
			transform.position = Vector3.Lerp (transform.position, customUpPos.position, exitSpeed * Time.deltaTime);
			transform.position = Vector3.MoveTowards (transform.position, customUpPos.position, exitSpeed*50 * Time.deltaTime);
			transitionEffect.color = new Color (0, 0, 0, Mathf.MoveTowards (transitionEffect.color.a, 0f, colorSpeed * Time.deltaTime));

			yield return null;
		}

		transform.position = customUpPos.position;
		transitionEffect.color = new Color (0, 0, 0, 0);

		yield return null;
		transitionEffect.enabled = false;
		gameObject.SetActive (false);
	}
}

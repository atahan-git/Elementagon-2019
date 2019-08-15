using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyPoisonEffect : MonoBehaviour {

	public GameObject TheRing;
	public TMPro.TextMeshPro timerText;

	public void SetUp (float timer) {
		StartCoroutine (DeadlyCountdown (timer));
	}

	
	IEnumerator DeadlyCountdown (float timer) {

		Vector3 ringFullSize = TheRing.transform.localScale;
		Vector3 ringZerosize = Vector3.zero;

		float distance = Vector3.Distance (ringFullSize, ringZerosize)/2f;

		while (timer >= 0) {
			TheRing.transform.localScale = Vector3.Lerp (TheRing.transform.localScale, ringZerosize, (distance / timer) * Time.deltaTime);
			timer -= Time.deltaTime;

			timerText.text = Mathf.CeilToInt (timer).ToString ();
			yield return null;
		}

		yield return null;
	}

	public void GetCured () {

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GrowAtCreation : MonoBehaviour {

	public AnimationCurve growCurve = new AnimationCurve();

	public bool growOnStart = true;
	public bool growOnEnable = false;
	public bool simpleGrowth = true;
	public float growTime = 0.5f;
	float growSpeed;

	Vector3 grownScale;
	Vector3 startingScale;
	Vector3 targetScale;
	// Use this for initialization
	private void Awake () {
		grownScale = transform.localScale;
	}

	public void Start () {
		if (growOnStart)
			GrowtoFull ();
	}

	public void OnEnable () {
		if (growOnEnable)
			GrowtoFull ();
	}

	public void GrowtoFull () {
		targetScale = grownScale;
		StopAllCoroutines ();

		transform.localScale = Vector3.zero;
		growSpeed = (targetScale.magnitude - transform.localScale.magnitude) / growTime;
		StartCoroutine (Grow());
	}

	public void GrowDestruct (float destructTime) {
		targetScale = Vector3.zero;
		StopAllCoroutines ();

		growSpeed = Mathf.Abs (targetScale.magnitude - transform.localScale.magnitude) / destructTime;
		StartCoroutine (Grow ());
	}

	IEnumerator Grow () {
		startingScale = transform.localScale;
		float timer = 0;
		while (timer < growTime){
			if (simpleGrowth) {
				transform.localScale = Vector3.MoveTowards (transform.localScale, targetScale, growSpeed * Time.deltaTime);
			} else {
				transform.localScale = (targetScale - startingScale) * growCurve.Evaluate (timer / growTime);
			}
			timer += Time.deltaTime;
			yield return null;
		}

		transform.localScale = targetScale;

		yield return null;
	}

}

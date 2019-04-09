using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowToCardEffect : MonoBehaviour {

	public float waitTime = 0.4f;
	public float throwTime = 0.2f;
	public float destroyTime = 0.4f;

	public float forwardOffset = 2f;

	public Vector3 curPos;
	public Vector3 target;

	public GameObject[] toActivate;

	// Use this for initialization
	void Start () {
		target = transform.position;

		curPos = PowerUpManager.s.throwToCardStartPos.position;
		curPos = new Vector3 (curPos.x, curPos.y, target.z);
		transform.position = curPos;
		transform.rotation = Quaternion.LookRotation (target - curPos, Vector3.forward);
		transform.position += transform.forward * forwardOffset;
		curPos = transform.position;

		foreach (GameObject gm in toActivate) {
			if (gm != null) {
				gm.SetActive (true);
			}
		}

		StartCoroutine (Sequence());
	}
	

	IEnumerator Sequence () {
		yield return new WaitForSeconds (waitTime);
		float dist = Vector3.Distance (curPos, target);

		while (Vector3.Distance (curPos, target) > 0.01f) {
			curPos = Vector3.MoveTowards (curPos, target, (dist / throwTime) * Time.deltaTime);
			transform.position = curPos;
			yield return null;
		}

		transform.position = target;

		yield return new WaitForSeconds (destroyTime);

		GetComponent<DisableAndDestroy> ().Engage ();

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPawnMover : MonoBehaviour {

	public string pawnName = "New Pawn";

	public GameObject customStartArea;
	public LevelHolder[] myLevels;
	public Vector3 posOffset;

	public int curLevel = 0;
	Vector3 goToVector;
	Vector3 startVector;
	float leapTime = 1f;

	float leapHeight = 30f;

	public AnimationCurve leapCurve;

	bool triggerMovement = false;

	private void Start () {
		if (myLevels.Length > 0) {
			curLevel = PlayerPrefs.GetInt (pawnName + "level", -1);
			if (curLevel == -1 && customStartArea == null)
				curLevel = 0;

			if (curLevel == -1)
				transform.localPosition = customStartArea.transform.localPosition + posOffset;
			else
				transform.localPosition = myLevels[curLevel].transform.localPosition + posOffset;
			startVector = transform.localPosition;
			int oldLvl = curLevel;

			while (true) {
				curLevel++;
				if (curLevel < myLevels.Length) {
					if (!myLevels[curLevel].isInitialized)
						myLevels[curLevel].Start ();
					if (!myLevels[curLevel].isUnlocked) {
						break;
					}
				} else {
					break;
				}
			}
			curLevel -= 1;

			goToVector = myLevels[curLevel].transform.localPosition + posOffset;

			print (oldLvl.ToString () + " - " + curLevel.ToString());
			if (oldLvl != curLevel)
				triggerMovement = true;

		} else {
			gameObject.SetActive (false);
		}
	}

	private void OnEnable () {
		transform.localPosition = startVector;
		if (triggerMovement) {
			StartCoroutine (GoToPos ());
		}
	}

	IEnumerator GoToPos () {
		transform.localPosition = startVector;

		yield return new WaitForSeconds (0.3f);

		myLevels[curLevel].TriggerLevelUnlockedAnimation ();

		yield return new WaitForSeconds (0.8f);

		float counter = 0f;
		while (counter < leapTime) {
			transform.localPosition = Vector3.Lerp (startVector, goToVector, counter / leapTime);
			transform.localPosition += Vector3.Lerp (Vector3.zero, Vector3.up * leapHeight, leapCurve.Evaluate (counter / leapTime));

			counter += Time.deltaTime;
			yield return null;
		}

		transform.localPosition = goToVector;
		yield return null;

		PlayerPrefs.SetInt (pawnName + "level", curLevel);
	}

}

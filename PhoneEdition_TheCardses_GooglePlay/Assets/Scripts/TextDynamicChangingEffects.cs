using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDynamicChangingEffects : MonoBehaviour {
	TextMeshProUGUI myText;

	public float goUpTime = 0.4f;
	[Header("These should usually be the same")]
	public float upSpeed = 10f;
	public float upAcceleration = 10f;


	GameObject stuffToSpawn;
	// Start is called before the first frame update
	void Start () {
		myText = GetComponent<TextMeshProUGUI> ();

		stuffToSpawn = Instantiate (myText.gameObject, myText.transform);
		stuffToSpawn.transform.localPosition = Vector3.zero;
		stuffToSpawn.SetActive (false);
		stuffToSpawn.GetComponent<TextDynamicChangingEffects> ().enabled = false;
		stuffToSpawn.transform.localPosition = Vector3.zero;
	}

	public string text {
		get { return myText.text; }
		set {
			if (myText != null) {
				if (!value.Equals (myText.text)) {
					EngageAnimation ();
					myText.text = value;
				}
			} else {
				DataLogger.LogError ("Text of Dynamic text changer not set!" + name);
			}
		}
	}

	int n = 0;
	int maxCount = 50;
	void EngageAnimation () {
		DataLogger.LogMessage ("Spawning text changed animation");
		if(n < maxCount)
		StartCoroutine (Animate (Instantiate (stuffToSpawn, myText.transform)));
	}

	IEnumerator Animate (GameObject myObj) {
		myObj.GetComponent<TextMeshProUGUI> ().text = myText.text;
		myObj.SetActive (true);
		myObj.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
		TextMeshProUGUI fadeText = myObj.GetComponent<TextMeshProUGUI> ();
		n++;
		float timer = 0;

		while (timer <= goUpTime) {
			myObj.transform.Translate (0, (upSpeed + (upAcceleration*timer)) * Time.deltaTime, 0);
			fadeText.color = new Color (fadeText.color.r, fadeText.color.g, fadeText.color.b, Mathf.Lerp (1f, 0f, timer / goUpTime));
			timer += Time.deltaTime;
			yield return null;
		}

		n--;
		Destroy (myObj);
		yield return null;
	}
}

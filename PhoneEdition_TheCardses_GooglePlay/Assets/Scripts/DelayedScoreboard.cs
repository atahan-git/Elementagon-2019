using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DelayedScoreboard : MonoBehaviour {

	public static DelayedScoreboard s;

	public int myPlayer = 0;

	public float delay = 1.5f;

	public int myScore = 0;

	TMPro.TextMeshProUGUI myText;

	// Use this for initialization
	void Awake () {
		s = this;
		myText = GetComponent<TMPro.TextMeshProUGUI> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateScore (int newScore, bool isDelayed) {
		myScore = newScore;
		if (isDelayed) {
			StartCoroutine (UpdateGfx (myScore, delay));
		} else {
			StartCoroutine (UpdateGfx (myScore, 0));
		}
	}

	IEnumerator UpdateGfx (int score, float delayAm) {
		yield return new WaitForSeconds (delayAm);
		if (GS.a.scoreReach > 0)
			myText.text = score.ToString () + "/" + GS.a.scoreReach;
		else
			myText.text = score.ToString ();
	}

	public void UpdateScoreReach () {
		if (GS.a.scoreReach > 0)
			myText.text = myScore.ToString () + "/" + GS.a.scoreReach;
		else
			myText.text = myScore.ToString ();
	}
}

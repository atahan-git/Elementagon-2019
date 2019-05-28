using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmartScoreboard : MonoBehaviour {

	public static SmartScoreboard s;

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
		switch (GS.a.myGameObjectiveType) {
		default:
		case GameSettings.GameObjectiveTypes.Standard:
			if (GS.a.scoreReach > 0)
				myText.text = score.ToString () + "/" + GS.a.scoreReach;
			else
				myText.text = score.ToString (); break;
		case GameSettings.GameObjectiveTypes.Haggle:
				
			switch (GS.a.myGameType) {
			case GameSettings.GameType.Singleplayer:
				myText.text = score.ToString () + "/" + (GS.a.scoreReach + ScoreBoardManager.s.allScores[3, 0]) + "◍";
				break;
			case GameSettings.GameType.OneVOne:
				myText.text = score.ToString () + "/" + (GS.a.scoreReach + ScoreBoardManager.s.allScores[1, 0]) + "◍";
				break;
			case GameSettings.GameType.Two_Coop:
			case GameSettings.GameType.TwoVTwo:
				myText.text = score.ToString () + "/" + (GS.a.scoreReach + ScoreBoardManager.s.allScores[5, 0]) + "◍";
				break;
			case GameSettings.GameType.Three_FreeForAll:
			case GameSettings.GameType.Four_FreeForAll:
				myText.text = "This Game type is not supported! " + GS.a.myGameObjectiveType.ToString () + " - " + GS.a.myGameType.ToString ();
				break;
			}
			break;
		case GameSettings.GameObjectiveTypes.Health:
			myText.text = score.ToString () + "<3";
			break;
		}
	}

	public void UpdateScoreReach () {
		StartCoroutine (UpdateGfx (myScore, 0));
	}
}

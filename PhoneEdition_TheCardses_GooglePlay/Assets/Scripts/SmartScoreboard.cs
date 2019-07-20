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
		DataLogger.LogError ("Smart Scoreboard is now deprecated");
		return;
		myScore = newScore;
		if (isDelayed) {
			StartCoroutine (UpdateGfx (myScore, delay));
		} else {
			StartCoroutine (UpdateGfx (myScore, 0));
		}
	}

	IEnumerator UpdateGfx (int score, float delayAm) {
		yield return new WaitForSeconds (delayAm);
		DataLogger.LogError ("Smart Scoreboard is now deprecated");
		/*switch (GS.a.myGameRuleType) {
		default:
		case GameSettings.GameRuleTypes.Standard:
			if (GS.a.scoreReach > 0)
				myText.text = score.ToString () + "/" + GS.a.scoreReach;
			else
				myText.text = score.ToString (); break;
		case GameSettings.GameRuleTypes.Haggle:
				
			switch (GS.a.myGamePlayerType) {
			case GameSettings.GamePlayerTypes.Singleplayer:
				myText.text = score.ToString () + "/" + (GS.a.scoreReach + ScoreBoardManager.s.allScores[3, 0]) + "◍";
				break;
			case GameSettings.GamePlayerTypes.OneVOne:
				myText.text = score.ToString () + "/" + (GS.a.scoreReach + ScoreBoardManager.s.allScores[1, 0]) + "◍";
				break;
			case GameSettings.GamePlayerTypes.Two_Coop:
			case GameSettings.GamePlayerTypes.TwoVTwo:
				myText.text = score.ToString () + "/" + (GS.a.scoreReach + ScoreBoardManager.s.allScores[5, 0]) + "◍";
				break;
			case GameSettings.GamePlayerTypes.Three_FreeForAll:
			case GameSettings.GamePlayerTypes.Four_FreeForAll:
				myText.text = "This Game type is not supported! " + GS.a.myGameRuleType.ToString () + " - " + GS.a.myGamePlayerType.ToString ();
				break;
			}
			break;
		case GameSettings.GameRuleTypes.Health:
			myText.text = score.ToString () + "<3";
			break;
		}*/
	}

	public void UpdateScoreReach () {
		StartCoroutine (UpdateGfx (myScore, 0));
	}
}

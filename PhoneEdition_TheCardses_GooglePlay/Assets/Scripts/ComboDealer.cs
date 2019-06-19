using UnityEngine;
using System.Collections;

public class ComboDealer : MonoBehaviour {

	public static ComboDealer s;

	//public float comboTimer;
	public int comboCount = 1;
	public int lastCardType = -1;

	//public GameObject[] comboEffect;
	public GameObject comboEffect;
	public GameObject sameCardComboEffect;

	private void Start () {
		s = this;
	}

	public void NotMatched () {
		comboCount = 1;
	}

	//combo numbers >>
	//1 - 2 - 3 - 4 - 5 - 6
	//same color = ceil(n*1.5)
	public void ProcessCombo (int playerId, bool isdelayed, IndividualCard card1, IndividualCard card2) {

		/*if (myCardType < GS.a.cardSettings.cardScores.Length)
			CardMatchCoolEffect.s.MatchTwo (myPlayerinteger, cardsToCheck[k], cardsToCheck[l], GS.a.cardSettings.cardScores[myCardType]);*/

		int elementalType = card1.cBase.elementType;

		//print ("Adding with combo check");
		if (!card1.cBase.isItem) {
			//we got a card
			int scoreToAdd = card1.cBase.score;
			int enemyScoreToAdd = card1.cBase.enemyScore;

			if (GS.a.canCombo && playerId == DataHandler.s.myPlayerInteger) {
				bool isSame = false;

				if (scoreToAdd == 0 && enemyScoreToAdd == 0) {
					lastCardType = elementalType;
					return;
				}

				if (elementalType != lastCardType) {
					scoreToAdd *= comboCount;
					enemyScoreToAdd *= comboCount;
				} else {
					scoreToAdd *= Mathf.CeilToInt ((float)comboCount * 1.5f);
					enemyScoreToAdd *= Mathf.CeilToInt ((float)comboCount * 1.5f);
					isSame = true;
				}

				if (comboCount > 1) {
					Tutorial_FirstTimeStuff.ComboCheck ();

					GameObject toIns = isSame ? sameCardComboEffect : comboEffect;

					GameObject myObj = Instantiate (toIns);
					BetweenCardsEffect.AlignBetweenCards (myObj, card1, card2, BetweenCardsEffect.AlignMode.both);
					float scale = 0.5f + ((isSame ? Mathf.CeilToInt (comboCount * 1.5f) : comboCount) * 0.2f);
					myObj.transform.localScale = new Vector3 (scale, scale, scale);

					try {
						myObj.GetComponentInChildren<TMPro.TextMeshPro> ().text = (isSame ? ("Same Type Combo! x" + (Mathf.CeilToInt (comboCount * 1.5f)).ToString ()) : ("Combo! x" + (comboCount).ToString ()));
					} catch {
						print ("Combo text not found");
					}
				}

				lastCardType = elementalType;
				comboCount++;
			}

			/*if (isIncreaseScoreReach) {
				GS.a.scoreReach += scoreToAdd;
				DelayedScoreboard.s.UpdateScoreReach ();
			}*/
			if (scoreToAdd != 0)
				ScoreBoardManager.s.AddScore (playerId, elementalType, scoreToAdd, isdelayed);
			else
				Tutorial_FirstTimeStuff.NoPointCards ();

			if (enemyScoreToAdd != 0) {
				ScoreBoardManager.s.AddScoreToOthers (playerId, elementalType, enemyScoreToAdd, isdelayed);
			}
		} else {
			//we didnt get a card, but an item
			DataLogger.LogMessage ("Got an item: " + card1.cBase.myItem.name);
			InventoryMaster.s.Add (card1.cBase.myItem);
		}
	}

	/*
	//debug
	public Transform t1;
	public Transform t2;

	public GameObject o;

	public void Update () {
		if (t1.position.x > t2.position.x) {
			Transform temp = t1;
			t1 = t2;
			t2 = temp;
		}

		Vector3 lookVector = t2.position - t1.position;

		o.transform.position = Vector3.Lerp (t1.position, t2.position, 0.5f);
		Quaternion myRot = Quaternion.LookRotation (lookVector, Vector3.up);
		//o.transform.rotation = myRot;
		o.transform.rotation = Quaternion.Euler (Mathf.Clamp( myRot.eulerAngles.x < 180 ? myRot.eulerAngles.x : (myRot.eulerAngles.x - 360f),-30f,30f),myRot.eulerAngles.y,myRot.eulerAngles.z);
		print (o.transform.rotation.eulerAngles);
	}*/
}

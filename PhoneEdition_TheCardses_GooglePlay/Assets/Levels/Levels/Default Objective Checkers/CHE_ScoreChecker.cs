using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHE_ScoreChecker : MonoBehaviour, IObjectiveChecker {

	int myScoreVal = 0;

	public float GetValue () {
		//return ScoreBoardManager.s.allScores[DataHandler.s.myPlayerInteger, 0];
		return myScoreVal;
	}

	public void Initialize (Objective myObj) {
		ScoreBoardManager.s.AddScoreHook += ScoreAdded;
	}


	void ScoreAdded (int playerInt, int scoreElementalType, int toAdd, bool isDelayed, bool careGameTypes) {
		if (playerInt == DataHandler.s.myPlayerInteger) {
			if (isDelayed)
				GameObjectiveMaster.s.StartCoroutine (DelayedScoreAdd (toAdd));
			else
				myScoreVal += toAdd;
		}
	}

	IEnumerator DelayedScoreAdd (int toAdd) {
		yield return new WaitForSeconds (1f);

		myScoreVal += toAdd;
	}

	public void ObjectiveDone () {
	}
}

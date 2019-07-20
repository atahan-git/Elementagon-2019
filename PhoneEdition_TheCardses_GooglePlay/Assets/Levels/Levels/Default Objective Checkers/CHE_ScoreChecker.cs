using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHE_ScoreChecker : MonoBehaviour, IObjectiveChecker {

	public float GetValue () {
		return ScoreBoardManager.s.allScores[DataHandler.s.myPlayerInteger, 0];
	}

	public void Initialize (Objective myObj) {
	}

	public void ObjectiveDone () {
	}
}

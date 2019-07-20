using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHE_HaggleChecker : MonoBehaviour, IObjectiveChecker {
	Objective myObj;
	float startingValue = 0;

	public float GetValue () {
		UpdateRequiredValue ();
		return ScoreBoardManager.s.allScores[DataHandler.s.myPlayerInteger, 0];
	}

	public void UpdateRequiredValue () {
		myObj.requiredValue = startingValue + ScoreBoardManager.s.allScores[DataHandler.NPCInteger, 0];
	}

	public void Initialize (Objective _myObj) {
		myObj = _myObj;
		print (myObj);
		startingValue = myObj.requiredValue;
	}

	public void ObjectiveDone () {
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHE_TurnChecker : MonoBehaviour, IObjectiveChecker {
	public static int turnCount = 0;

	public void Initialize (Objective myObj) {
		turnCount = 0;
	}

	public float GetValue () {
		return turnCount;
	}

	public void ObjectiveDone () {
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHE_PangeLastLevelDragonChecker : MonoBehaviour, IObjectiveChecker {
	public static float isDone = 0;

	public float GetValue () {
		return isDone;
	}

	public void Initialize (Objective myObj) {
	}

	public void ObjectiveDone () {
	}
}

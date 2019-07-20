using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHE_TimeChecker : MonoBehaviour, IObjectiveChecker {
	public float GetValue () {
		return GameObjectiveMaster.s.timer;
	}

	public void Initialize (Objective myObj) {
	}

	public void ObjectiveDone () {
	}
}

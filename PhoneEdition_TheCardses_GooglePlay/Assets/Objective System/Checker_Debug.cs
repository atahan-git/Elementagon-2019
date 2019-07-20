using UnityEngine;
using System.Collections;

public class Checker_Debug : MonoBehaviour, IObjectiveChecker {

	public float curValue {
		get{
			return leValue;
		}
		set{
			curValue = value;
		}
	}

	public float leValue = 0;
		
	public float GetValue () {
		return curValue;
	}

	public void ObjectiveDone () {
	}

	public void Initialize (Objective myObj) {
	}
}

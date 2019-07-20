using UnityEngine;
using System.Collections;
using TypeReferences;

[System.Serializable]
public class Objective {

	public bool isDone = false;

	public string name;

	public enum conditionType{
		ifTrue,
		ifFalse,
		ifAll,
		ifAny,
		ifLessThan,
		ifMoreThan,
		ifMoreThanOrEqual,
		ifLessThanOrEqual,
		ifEqual
	}
	public conditionType type = conditionType.ifLessThanOrEqual;

	[Tooltip("Use 0 and 1 for booleans")]
	public float requiredValue = 1;
	/*[Tooltip ("Doesn't matter for booleans")]
	public float maxValue = 1;*/

	//this script will check our target objective and give us values
	//switch this script for different functionality
	[SerializeField]
	[ClassImplements (typeof (IObjectiveChecker))]
	ClassTypeReference myChecker = null;

	IObjectiveChecker _myObjectiveChecker = null; 
	public IObjectiveChecker myObjectiveChecker {
		get {
			if (_myObjectiveChecker == null) {
				try {
					_myObjectiveChecker = System.Activator.CreateInstance (myChecker) as IObjectiveChecker;
				} catch {
					DataLogger.LogError ("Cant Create Checker! " + myChecker.ToString());
				}
			}
			return _myObjectiveChecker;
		}
	}
}


public interface IObjectiveChecker {
	void Initialize (Objective myObj);
	float GetValue ();
	void ObjectiveDone ();
}
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class ObjectiveGUI : MonoBehaviour {
	

	Objective myObjective;

	//reference
	/*public enum conditionType{
		ifTrue,
		ifFalse,
		ifAll,
		ifAny,
		ifLessThan,
		ifMoreThan,
		ifEqual
	}*/

	public GameObject ifTrue;
	public GameObject ifFalse;
	public GameObject ifAll;
	public GameObject ifAny;
	public GameObject ifLessThan;
	public GameObject ifMoreThan;
	public GameObject ifEqual;


	Slider slider;
	Toggle toggle;
	Image image;
    public TextDynamicChangingEffects numText;
    public TextMeshProUGUI text;

	GameObject toSpawn;

	// Use this for initialization
	public void SetUpObjectiveGUI (Objective _myObj) {
		myObjective = _myObj;

		switch (myObjective.type) {
		case Objective.conditionType.ifTrue:
			toSpawn = ifTrue;
			break;
		case Objective.conditionType.ifFalse:
			toSpawn = ifFalse;
			break;
		case Objective.conditionType.ifAll:
			toSpawn = ifAll;
			break;
		case Objective.conditionType.ifAny:
			toSpawn = ifAny;
			break;
		case Objective.conditionType.ifLessThanOrEqual:
		case Objective.conditionType.ifLessThan:
			toSpawn = ifLessThan;
			break;
		case Objective.conditionType.ifMoreThanOrEqual:
		case Objective.conditionType.ifMoreThan:
			toSpawn = ifMoreThan;
			break;
		case Objective.conditionType.ifEqual:
			toSpawn = ifEqual;
			break;
		}

		if (toSpawn) {
			GameObject spawned = (GameObject)Instantiate (toSpawn, transform);
			slider = spawned.GetComponent<Slider> ();
			toggle = spawned.GetComponent<Toggle> ();
			numText = spawned.GetComponentInChildren<TextDynamicChangingEffects> ();
		}

		image = GetComponent<Image> ();
		text = GetComponentInChildren<TextMeshProUGUI> ();
		text.text = myObjective.name;
	
	}
	
	// Update is called once per frame
	void Update () {

		if (toggle) {
			if (myObjective.myObjectiveChecker.GetValue () == 1)
				toggle.isOn = true;
			else
				toggle.isOn = false;
		}
        if (slider) {
			slider.maxValue = myObjective.requiredValue;
			slider.value = myObjective.myObjectiveChecker.GetValue ();
		}
        if (numText) {
            switch (myObjective.myDisplayType) {
                case Objective.displayType.justValue:
                    numText.text = ((int)myObjective.myObjectiveChecker.GetValue()).ToString();
                    break;
                case Objective.displayType.fraction:
                    numText.text = ((int)myObjective.myObjectiveChecker.GetValue()).ToString() + "/" + myObjective.requiredValue.ToString();
                    break;
                case Objective.displayType.countdown:
                    numText.text = GameObjectiveMaster.TimeToString(((int)myObjective.requiredValue - (int)myObjective.myObjectiveChecker.GetValue()));
                    break;
            }
		}

		if (myObjective.isDone)
			Done ();
		else
			NotDone ();
	}

	public void Done(){
		image.color = Color.green;
	}
	public void NotDone(){
		image.color = Color.white;
	}
}


/*
		switch (myObjective.type) {
		case Objective.conditionType.ifTrue:
			
			break;
		case Objective.conditionType.ifFalse:
			
			break;
		case Objective.conditionType.ifAll:
			
			break;
		case Objective.conditionType.ifAny:
			
			break;
		case Objective.conditionType.ifLessThan:
			
			break;
		case Objective.conditionType.ifMoreThan:
			
			break;
		case Objective.conditionType.ifEqual:
			
			break;
		}
*/
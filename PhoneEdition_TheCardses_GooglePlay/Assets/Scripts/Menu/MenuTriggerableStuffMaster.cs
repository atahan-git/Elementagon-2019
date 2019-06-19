using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTriggerableStuffMaster : MonoBehaviour {

	public Triggerable[] allTriggerables;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < allTriggerables.Length; i++) {
			if (allTriggerables[i] != null)
				allTriggerables[i].id = i;
		}

		if (SaveMaster.s.mySave.triggeredEvents == null)
			SaveMaster.s.mySave.triggeredEvents = new bool[allTriggerables.Length];

		if (SaveMaster.s.mySave.triggeredEvents.Length < allTriggerables.Length) {
			bool[] temp = SaveMaster.s.mySave.triggeredEvents;
			SaveMaster.s.mySave.triggeredEvents = new bool[allTriggerables.Length];
			temp.CopyTo (SaveMaster.s.mySave.triggeredEvents, 0);
		}
	}

	public static bool IsTriggerDone (Triggerable obj) {
		if (obj == null)
			return true;

		int levelId = obj.id;
		if (levelId < SaveMaster.s.mySave.triggeredEvents.Length) {
			if (SaveMaster.s.mySave.triggeredEvents[levelId]) {
				return true;
			} else {
				return false;
			}
		} else {
			Debug.LogError ("Event not registered");
			return false;
		}
	}


	public class Triggerable : MonoBehaviour {
		[HideInInspector]
		public int id = -1;
	}
}

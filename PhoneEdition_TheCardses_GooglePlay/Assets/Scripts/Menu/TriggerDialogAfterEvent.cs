using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogAfterEvent : MenuTriggerableStuffMaster.Triggerable {

	public GameSettings myLevel;
	public DialogTreeAsset myDialog;

	public int eventId = -1;

	// Use this for initialization
	void Start () {
		if (SaveMaster.s.mySave.triggeredEvents == null)
			SaveMaster.s.mySave.triggeredEvents = new bool[5];

		if (SaveMaster.s.mySave.triggeredEvents.Length <= eventId) {
			bool[] temp =  SaveMaster.s.mySave.triggeredEvents;
			SaveMaster.s.mySave.triggeredEvents = new bool[eventId];
			temp.CopyTo (SaveMaster.s.mySave.triggeredEvents, 0);
		}

		if (!SaveMaster.s.mySave.triggeredEvents[eventId]) {
			if (SaveMaster.isLevelDone (myLevel)) {
				DialogTree.s.LoadFromAsset (myDialog);
				DialogTree.s.StartDialog ();
				SaveMaster.s.mySave.triggeredEvents[eventId] = true;
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogAfterEvent : MonoBehaviour {

	public int eventId = -1;

	[Space]
	public GameSettings myLevel;
	public DialogTreeAsset myDialog;

	//public UnlockRequirementKeeper myReqs;

	[Space]
	[Tooltip ("Leave -1 for not checking")]
	public int questDecisionLockId = -1;
	[Tooltip ("leave as -1 if you want to swap based on decision and use the below array to put decision stuff")]
	public float questDecisionReqValue = -1;

	[Tooltip ("leave this empty if you want this just to be a if check")]
	public DialogTreeAsset[] questDecisionDialogSwap = new DialogTreeAsset[3];

	// Use this for initialization
	void Start () {
		bool isNotTriggeredYet, isLevelDone, isNotQuestSwapped, isQuestDone;
		isNotQuestSwapped = questDecisionReqValue == -1;

		//Check if this event was triggered before
		if (SaveMaster.s.mySave.triggeredEvents.Length <= eventId) {
			bool[] temp = SaveMaster.s.mySave.triggeredEvents;
			SaveMaster.s.mySave.triggeredEvents = new bool[eventId];
			temp.CopyTo (SaveMaster.s.mySave.triggeredEvents, 0);
		}
		isNotTriggeredYet = !SaveMaster.s.mySave.triggeredEvents[eventId];


		//check if the level is done
		isLevelDone = SaveMaster.isLevelDone (myLevel);


		if (isNotTriggeredYet && isLevelDone) {
			if (SaveMaster.s.mySave.questDecisions.Length < questDecisionLockId) {
				float[] temp = SaveMaster.s.mySave.questDecisions;
				SaveMaster.s.mySave.questDecisions = new float[questDecisionLockId];
				temp.CopyTo (SaveMaster.s.mySave.questDecisions, 0);
			}

			if (isNotQuestSwapped) {
                //check if the quest requirements are met
                isQuestDone = true;
                if(questDecisionLockId != -1 && questDecisionLockId < SaveMaster.s.mySave.questDecisions.Length)
				    isQuestDone = SaveMaster.s.mySave.questDecisions[questDecisionLockId] == questDecisionReqValue;

				if (isQuestDone) {
					DialogTree.s.LoadFromAsset (myDialog);
					DialogTree.s.StartDialog ();
					SaveMaster.s.mySave.triggeredEvents[eventId] = true;
				}
			} else {
				//swap based on quest decision
				DialogTreeAsset myDecisionDialog = null;
				try {
					myDecisionDialog = questDecisionDialogSwap[(int)SaveMaster.s.mySave.questDecisions[questDecisionLockId]];
				} catch (System.Exception e) {
					DataLogger.LogError ("Can't trigger dialog after event! " + eventId, e);
				}

				if (myDecisionDialog != null) {
					DialogTree.s.LoadFromAsset (myDecisionDialog);
					DialogTree.s.StartDialog ();
					SaveMaster.s.mySave.triggeredEvents[eventId] = true;
				}
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHE_ElementalScoreChecker : MonoBehaviour, IObjectiveChecker {
	Objective myObj;
	int dynamicCardId = 0;

	public float GetValue () {
		return ScoreBoardManager.s.allScores[DataHandler.s.myPlayerInteger, dynamicCardId];
	}


	public void Initialize (Objective _myObj) {
		myObj = _myObj;
		print (myObj);
		Invoke ("LateInitialize", 0.5f);
	}

	void LateInitialize () {
		for (int i = 0; i < CardTypeRandomizer.s.allCards.Length; i++) {
			if (myObj != null) {
				if (CardTypeRandomizer.s.allCards[i].name == myObj.checkerSetting) {
					dynamicCardId = i;
					break;
				}
			}
		}

		if (dynamicCardId == 0)
			DataLogger.LogError ("Can't find specificied card! " + myObj.checkerSetting);
	}

	public void ObjectiveDone () {
	}
}

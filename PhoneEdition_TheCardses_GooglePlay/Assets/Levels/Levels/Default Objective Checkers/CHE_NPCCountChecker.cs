using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHE_NPCCountChecker : MonoBehaviour, IObjectiveChecker {

	public float GetValue () {
		return NPCManager.s.ActiveNPCS.Count + (GS.a.npcSpawnCount != -1 ? GS.a.npcSpawnCount : 0);
	}

	public void Initialize (Objective myObj) {
	}

	public void ObjectiveDone () {
	}
}

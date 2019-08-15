using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PangeDragonLastLevel : MonoBehaviour {

	public NPCBase rockGolemPrefab;
	public NPCBase entPrefab;
	public NPCBase dragonPrefab;

	// Start is called before the first frame update
	void Start () {
		StartCoroutine (PangeLevel ());
	}

	IEnumerator PangeLevel () {
		yield return new WaitForSeconds (1f);

		for (int i = 0; i < 2; i++) {
			NPCManager.s.SpawnNPC (rockGolemPrefab);
			yield return new WaitForSeconds (5f);
		}

		yield return new WaitUntil (() => ScoreBoardManager.s.allScores[DataHandler.s.myPlayerInteger,0] >= 3);

		NPCManager.s.KillAllNPCs ();

		ScoreBoardManager.s.allScores[DataHandler.s.myPlayerInteger, 0] = 0;

		for (int i = 0; i < 2; i++) {
			NPCManager.s.SpawnNPC (entPrefab);
			yield return new WaitForSeconds (5f);
		}

		yield return new WaitUntil (() => ScoreBoardManager.s.allScores[DataHandler.s.myPlayerInteger, 0] >= 3);

		NPCManager.s.KillAllNPCs ();

		ScoreBoardManager.s.allScores[DataHandler.s.myPlayerInteger, 0] = 0;

		NPCManager.s.SpawnNPC (dragonPrefab);

		yield return new WaitUntil (() => ScoreBoardManager.s.allScores[DataHandler.s.myPlayerInteger, 0] >= 3);

		NPCManager.s.KillAllNPCs ();

		CHE_PangeLastLevelDragonChecker.isDone = 1;

		yield return null;
	}
}

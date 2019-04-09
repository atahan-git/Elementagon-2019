using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivesSystem : MonoBehaviour {

	public static ObjectivesSystem s;

	/*public GameObject basicObj;
	GameObject obj;*/

	/*public string[] typeNames = new string[15];
	public string[] eventNames = new string[3];

	// Use this for initialization
	void Start () {
		s = this;

		memSettings = GS.a.Copy();
		if (GS.a.objectiveSettings.objs_isEnabled && DataHandler.s.myPlayerinteger == 0)
			StartCoroutine (MainLoop ());
	}

	GameSetting memSettings = new GameSetting();

	public bool objectivesActive = true;
	
	// Update is called once per frame
	IEnumerator MainLoop (){
		while (objectivesActive) {

			yield return new WaitForSeconds (Random.Range (GS.a.objectiveSettings.objs_betweenTime.x, GS.a.objectiveSettings.objs_betweenTime.y));

			int cardType = Random.Range (1, 15);

			//pick the type of objective we will have
			switch (Random.Range (0, 3)) {

			//multSpawnChance
			case 0: 
			//if we cant actually pick that card type because of our filter
				while (!GS.a.objectiveSettings.objs_basic.filter_mSC [cardType] || GS.a.cardSettings.cardChances[cardType] == 0)
					cardType = Random.Range (1, 15);
				
				ApplyMultSpawnChance (cardType);
				SendNetworkObjectiveAction (true, cardType, 0);

				break;

			//multDragonChance
			case 1:

				ApplyMultDragonChance (cardType);
				SendNetworkObjectiveAction (true, cardType, 1);

				break;

			//multScoreGained
			case 2:
			//if we cant actually pick that card type because of our filter
				while (!GS.a.objectiveSettings.objs_basic.filter_mSG [cardType])
					cardType = Random.Range (1, 15);

				ApplyMultScoreGained (cardType);
				SendNetworkObjectiveAction (true, cardType, 2);

				break;
			}

			yield return new WaitForSeconds (Random.Range (GS.a.objectiveSettings.objs_basic.activeTime.x, GS.a.objectiveSettings.objs_basic.activeTime.y));

			//Revert our changes
			GS.a = memSettings.Copy ();
			BasicObjectiveGfx.s.Close ();
			SendNetworkObjectiveAction (false, -1, -1);
		}
	}

	void ApplyMultSpawnChance (int cardType){
		//change our value according to our amount
		GS.a.cardSettings.cardChances [cardType] = GS.a.cardSettings.cardChances [cardType] * GS.a.objectiveSettings.objs_basic.amount_mSC;
		//activate graphic
		BasicObjectiveGfx.s.Open (cardType, typeNames [cardType] + " " + eventNames [0]);
	}

	void ApplyMultDragonChance (int cardType){
		for (int i = 8; i < 15; i++) {
			if(GS.a.objectiveSettings.objs_basic.filter_mDC[i])
				GS.a.cardSettings.cardChances [i] = GS.a.cardSettings.cardChances [i] * GS.a.objectiveSettings.objs_basic.amount_mDC;
		}

		BasicObjectiveGfx.s.Open (cardType, eventNames [1]);
	}

	void ApplyMultScoreGained (int cardType){
		GS.a.cardSettings.cardScores [cardType] = (GS.a.cardSettings.cardScores [cardType] * GS.a.objectiveSettings.objs_basic.amount_mSG);
		//activate graphic
		BasicObjectiveGfx.s.Open (cardType, typeNames [cardType] + " " + eventNames [2]);
	}


	void SendNetworkObjectiveAction (bool isOn, int cardType, int objectiveType){
		DataHandler.s.SendObjectiveAction (isOn, cardType, objectiveType);
	}

	public void ReceiveNetworkObjectiveAction (bool isOn, int cardType, int objectiveType){
		if (!isOn) {
			ReceiveNetworkObjectiveAction (false);
			return;
		}

		switch (objectiveType) {
		case 0:
			ApplyMultSpawnChance (cardType);
			break;
		case 1:
			ApplyMultDragonChance (cardType);
			break;
		case 2:
			ApplyMultScoreGained (cardType);
			break;
		}
	}

	void ReceiveNetworkObjectiveAction (bool isOn){
		GS.a = memSettings.Copy ();
		BasicObjectiveGfx.s.Close ();
	}

	[System.Serializable]
	public class BasicObjectives {

		public Vector2 activeTime = new Vector2(15f,35f);

		[Space]

		public bool multSpawnChance = true;
		[Tooltip("//--------CARD TYPES---------\n// 0 = empty / already gotten\n// 1-7 = normal cards\n// 8-14 = dragons\n//---------------------------\n// 1 = Earth\n// 2 = Fire\n// 3 = Ice\n// 4 = Light\n// 5 = Nether\n// 6 = Poison\n// 7 = Shadow\n//---------------------------\n// 8 = Earth Dragon\n// 9 = Fire Dragon\n//10 = Ice Dragon\n//11 = Light Dragon\n//12 = Nether Dragon\n//13 = Poison Dragon\n//14 = Shadow Dragon\n//---------------------------")]
		public bool[] filter_mSC = new bool[15];
		public float amount_mSC = 3;

		[Space]

		public bool multDragonChance = true;
		[Tooltip("//--------CARD TYPES---------\n// 0 = empty / already gotten\n// 1-7 = normal cards\n// 8-14 = dragons\n//---------------------------\n// 1 = Earth\n// 2 = Fire\n// 3 = Ice\n// 4 = Light\n// 5 = Nether\n// 6 = Poison\n// 7 = Shadow\n//---------------------------\n// 8 = Earth Dragon\n// 9 = Fire Dragon\n//10 = Ice Dragon\n//11 = Light Dragon\n//12 = Nether Dragon\n//13 = Poison Dragon\n//14 = Shadow Dragon\n//---------------------------")]
		public bool[] filter_mDC = new bool[15];
		public float amount_mDC = 3;

		[Space]

		public bool multScoreGained = true;
		[Tooltip("//--------CARD TYPES---------\n// 0 = empty / already gotten\n// 1-7 = normal cards\n// 8-14 = dragons\n//---------------------------\n// 1 = Earth\n// 2 = Fire\n// 3 = Ice\n// 4 = Light\n// 5 = Nether\n// 6 = Poison\n// 7 = Shadow\n//---------------------------\n// 8 = Earth Dragon\n// 9 = Fire Dragon\n//10 = Ice Dragon\n//11 = Light Dragon\n//12 = Nether Dragon\n//13 = Poison Dragon\n//14 = Shadow Dragon\n//---------------------------")]
		public bool[] filter_mSG = new bool[15];
		public int amount_mSG = 3;

	

		public BasicObjectives(){
			for(int i = 0; i<15;i++){
				filter_mSC[i] = true;
				filter_mDC[i] = true;
				filter_mSG[i] = true;
			}
		}
	}

	[System.Serializable]
	public class AdvancedObjective {

		public IObjective script;

		public Vector2 activeTime = new Vector2(15f,35f);

	}

	public interface IObjective {

		void Activate();

		void Disable();
	}*/
}

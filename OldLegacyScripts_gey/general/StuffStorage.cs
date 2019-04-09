using UnityEngine;
using System.Collections;

public class StuffStorage : MonoBehaviour {


	public static StuffStorage s;
	public GameObject[] players = new GameObject[4];
	[HideInInspector]
	public ScoreBoardManager_LEGACY sbManager;

	// Use this for initialization
	void Awake () {
		s = this;
		sbManager = GameObject.FindObjectOfType<ScoreBoardManager_LEGACY> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void AddPlayer (GameObject player) {
		print ("Added Player = "+player);
		AddToArray (ref players, player);
		sbManager.SpawnScorePanels ();
	}


	void AddToArray (ref GameObject[] array, GameObject toAdd){
		if (array [array.Length - 1] != null) {
			var temp = new GameObject[array.Length];
			array.CopyTo (temp, 0);

			array = new GameObject[array.Length+1];

			temp.CopyTo (array,0);

			array [array.Length - 1] = toAdd;

		} else {
			int i = 0;
			while (array [i] != null) {
				i++;
			}
			array [i] = toAdd;
		}

	}
}

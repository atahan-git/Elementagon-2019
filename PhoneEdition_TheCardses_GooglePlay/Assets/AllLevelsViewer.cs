using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllLevelsViewer : MonoBehaviour {

	public GameObject levelHolder;
	// Start is called before the first frame update
	void Start () {
		foreach (GameSettings gs in GS.s.allModes) {
			Instantiate (levelHolder, transform).GetComponent<LevelHolder> ().mySettings = gs;
		}
	}

}

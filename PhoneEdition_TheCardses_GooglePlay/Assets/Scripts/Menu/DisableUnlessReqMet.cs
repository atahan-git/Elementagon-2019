using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableUnlessReqMet : MonoBehaviour {

	public GameSettings reqLevel;

	// Use this for initialization
	void Start () {
		if (reqLevel != null) {
			if (!SaveMaster.isLevelDone (reqLevel)) {
				gameObject.SetActive (false);
			}
		}
	}
}

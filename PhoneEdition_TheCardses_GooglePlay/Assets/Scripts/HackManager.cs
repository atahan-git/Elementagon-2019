using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.H)) {
			for (int i = 0; i < 4; i++) {
				for (int m = 0; m < 15; m++) {
					ScoreBoardManager.s.AddScore (i, m, 1, false);
				}
			}
		}
	}
}

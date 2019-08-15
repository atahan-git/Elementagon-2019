using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pauser : MonoBehaviour {

	public static Pauser s;

	public GameObject pauseObj;

	void Start (){
		s = this;
		pauseObj.SetActive (false);
	}

	bool pauseState = false;
	bool[] oldStates = new bool[5];
	public void Pause (){
		Pause (true);
	}

	public void Pause (bool showGfx) {
		pauseState = !pauseState;

		if (pauseState) {
			if (showGfx)
				pauseObj.SetActive (true);
			Time.timeScale = 0;
			oldStates[0] = LocalPlayerController.isActive;
			LocalPlayerController.isActive = false;
			oldStates[1] = PowerUpManager.s.canActivatePowerUp;
			PowerUpManager.s.canActivatePowerUp = false;
		} else {
			pauseObj.SetActive (false);
			Time.timeScale = 1;
			LocalPlayerController.isActive = oldStates[0];
			PowerUpManager.s.canActivatePowerUp = oldStates[1];
		}
	}
}

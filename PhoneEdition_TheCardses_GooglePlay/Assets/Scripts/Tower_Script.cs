using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Script: MonoBehaviour {

	public Transform startPoint;
	public Transform endPoint;

	void Start (){
		CardHandler.s.gameObject.transform.position = startPoint.position;
	}

	// Update is called once per frame
	void Update () {
		CardHandler.s.gameObject.transform.position = Vector3.MoveTowards (CardHandler.s.gameObject.transform.position, endPoint.position, 1f * Time.deltaTime);
		if (Vector3.Distance (CardHandler.s.gameObject.transform.position, endPoint.position) < 0.5f) {
			EndGame ();
		}
	}


	void EndGame (){
		LocalPlayerController.s.canSelect = false;
		PowerUpManager.s.canActivatePowerUp = false;
		GameEndScreen.s.Endgame ("The Tower", false);
	}
}

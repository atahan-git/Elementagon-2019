using UnityEngine;
using System.Collections;

public class CoolIceEnabler : MonoBehaviour {

	public static CoolIceEnabler s;

	public bool isEnabled = false;

	FrostEffect frost;

	public float onSpeed = 2f;
	public float offSpeed = 1.5f;

	// Use this for initialization
	void Start () {
		s = this;
		frost = GetComponent<FrostEffect> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isEnabled) {
			frost.FrostAmount = Mathf.Lerp (frost.FrostAmount, 1f, onSpeed * Time.deltaTime);
		} else {
			frost.FrostAmount = Mathf.Lerp (frost.FrostAmount, 0f, offSpeed * Time.deltaTime);
		}
	}

	public void SetIceState (bool state){

		isEnabled = state;
	}
}

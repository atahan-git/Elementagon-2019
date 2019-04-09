using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownPanelReferenceHolder : MonoBehaviour {

	public static DownPanelReferenceHolder s;

	public GameObject power;
	public GameObject items;
	public GameObject timer;
	public GameObject objective;
	public GameObject pause;
	public GameObject score;
	public GameObject backToMenu;

	private void Awake () {
		s = this;
	}
}

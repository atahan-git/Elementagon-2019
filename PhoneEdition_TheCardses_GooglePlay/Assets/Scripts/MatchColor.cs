using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchColor : MonoBehaviour {

	public SpriteRenderer master;
	SpriteRenderer me;
	// Use this for initialization
	void Start () {
		me = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		me.color = master.color;
	}
}

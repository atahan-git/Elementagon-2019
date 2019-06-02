using UnityEngine;
using System.Collections;

public class PlayerEffectStorage : MonoBehaviour {

	public static GameObject[] playerEffects;

	public GameObject[] effects = new GameObject[4];

	// Use this for initialization
	void Start () {
		playerEffects = effects;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

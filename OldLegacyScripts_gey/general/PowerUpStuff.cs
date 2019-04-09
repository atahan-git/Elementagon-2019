using UnityEngine;
using System.Collections;

public class PowerUpStuff : MonoBehaviour {

	public static PowerUpStuff s;

	[HideInInspector]
	public Transform activeEffectParent;

	[Space]

	public float lightTime = 10f;
	public GameObject LightEffect;

	[Space]

	public float shadowTime = 10f;
	public GameObject ShadowEffect;
	public GameObject ShadowSelectEffect;
	//public float shadowMultiplier = 2f;

	[Space]

	public int poisonAmount = 5;
	public GameObject PoisonActiveEffect;
	public GameObject PoisonOthersEffect;
	public GameObject TrapCardActivationEffect;
	public GameObject TrapCardEffect;
	public GameObject TrapCardExplosionEffect;

	[Space]

	public float iceTime = 5f;
	public GameObject IceSource;
	public GameObject IceFrozenGuys;

	[Space]

	public float earthTime = 5f;
	public GameObject EarthEffect;
	public GameObject EarthSelectEffect;

	[Space]

	public GameObject FireActiveEffect;
	public GameObject FireOthersEffect;
	public GameObject FireEffect;

	[Space]

	public float NetherReRotateTime = 0.5f;
	public GameObject NetherEffect;
	public GameObject NetherGetEffect;

	// Use this for initialization
	void Awake () {
		s = this;
		activeEffectParent = GameObject.Find ("ActiveEffectParent").transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

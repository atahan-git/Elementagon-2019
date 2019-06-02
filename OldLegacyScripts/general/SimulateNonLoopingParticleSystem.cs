using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class SimulateNonLoopingParticleSystem : MonoBehaviour {

	public bool simulate = true;
	public float time = 1f; 

	ParticleSystem particle;

	// Use this for initialization
	void Start () {
		particle = GetComponent<ParticleSystem> ();
		StartCoroutine (Reset());
	}

	float oldTime;
	bool oldBool;
	// Update is called once per frame
	void Update () {
		/*if (oldTime != time || oldBool != simulate) {
			StopCoroutine (Reset());
			if (simulate) {
				StartCoroutine (Reset);
			}
		}


		oldTime = time;
		oldBool = simulate;*/
	}


	IEnumerator Reset () {
		while (simulate) {
			//particle.Simulate (0.0f, true, true);
			particle.Stop();
			particle.Clear ();
			particle.Play ();
			//print ("this called");
			yield return new WaitForSeconds (time);

		}
	}
}

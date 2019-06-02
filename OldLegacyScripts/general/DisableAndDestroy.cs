using UnityEngine;
using System.Collections;

public class DisableAndDestroy : MonoBehaviour {

	public float destroyTime = 2f;

	public void Engage () {

		ParticleSystem[] myParticles = GetComponentsInChildren<ParticleSystem> ();
		LightBeamsControlScript[] myLights = GetComponentsInChildren<LightBeamsControlScript> ();
		GrowAtCreation[] myGrows = GetComponentsInChildren<GrowAtCreation> ();
		SpriteFade[] myFades = GetComponentsInChildren<SpriteFade> ();

		foreach (ParticleSystem sys in myParticles) {
			if (sys != null)
				sys.Stop ();
		}

		foreach (LightBeamsControlScript lig in myLights) {
			if (lig != null)
				lig.Stop ();
		}

		foreach (GrowAtCreation gro in myGrows) {
			if (gro != null)
				gro.GrowDestruct (gro.growTime / 2f);
		}

		foreach (SpriteFade fad in myFades) {
			if (fad != null)
				fad.FadeOut ();
		}


		if (GetComponent<Animator> () != null)
			GetComponent<Animator> ().SetTrigger ("Destroy");

		Destroy (gameObject, destroyTime);

	}
}

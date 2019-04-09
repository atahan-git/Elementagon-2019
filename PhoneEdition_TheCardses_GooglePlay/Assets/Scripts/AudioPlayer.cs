using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {


	public AudioClip openSound;
	public AudioClip closeSound;
	public AudioClip matchSound;

	AudioSource aud;
	// Use this for initialization
	void Awake () {
		aud = GetComponent<AudioSource> ();
		if (!aud)
			this.enabled = false;
	}


	public void OpenSound (){
		PlaySound (openSound);
	}

	public void CloseSound (){
		PlaySound (closeSound);
	}

	public void MatchSound (){
		PlaySound (matchSound);
	}

	void PlaySound (AudioClip clip){
		aud.Stop ();
		aud.clip = clip;
		aud.Play ();
	}

}

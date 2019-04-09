using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour {

	string myName;

	public AudioMixerGroup myGroup;

	Slider mySlider;
	public float volume = 0;

	// Use this for initialization
	void Start () {
		mySlider = GetComponentInChildren<Slider> ();

		myName = myGroup.name;
		volume = PlayerPrefs.GetFloat (myName, 0f);
		mySlider.value = volume;

		myGroup.audioMixer.SetFloat (myName, volume);
	}

	public void OnSliderChange () {
		volume = mySlider.value;
		PlayerPrefs.SetFloat (myName, volume);
		myGroup.audioMixer.SetFloat (myGroup.name, volume);
	}
}

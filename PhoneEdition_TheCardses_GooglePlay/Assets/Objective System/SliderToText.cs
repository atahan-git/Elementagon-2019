using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class SliderToText : MonoBehaviour {

	public TextMeshProUGUI text;
	public Slider slider;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		text.text = ((int)slider.value).ToString () + "/" + slider.maxValue.ToString ();
	
	}
}

using UnityEngine;
using System.Collections;

public class LerpColors : MonoBehaviour {

	public Color color1;
	public Color color2;
	public Color curCol;

	bool lastColor = true;
	Color colorToGo;

	Renderer rend;

	float lerpTime = 0.8f;
	float lerpSpeed = 2f;

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer> ();
		InvokeRepeating ("ChangeColor", 0f, lerpTime);
	}

	// Update is called once per frame
	void Update () {
		rend.material.color = Color.Lerp (rend.material.color, colorToGo, lerpSpeed * Time.deltaTime);
		curCol = rend.material.color;
	}

	void ChangeColor(){
		if (lastColor) {
			lastColor = false;
			colorToGo = color1;
		} else {
			lastColor = true;
			colorToGo = color2;
		}
	}
}

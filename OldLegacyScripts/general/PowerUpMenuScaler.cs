using UnityEngine;
using System.Collections;

public class PowerUpMenuScaler : MonoBehaviour {

	RectTransform rectTransform;

	// Use this for initialization
	void Start () {
		rectTransform = GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () {
		rectTransform.sizeDelta = new Vector2 (rectTransform.sizeDelta.x, Mathf.Clamp (transform.childCount * 80f, rectTransform.parent.GetComponent<RectTransform>().sizeDelta.y, Mathf.Infinity));
	}
}

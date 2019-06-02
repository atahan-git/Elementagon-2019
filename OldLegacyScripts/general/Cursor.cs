using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour {


	public Transform goToPos;

	public float size;

	public GameObject leftPart;
	public GameObject righPart;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp (transform.position, goToPos.position, 20 * Time.deltaTime);
		leftPart.transform.localPosition = new Vector3 (Mathf.Lerp(leftPart.transform.localPosition.x,  size, 20f * Time.deltaTime), 0, 0);
		righPart.transform.localPosition = new Vector3 (Mathf.Lerp(righPart.transform.localPosition.x, -size, 20f * Time.deltaTime), 0, 0);
	}

	public void instaGo () {
		transform.position = goToPos.position;
	}
}

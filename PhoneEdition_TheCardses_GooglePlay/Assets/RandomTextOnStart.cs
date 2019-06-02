using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTextOnStart : MonoBehaviour {

	public string[] possibilities = new string[5];
	// Start is called before the first frame update
	void Start () {
		GetComponent<TMPro.TextMeshPro> ().text = possibilities[Random.Range (0, possibilities.Length)];
	}
}

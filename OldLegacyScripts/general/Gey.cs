using UnityEngine;
using System.Collections;

public class Gey : MonoBehaviour {


	public GameObject[] scoreBoards = new GameObject[1];

	public GameObject one;
	public GameObject two;
	public GameObject three;

	// Use this for initialization
	void Start () {
		AddToArray (ref scoreBoards, one);
		AddToArray (ref scoreBoards, three);
		AddToArray (ref scoreBoards, two);

		foreach (GameObject gam in scoreBoards) {

			print (gam);
		}
		//scoreBoards = new GameObject[5];
	}
	// Update is called once per frame
	void Update () {
	
	}

	void AddToArray (ref GameObject[] array, GameObject toAdd){
		if (array [array.Length - 1] != null) {
			var temp = new GameObject[array.Length];
			array.CopyTo (temp, 0);

			array = new GameObject[array.Length+1];

			temp.CopyTo (array,0);

			array [array.Length - 1] = toAdd;

		} else {
			int i = 0;
			while (array [i] != null) {
				i++;
			}
			array [i] = toAdd;
		}

	}
}

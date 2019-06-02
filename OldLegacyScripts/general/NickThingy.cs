using UnityEngine;
using System.Collections;

public class NickThingy : MonoBehaviour {


	public string[] chunks;

	int minLength = 3;
	int maxLength = 6;

	int amount = 20;
	// Use this for initialization
	void Start () {
		/*for (int i = 0; i <= 20; i++) {
			int length = Random.Range (minLength, maxLength);
			string myAmazingNick = "";
			for (int m = 0; m < length; m++) {
				myAmazingNick += chunks[Random.Range(0, chunks.Length)];
			}
			print (myAmazingNick);
		}*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

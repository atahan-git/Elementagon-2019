using UnityEngine;
using System.Collections;

public class AllCardSprites : MonoBehaviour {

	public Sprite back;
	//public Sprite defaultOne;
	public Sprite[] fronts = new Sprite[15];

	public Sprite trapcard;

	public static AllCardSprites s;

	void Awake(){
		s = this;
	}
}

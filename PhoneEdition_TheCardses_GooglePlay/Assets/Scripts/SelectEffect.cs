using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SelectEffect : MonoBehaviour {

	public int id;

	public void DestroyEffect () {
		Destroy (gameObject);
	}

}

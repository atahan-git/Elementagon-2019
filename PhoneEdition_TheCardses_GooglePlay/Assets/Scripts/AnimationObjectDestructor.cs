using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationObjectDestructor : MonoBehaviour {

	void DestroyObj (){
		Destroy (gameObject.transform.parent.gameObject);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMenu : MonoBehaviour {

	public 	void Back (){
		SceneMaster.s.LoadMenu ();
	}

	bool shouldDo = true;

	void Start (){
		Invoke ("Stop",1f);
	}

	void Update (){
		if (shouldDo)
			transform.SetAsLastSibling ();
	}

	void Stop (){
		shouldDo = false;
	}
}

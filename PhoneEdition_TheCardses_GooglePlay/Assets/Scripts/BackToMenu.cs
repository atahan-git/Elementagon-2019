using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMenu : MonoBehaviour {

	public void Back (){
		//make us lose the game
		GameObjectiveFinishChecker.s.EndGame (DataHandler.s.myPlayerInteger == 0 || DataHandler.s.myPlayerInteger == 1 ? 5 : 4);
		//SceneMaster.s.LoadMenu ();
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

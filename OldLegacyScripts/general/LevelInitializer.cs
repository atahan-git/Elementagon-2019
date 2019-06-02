using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelInitializer : MonoBehaviour {

	//functionality moved on to the GameStarter

	// Use this for initialization
	void OnLevelWasLoaded () {
		return;

		if (GoogleAPI.s != GetComponent<GoogleAPI> ())
			return;
		if (SceneMaster.GetSceneID () != 3)
			return;

		print ("onlevelwasloaded");

		GameObject bg = GameObject.Find ("BG");
		if (bg != null) {
			if (GS.a.bgSprite != null) {
				bg.GetComponentInChildren<Image> ().sprite = GS.a.bgSprite;
			}
			if (GS.a.bgAnimation != null) {
				bg.GetComponentInChildren<AnimatedSpriteController> ().SetAnimation(GS.a.bgAnimation);
			}
		}

		try{
			GameObject.Find ("Daragon Text").GetComponent<Text> ().text = GS.a.startingText;
			GameObject.Find ("Daragon Image").GetComponent<Image> ().sprite = GS.a.startingImage;
		}catch{
		}

		/*if (GS.a.npcSettings.isNPC) {
			GameObject logic = GameObject.Find ("_Logic");
			NPCBehaviour toAdd = logic.AddComponent (GS.a.npcSettings.npcScript.GetType()) as NPCBehaviour;
			logic.GetComponent<GameStarter> ().toCall [0] = toAdd;
			toAdd.SelectorObject = GS.a.npcSettings.SelectorObject;
			toAdd.activatePrefab = GS.a.npcSettings.activatePrefab;
			toAdd.normalSelectEffect = GS.a.npcSettings.normalSelectEffect;
		}*/
		/*if (GS.a.npcSettings.isNPC) {
			GameObject mynpc = (GameObject)Instantiate (GS.a.npcSettings.npcObject);
			mynpc.GetComponent<_NPCBehaviour> ().enabled = false;

			GameObject.Find ("_Logic").GetComponent<GameStarter> ().toCall [0] = mynpc.GetComponent<_NPCBehaviour> ();
		}	*/
		if (GS.a.customObject != null)
			Instantiate (GS.a.customObject);

		Invoke ("LateBegin",0.1f);
	}

	void LateBegin (){
		/*for (int i = 0; i < GS.a.startingHand.Length; i++) {
			ScoreBoardManager.s.AddScore (DataHandler.s.myPlayerInteger, i, GS.a.startingHand [i], false);
		}*/
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

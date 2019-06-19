using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelOnStart : MonoBehaviour {

	public int lvlId = 1;
	// Start is called before the first frame update
	void Start () {
		Debug.Log ("Loading scene");
		SceneManager.LoadSceneAsync (lvlId);
	}
}

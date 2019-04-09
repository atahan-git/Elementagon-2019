using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ScoreBoardManager_LEGACY : NetworkBehaviour {

	public static ScoreBoardManager_LEGACY s;

	public GameObject prefab;

	public GameObject[] scoreBoards = new GameObject[8];

	public int myPlayer = -1;
	public PlayerScript myPlayerScript;

	public GameObject dragonPrefab;
	public GameObject[] dragonBoards = new GameObject[7];
	GameObject parentDragon;

	// Use this for initialization
	void Awake () {
		s = this;
		parentDragon = GameObject.Find ("ParentDragon");
		//scoreBoards = new GameObject[5];
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	public void SpawnScorePanels () {

		for (int i = 0; i < StuffStorage.s.players.Length; i++) {
			if (StuffStorage.s.players [i] != null) {
				if (scoreBoards.Length <= i) {

					SpawnOnePanel (i);
						
				} else if (scoreBoards [i] == null) {

					SpawnOnePanel (i);
				}
			}
		}
	}


	void SpawnOnePanel (int id) {

		print ("spawning one panel with Id - " + id);

		RectTransform panelParent = GameObject.Find ("LeftPanel").GetComponent<RectTransform>();
		GameObject scorePanel = (GameObject)Instantiate (prefab, panelParent.position, panelParent.rotation);
		/*scorePanel.GetComponent<RectTransform> ().parent = panelParent;
		scorePanel.GetComponent<RectTransform> ().localScale = panelParent.localScale;*/
		//AddToArray(ref scoreBoards, scorePanel);
		scoreBoards[id] = scorePanel;
		NetworkServer.Spawn (scorePanel);
		//RpcSetLocation (scorePanel);

		//TargetChangeLocation (StuffStorage.s.players[id].GetComponent<NetworkIdentity>().connectionToClient, scorePanel);

		/*if (isLocalPlayer) {
			scorePanel.GetComponent<LayoutElement> ().minHeight = bigSize;
			scorePanel.GetComponent<RectTransform> ().SetAsLastSibling ();
			GameObject.Find ("PowerUps").GetComponent<RectTransform>().SetAsFirstSibling();
			scorePanel.gameObject.name = "Score Panel Main Player";
		} else {
			GameObject mainPanel = GameObject.Find ("Score Panel Main Player");
			if(mainPanel != null)
				mainPanel.GetComponent<RectTransform> ().SetAsLastSibling ();
			GameObject.Find ("PowerUps").GetComponent<RectTransform>().SetAsFirstSibling();
		}*/

		scorePanel.GetComponent<ScorePanel> ().playerid = id;
		//print ("Panel Spawned With Id = "+scorePanel.GetComponent<ScorePanel> ().playerid);

	}

	public void UpdateScores () {
		int n = 0;
		foreach (GameObject gam in scoreBoards) {
			if (gam != null) {
				int myScore = 0;

				for (int i = 1; i < 8; i++) {

					myScore += ScoreKeeper.s.players[n].Scores [i];
				}

				gam.GetComponent<ScorePanel> ().scoreVal = myScore;
				n++;
			}
		}


	}

	[ClientRpc]
	public void RpcCheckAddDragon (int playerId, int DragonType, int DragonAmmount) {
		if (playerId == myPlayer) {
			int i = DragonType - 8;
			if (DragonAmmount > 0) {
				if (dragonBoards [i] != null) {
					dragonBoards [i].GetComponent<DragonPanel> ().count = DragonAmmount;
					//dragonBoards [i].GetComponent<DragonPanel> ().SetType ();
				} else {
					dragonBoards [i] = (GameObject)Instantiate (dragonPrefab, parentDragon.transform);
					dragonBoards [i].transform.localScale = new Vector3 (1, 1, 1);
					dragonBoards [i].GetComponent<DragonPanel> ().count = DragonAmmount;
					dragonBoards [i].GetComponent<DragonPanel> ().type = i + 8;
					//dragonBoards [i].GetComponent<DragonPanel> ().SetType ();
					int type = i + 8;
					dragonBoards [i].GetComponentInChildren<Button> ().onClick.AddListener (delegate {
						myPlayerScript.UsePowerUp (type);
					});
				}

			} else {
				if (dragonBoards [i] != null) {
					Destroy (dragonBoards [i].gameObject);
					dragonBoards [i] = null;
				}
			}
		}
	}

	/*[ClientRpc]
	void RpcSetLocation (GameObject scorePanel){

		RectTransform panelParent = GameObject.Find ("LeftPanel").GetComponent<RectTransform>();

		scorePanel.GetComponent<RectTransform> ().parent = panelParent;
		scorePanel.GetComponent<RectTransform> ().localScale = panelParent.localScale;


		print ("Client Set Location");
	}

	float bigSize = 65f;

	[TargetRpc]
	void TargetChangeLocation (NetworkConnection target, GameObject scorePanel) {

		print ("Target Set Location");

		scorePanel.GetComponent<LayoutElement> ().minHeight = bigSize;
		scorePanel.GetComponent<RectTransform> ().SetAsLastSibling ();
		GameObject.Find ("PowerUps").GetComponent<RectTransform>().SetAsFirstSibling();
		scorePanel.gameObject.name = "Score Panel Main Player";

	}*/


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

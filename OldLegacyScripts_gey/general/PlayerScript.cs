using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerScript : NetworkBehaviour {

	//public PowerUpDealer.PowerUpTypes curPowerUp = PowerUpDealer.PowerUpTypes.noUp;

	[SyncVar]
	public int id = -1;

	[HideInInspector]
	public PowerUpDealer powerUp;
	[HideInInspector]
	public ComboDealer comboD;

	float checkSpeed = 0.35f;

	[HideInInspector]
	public CardGenerator cardGen;

	//[HideInInspector]
	public CardScript[] rotatedCards = new CardScript[2];
	//[HideInInspector]
	public GameObject[] playerEffectMem = new GameObject[2];

	public bool isActive = true;
	public bool canMove = true;
	public bool canSelect = true;
	public bool isPowerUp = false;

	public GameObject playerEffect;
	public GameObject ScorePanel;
	float bigSize = 65f;

	void Start () {
		comboD = GetComponent<ComboDealer> ();
		powerUp = GetComponent<PowerUpDealer> ();


		/*print (Network.player);
		print(Network.player.guid);*/
		cardGen = CardGenerator.s;

		//CmdSpawnScorePanel ();
		if (isLocalPlayer) {
			gameObject.name = "Local Player";
			CmdSetUpPlayer ();
			if (isServer) {
				id = 0;
				print ("We are the Host = " + id);
				playerEffect = PlayerEffectStorage.playerEffects [id];

				//ScoreBoardManager_LEGACY.s.myPlayer = id;
				//ScoreBoardManager_LEGACY.s.myPlayerScript = this;
				//print (connectionToClient.connectionId);
				//SpawnPanel ();
				//NetworkServer.Spawn (ScorePanel);
				Invoke("BitOfLag",0.1f);	
			}
		} else {

			//gameObject.SetActive (false);
		}
		/*else if (id >= 0) {
			SpawnPanel ();
		} else {
			Invoke ("ReCheck", 0.5f);
		}*/


	}

	void ReCheck () {
		if (id >= 0) {
			SpawnPanel ();
		} else {
			Invoke ("ReCheck", 0.5f);
		}
	}
		

	[Command]
	void CmdSetUpPlayer () {

		StuffStorage.s.AddPlayer (gameObject);
		RpcGetId (connectionToClient.connectionId);

	}

	[ClientRpc]
	void RpcGetId (int theId) {

		id = theId;
		print ("My Player Id = " + id);
		playerEffect = PlayerEffectStorage.playerEffects [id];
		if (isLocalPlayer) {
			print ("Got Local Player Id = " + id);
			//ScoreBoardManager_LEGACY.s.myPlayer = id;
			//ScoreBoardManager_LEGACY.s.myPlayerScript = this;
		}
		//print (connectionToClient.connectionId);
		//SpawnPanel ();
		//NetworkServer.Spawn (ScorePanel);
		Invoke("BitOfLag",0.1f);
	}

	void BitOfLag () {

		GameObject.Find("LeftPanel").BroadcastMessage("CheckYourself");
	}

	void SpawnPanel () {
		if (ScorePanel.GetComponent<ScorePanel> ().playerid != -1)
			return;

		print (id);
		RectTransform panelParent = GameObject.Find ("LeftPanel").GetComponent<RectTransform>();
		ScorePanel = (GameObject)Instantiate (ScorePanel, panelParent.position, panelParent.rotation);
		ScorePanel.GetComponent<RectTransform> ().parent = panelParent;
		ScorePanel.GetComponent<RectTransform> ().localScale = panelParent.localScale;

		if (isLocalPlayer) {
			ScorePanel.GetComponent<LayoutElement> ().minHeight = bigSize;
			ScorePanel.GetComponent<RectTransform> ().SetAsLastSibling ();
			GameObject.Find ("PowerUps").GetComponent<RectTransform>().SetAsFirstSibling();
			ScorePanel.gameObject.name = "Score Panel Main Player";
		} else {
			GameObject mainPanel = GameObject.Find ("Score Panel Main Player");
			if(mainPanel != null)
				mainPanel.GetComponent<RectTransform> ().SetAsLastSibling ();
			GameObject.Find ("PowerUps").GetComponent<RectTransform>().SetAsFirstSibling();
		}

		ScorePanel.GetComponent<ScorePanel> ().playerid = id;
		print (ScorePanel.GetComponent<ScorePanel> ().playerid);
	}
		

	void Update () {
		if (!isLocalPlayer)
			return;

		if (Input.GetMouseButtonDown (0) && canSelect && Input.mousePosition.x > Screen.width/6.6f) {

			if (id < 0) {
				gameObject.name = "Local Player";
				CmdSetUpPlayer ();
			} 

			RaycastHit myHet = new RaycastHit ();

			if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out myHet)){

				//print (myHet.collider.transform.parent.gameObject.name);

				CardScript myCardS = myHet.collider.gameObject.GetComponentInParent<CardScript> ();

				if (myCardS != null) {

					if (!isPowerUp) {
						canSelect = false;
						CmdSelectCard (myCardS.gameObject, true, gameObject);
					} else {
						CmdSetPowerUpSelect (myCardS.gameObject);
						//print ("Set power up select " + myCardS.gameObject);
					}
					//if(rotatedCards[1] != null)
						
				}
			}
		}
	}

	[Command]
	void CmdSetPowerUpSelect (GameObject card){
		powerUp.powerUpSelect = card.GetComponent<CardScript>();
		//print ("Setted it " + powerUp.powerUpSelect);
	}


	[Command]
	void CmdSelectCard (GameObject card,bool state, GameObject client){

		CardScript myCardS = card.GetComponent<CardScript> ();

		if (myCardS.cardType == 0) {
			RpcLetHimReRotate ();
			return;
		}
		if (myCardS.isSelected) {
			RpcLetHimReRotate();
			return;
		}

		if(myCardS.isPoison){
			powerUp.PoisonSelect (card);
			return;
		}


		if (state) {
			
			/*myCardS.RotateSelf ();
			myCardS.isSelected = true;*/
			RpcRotateNSelectCard (card, true);

			GameObject temp = (GameObject)Instantiate (playerEffect, myCardS.transform.position, Quaternion.identity);
			NetworkServer.SpawnWithClientAuthority (temp, client);

			if (playerEffectMem [0] == null) {
				playerEffectMem [0] = temp;
			} else {
				playerEffectMem [1] = temp;
			}


			if (rotatedCards [0] == null) {
				rotatedCards [0] = myCardS;
				RpcLetHimReRotate ();
			} else {
				rotatedCards [1] = myCardS;
				Invoke ("CheckCards", checkSpeed);
			}

		} else {

		}
	}

	[ClientRpc]
	void RpcRotateNSelectCard (GameObject card,bool state){
		CardScript myCardS = card.GetComponent<CardScript> ();
		//myCardS.SetDirtyBitses ();
		if (state) {
			myCardS.SetState (true);
			myCardS.isSelected = true;
		} else {
			myCardS.SetState (false);
			myCardS.isSelected = false;
		}

	}

	[ClientRpc]
	public void RpcLetHimReRotate () {
		canSelect = true;
	}

	void CheckCards(){
		CmdCheckMatch (rotatedCards[0].gameObject, rotatedCards[1].gameObject);
		RpcLetHimReRotate ();
	}


	[Command]
	void CmdCheckMatch (GameObject first, GameObject second){

		foreach (GameObject gam in playerEffectMem) {
			if (gam != null)
				Destroy (gam.gameObject);
		}
			

		//got them correct
		if (rotatedCards [0].cardType == rotatedCards [1].cardType) {
			int myCardType = rotatedCards [0].cardType;

			rotatedCards [0].SetType(0);	
			rotatedCards [0].isSelected = false;
			rotatedCards [0] = null;
			rotatedCards [1].SetType(0);
			rotatedCards [1].isSelected = false;
			rotatedCards [1] = null;

			//ScoreKeeper.s.AddScore(id, myCardType, 1);
			//comboD.AddScoreWithComboCheck (connectionToClient.connectionId, myCardType);

		} else {
			/*rotatedCards [0].RotateSelf ();
			rotatedCards [0].isSelected = false;*/
			RpcRotateNSelectCard (rotatedCards [0].gameObject, false);
			rotatedCards [0] = null;
			/*rotatedCards [1].RotateSelf ();
			rotatedCards [1].isSelected = false;*/
			RpcRotateNSelectCard (rotatedCards [1].gameObject, false);
			rotatedCards [1] = null;
		}
	}


	public void UsePowerUp (int type) {
		print ("use power up " + type);
		CmdUsePowerUp (type);
	}

	[Command]
	void CmdUsePowerUp (int type){
		//ScoreKeeper.s.AddScore (id, type, -1);
		powerUp.ActivatePowerUp (type);
	}

	/*

	/*void PressSelect(){

		CardScript myCardS = cardGen.allCards [(int)playerPos.x, (int)playerPos.y].GetComponent<CardScript> ();

		if (myCardS.cardType == 0)
			return;
		if (myCardS.isSelected)
			return;
		if (rotatedCards [1] != null)
			return;

		myCardS.RotateSelf ();
		myCardS.isSelected = true;

		if (powerUp.isShadowActive) {
			powerUp.ShadowSelect(myCardS);
			return;
		}

		GameObject temp = (GameObject)Instantiate (playerEffect, myCardS.transform.position, myCardS.transform.rotation);

		/*if(myCardS.cardType >= 8){
			StartCoroutine (DragonCheck(myCardS, temp));
			return;
		}

		if (playerEffectMem [0] == null) {
			playerEffectMem [0] = temp;
		} else {
			playerEffectMem [1] = temp;
		}
		if (powerUp.isEarthActive) {
			powerUp.EarthPlace ();
		}

		if (rotatedCards [0] == null) {
			rotatedCards [0] = myCardS;
			Invoke ("UnSelectOneCard", 10f);
			if (powerUp.isEarthActive) {
				powerUp.Invoke("EarthPreCheck", CheckSpeed);
			}
		} else {
			rotatedCards [1] = myCardS;
			CancelInvoke ("UnSelectOneCard");
			powerUp.CancelInvoke ("EarthPreCheck");
			Invoke ("CheckCards", CheckSpeed);
		}
	}

	/*IEnumerator DragonCheck(CardScript myCardS, GameObject temp){

		Instantiate (myCardS.getEffect, myCardS.transform.position, myCardS.transform.rotation);

		yield return new WaitForSeconds (CheckSpeed * 3);

		ScoreKeeper.s.AddScore(id, myCardS.cardType, 1);

		if (powerUp.isPoisonActive) {
			powerUp.PoisonDragon (myCardS.cardType);
		}

		myCardS.cardType = 0;	
		myCardS.isSelected = false;

		Destroy (temp);
	}

	void CheckCards(){

		foreach (GameObject gam in playerEffectMem) {
			if (gam != null)
				Destroy (gam.gameObject);
		}

		if (powerUp.isEarthActive) {
			powerUp.EarthCheck();
			return;
		}

		if (powerUp.isPoisonActive) {
			powerUp.PoisonCheck(rotatedCards);
			return;
		}

		//got them correct
		if (rotatedCards [0].cardType == rotatedCards [1].cardType) {
			int myCardType = rotatedCards [0].cardType;

			rotatedCards [0].cardType = 0;	
			rotatedCards [0].isSelected = false;
			rotatedCards [0] = null;
			rotatedCards [1].cardType = 0;
			rotatedCards [1].isSelected = false;
			rotatedCards [1] = null;

			//ScoreKeeper.s.AddScore(id, myCardType, 1);
			comboD.AddScoreWithComboCheck (id, myCardType);

		} else {
			rotatedCards [0].RotateSelf ();
			rotatedCards [0].isSelected = false;
			rotatedCards [0] = null;
			rotatedCards [1].RotateSelf ();
			rotatedCards [1].isSelected = false;
			rotatedCards [1] = null;
		}
	}
		

	Vector3 AddVectors(Vector3 vec1, Vector3 vec2){
		return new Vector3 (vec1.x + vec2.x, vec1.y + vec2.y, vec1.z + vec2.z);
	}

	void UnSelectOneCard(){
		rotatedCards [0].RotateSelf ();
		rotatedCards [0].isSelected = false;
		rotatedCards [0] = null;

		foreach (GameObject gam in playerEffectMem) {
			if (gam != null)
				Destroy (gam.gameObject);
		}
	}

	//old move script
	/*void Move(JToken data){
		
		if ((bool)data ["dpadrelative-left"] ["pressed"]) {
			//print ("got something5");

			switch ((string)data ["dpadrelative-left"] ["message"] ["direction"]) {
			case "right":
				playerPos = AddVectors (playerPos, new Vector3 (1, 0, 0));
				break;
			case "left":
				playerPos = AddVectors (playerPos, new Vector3 (-1, 0, 0));
				break;
			case "up":
				playerPos = AddVectors (playerPos, new Vector3 (0, 1, 0));
				break;
			case "down":
				playerPos = AddVectors (playerPos, new Vector3 (0, -1, 0));
				break;
			default:
				print ("error");
				break;
			}

		}
		playerPos = new Vector3 ((int)Mathf.Clamp (playerPos.x, 0, cardGen.gridSizeX - 1), (int)Mathf.Clamp (playerPos.y, 0, cardGen.gridSizeY - 1), 0);
	}*/

	//other old move
	/*void Move(JToken data){
		//print ("Moving");
		//start moving
		print(data);
		if ((bool)data ["dpadrelative-left"] ["pressed"]) {
			//print ("got something5");

			switch ((string)data ["dpadrelative-left"] ["message"] ["direction"]) {
			case "right":
				print ("Right");
				try {
					StopCoroutine (moveRoutine);
				} catch {}
				moveRoutine = StartCoroutine (MoveStep (new Vector3 (1, 0, 0), moveSpeed));
				break;
			case "left":
				print ("Left");
				try {
					StopCoroutine (moveRoutine);
				} catch {}
				moveRoutine = StartCoroutine (MoveStep (new Vector3 (-1, 0, 0), moveSpeed));
				break;
			case "up":
				print ("Up");
				try {
					StopCoroutine (moveRoutine);
				} catch {}
				moveRoutine = StartCoroutine (MoveStep (new Vector3 (0, 1, 0), moveSpeed));
				break;
			case "down":
				print ("Down");
				try {
					StopCoroutine (moveRoutine);
				} catch {}
				moveRoutine = StartCoroutine (MoveStep (new Vector3 (0, -1, 0), moveSpeed));
				break;
			default:
				print ("error");
				break;
			}

			// stop moving
		} else {
			print ("stopped moving");
			try {
				StopCoroutine (moveRoutine);
			} catch {}
		}

	}

	IEnumerator MoveStep(Vector3 moveAm, float repeatRate) {
		//print ("started to move");
		while(true) {
			playerPos = AddVectors (playerPos, moveAm);
			playerPos = new Vector3 ((int)Mathf.Clamp (playerPos.x, 0, cardGen.gridSizeX - 1), (int)Mathf.Clamp (playerPos.y, 0, cardGen.gridSizeY - 1), 0);
			//print (moveAm);
			yield return new WaitForSeconds(repeatRate);
		}
	}*/

	/*bool moveLock = false;

	void Move(JToken data){
		//print ("Moving");
		//start moving
		print(data);
		if ((bool)data ["joystick-left"] ["pressed"]) {
			//print ("got something5");

			float x = (float)data ["joystick-left"] ["message"] ["x"];
			float y = -(float)data ["joystick-left"] ["message"] ["y"];
			//is x or y is first
			if (Mathf.Abs (x) < JoystickDeadZone && Mathf.Abs (y) < JoystickDeadZone) {
				print ("stopped moving");
				try {
					StopCoroutine (moveRoutine);
				} catch {}
			}

			if (Mathf.Abs (x) >= Mathf.Abs (y)) {
				//is x big or small enough
				if (x > JoystickDeadZone) {
					print ("Right");
					try {
						StopCoroutine (moveRoutine);
					} catch {}
					moveRoutine = StartCoroutine (MoveStep (new Vector3 (1, 0, 0), moveSpeed));

				} else if (x < -JoystickDeadZone) {
					print ("Left");
					try {
						StopCoroutine (moveRoutine);
					} catch {}
					moveRoutine = StartCoroutine (MoveStep (new Vector3 (-1, 0, 0), moveSpeed));

				}
			} else {
				if (y > JoystickDeadZone) {
					print ("Up");
					try {
						StopCoroutine (moveRoutine);
					} catch {}
					moveRoutine = StartCoroutine (MoveStep (new Vector3 (0, 1, 0), moveSpeed));

				} else if (y < -JoystickDeadZone) {
					print ("Down");
					try {
						StopCoroutine (moveRoutine);
					} catch {}
					moveRoutine = StartCoroutine (MoveStep (new Vector3 (0, -1, 0), moveSpeed));

				}
			}



			// stop moving
		} else {
			print ("stopped moving");
			try {
				StopCoroutine (moveRoutine);
			} catch {}
		}

	}

	IEnumerator MoveStep(Vector3 moveAm, float repeatRate) {
		//print ("started to move");
		while (moveLock) {
			yield return 0;
		}
		while(true) {
			moveLock = true;
			Invoke ("EndLock", repeatRate);
			playerPos = AddVectors (playerPos, moveAm);
			playerPos = new Vector3 ((int)Mathf.Clamp (playerPos.x, 0, cardGen.gridSizeX - 1), (int)Mathf.Clamp (playerPos.y, 0, cardGen.gridSizeY - 1), 0);
			//print (moveAm);
			yield return new WaitForSeconds(repeatRate);
		}
	}

	void EndLock(){
		moveLock = false;
	}*/
}
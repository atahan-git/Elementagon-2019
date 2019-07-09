using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DataLogger : MonoBehaviour {

	public static bool isDebugMode = false;
	public static DataLogger s;

	public GameObject logTextPrefab;

	public Transform myMessageTextParent;
	public Transform myErrorTextParent;

	public int maxLogCount = 50;

	public GameObject [] stuffToToggleVisibility;
	public bool visibilityState = false;

	// Use this for initialization
	void Awake () {
		if (s != null && s != this) {
			Destroy (this.gameObject);
			return;
		} else {
			s = this;
		}

		LogMessage ("log initialized");
		LogMessage ("error log initialized");
		UpdateVersionText ();

		try {
			Application.logMessageReceived += ExceptionHandler;
		}catch(System.Exception e){
			DataLogger.LogMessage (e.Message + " - " + e.StackTrace);
		}

		if (PlayerPrefs.GetInt ("DebugVisibilityState", 0) == 1) {
			visibilityState = true;
		}
		if (PlayerPrefs.GetInt ("DebugCheats", 0) == 1) {
			isDebugMode = true;
		}

		SetLogstate (visibilityState);
		UpdateDebugMode ();

		/*CancelInvoke ("MessageLoggingTest");
		InvokeRepeating ("MessageLoggingTest", 0.5f, 0.2f);*/
	}

	void MessageLoggingTest () {
		LogMessage ("wowowoooooooooooooooooooooooooooooooooo " + UnityEngine.Random.Range (0, 1000).ToString());
	}

	float counter = 0f;

	bool isCardDefRotChanged = false;
	private void Update () {


		if (messageQueue.Count > 0) {
			GameObject myLogText = Instantiate (logTextPrefab, myMessageTextParent);
			myLogText.GetComponent<TMPro.TextMeshProUGUI> ().text = messageQueue.Dequeue ();
			myLogText.transform.SetAsFirstSibling ();

			LayoutRebuilder.ForceRebuildLayoutImmediate (myLogText.transform.parent.parent.parent.GetComponent<RectTransform> ());
			LayoutRebuilder.ForceRebuildLayoutImmediate (myLogText.GetComponent<RectTransform> ());

			if (myMessageTextParent.childCount > maxLogCount) {
				Destroy (myMessageTextParent.GetChild (myMessageTextParent.childCount - 1).gameObject);
			}
		}

		if (errorQueue.Count > 0) {
			GameObject myLogText = Instantiate (logTextPrefab, myErrorTextParent);
			myLogText.GetComponent<TMPro.TextMeshProUGUI> ().text = errorQueue.Dequeue ();
			myLogText.transform.SetAsFirstSibling ();

			LayoutRebuilder.ForceRebuildLayoutImmediate (myLogText.transform.parent.parent.parent.GetComponent<RectTransform> ());
			LayoutRebuilder.ForceRebuildLayoutImmediate (myLogText.GetComponent<RectTransform> ());

			if (myErrorTextParent.childCount > maxLogCount) {
				Destroy (myErrorTextParent.GetChild (myErrorTextParent.childCount-1).gameObject);
			}
		}



		//				CHEATS
		string cheatEnabledMessage = "##CHEAT ACTIVATED: ";
		if (isDebugMode) {
			if (Input.touchCount > 3 || Input.GetKeyDown (KeyCode.H)) {
				/*for (int i = 8; i <= 14; i++) {
					if (ScoreBoardManager.s != null)
						ScoreBoardManager.s.AddScore (DataHandler.s.myPlayerInteger, i, 1, false);
				}*/

				for (int i = 0; i < SaveMaster.s.mySave.levelsCompleted.Length; i++) {
					SaveMaster.s.mySave.levelsCompleted[i] = true;
				}

				LogMessage (cheatEnabledMessage + "all leves completed");
			}

			if (Input.touchCount > 4 || Input.GetKeyDown (KeyCode.F)) {
				if (ScoreBoardManager.s != null)
					ScoreBoardManager.s.AddScore (DataHandler.s.myPlayerInteger, 0, 30, false);

				LogMessage (cheatEnabledMessage + "Score added");
			}

			if (Input.GetKeyDown (KeyCode.I)) {
				CardTypeRandomizer.s.DebugPrintCardChances ();
			}

			if (Input.GetKeyDown (KeyCode.Y)) {
				CardHandler.s.RandomizeAllCards ();
				LogMessage (cheatEnabledMessage + "Cards randomized");
			}

			if (Input.GetKeyDown (KeyCode.U)) {
				isCardDefRotChanged = !isCardDefRotChanged;
				if (isCardDefRotChanged) 
					CardAnimator.closeRotation = Quaternion.Euler (0, 0, 0);
				 else 
					CardAnimator.closeRotation = Quaternion.Euler (0, 180, 0);

				foreach (IndividualCard card in CardHandler.s.allCards) {
					card.transform.rotation = CardAnimator.closeRotation;
				}
				LogMessage (cheatEnabledMessage + "Card Rot Set");
			}

			if (Input.GetKeyDown (KeyCode.S)) {
				if (SaveMaster.s != null)
					SaveMaster.s.Save ();

				LogMessage (cheatEnabledMessage + "saved");
			}

			if (Input.GetKeyDown (KeyCode.R)) {
				SaveMaster.ResetProgress ();

				LogMessage (cheatEnabledMessage + "progress reset");
			}

			if (Input.GetKeyDown (KeyCode.O)) {
				SaveMaster.HardReset ();

				LogMessage (cheatEnabledMessage + "hard reset");
			}

			if (Input.GetKeyDown (KeyCode.W)) {
				if (GameObjectiveFinishChecker.s != null)
					GameObjectiveFinishChecker.s.EndGame (DataHandler.s.myPlayerInteger);

				LogMessage (cheatEnabledMessage + "game won");
			}

			if (Input.GetKeyDown (KeyCode.L)) {
				if (GameObjectiveFinishChecker.s != null)
					GameObjectiveFinishChecker.s.EndGame (4);

				LogMessage (cheatEnabledMessage + "game lost");
			}

			if (Input.GetKeyDown (KeyCode.C)) {
				InventoryMaster.s.ClearInventory ();

				LogMessage (cheatEnabledMessage + "Inventory cleared");
			}
		}
	}


	static Queue<string> messageQueue = new Queue<string>();
	static List<string> messageBacklog = new List<string> ();

	public static void LogMessage (string log){
		log = Time.realtimeSinceStartup + " - " + log;
		messageQueue.Enqueue (log);
		messageBacklog.Add (log);
//#if UNITY_EDITOR
		Debug.Log (log);
//#endif
	}

	

	public static void LogError (string log, Exception e) {
		LogError (log +" - "+ e.ToString());
	}

	static Queue<string> errorQueue = new Queue<string>();
	static List<string> errorBacklog = new List<string> ();

	public static void LogError (string log) {
		log = Time.realtimeSinceStartup + " - " + log;
		errorQueue.Enqueue (log);
		errorBacklog.Add (log);
		//#if UNITY_EDITOR
		Debug.LogError (log);
//#endif
	}

	public Text version;
	public TextAsset versionText;
	void UpdateVersionText (){
		try{
		version.text = GetVersionNumber ();
		}catch{
			Invoke ("UpdateVersionText", 2f);
		}
	}

	string GetVersionNumber (){
		try{
			string content = versionText.text;

			if (content != null) {
				return content;
			} else {
				return " ";
			}
		}catch(System.Exception e){
			LogError ("Can't Get Version Number ",e);
		}
		return " ";
	}

	void ExceptionHandler (string condition, string stackTrace, LogType type) {
		if (type == LogType.Exception) {
#if !UNITY_EDITOR
			LogError (type.ToString () +" - "+ condition +" - " + stackTrace);
#endif
		}
	}


	public void Export () {
		System.DateTime dt = System.DateTime.Now;
		string datetime = dt.ToString ("yyyy-MM-dd\\THH-mm-ss\\Z");
		string path = Application.persistentDataPath + "\\" + datetime + ".txt";
		//System.IO.File.CreateText (path);
		System.IO.StreamWriter writer = new System.IO.StreamWriter (path, true);

		writer.WriteLine ("Version: " + version.text);
		writer.WriteLine ("-*-*-*-*-*-*-*-*-*-*-*-");
		writer.WriteLine ("");

		writer.WriteLine ("Errors:");
		writer.WriteLine ("-*-*-*-*-*-*-*-*-*-*-*-");
		foreach (string log in errorBacklog) {
			writer.WriteLine (log);
		}

		writer.WriteLine ("");
		writer.WriteLine ("");
		writer.WriteLine ("");
		writer.WriteLine ("Messages:");
		writer.WriteLine ("-*-*-*-*-*-*-*-*-*-*-*-");
		foreach (string log in errorBacklog) {
			writer.WriteLine (log);
		}


		writer.Close ();

		DataLogger.LogMessage ("Succesfully exported: " + path);
	}

	public static void SetDebugSettings (bool state){
		if (state) {
			PlayerPrefs.SetInt ("DebugCheats", 1);
		} else {
			PlayerPrefs.SetInt ("DebugCheats", 0);
		}

		UpdateDebugMode ();
	}

	static void UpdateDebugMode (){
		if (PlayerPrefs.GetInt ("DebugCheats", 0) == 1) {
			isDebugMode = true;
			LogMessage ("Debug Enabled");
		} else {
			isDebugMode = false;
			LogMessage ("Debug Disabled");
		}

		if (isDebugMode) {
			if (CharacterStuffController.s != null)
				CharacterStuffController.s.InstantActivate ();

		}
	}


	public void ToggleLogVisuals () {
		visibilityState = !visibilityState;
		PlayerPrefs.SetInt ("DebugVisibilityState", visibilityState ? 1 : 0);

		SetLogstate (visibilityState);
		SetDebugSettings (visibilityState);
	}

	void SetLogstate (bool state) {
		foreach (GameObject obj in stuffToToggleVisibility) {
			if (obj != null)
				obj.SetActive (state);
		}
	}
}

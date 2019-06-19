using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveMaster : MonoBehaviour {

	public static SaveMaster s;

	public SaveData mySave;

	public delegate void SaveDelegate ();
	public static SaveDelegate toSave;

	// Use this for initialization
	void Awake () {
		if (s != null && s != this) {
			Destroy (this.gameObject);
			return;
		} else {
			s = this;
		}

		if (!Load ()) {
			mySave = new SaveData ();
			if (GS.s == null)
				GS.s = GetComponent<GS> ();
			mySave.levelsCompleted = new bool[GS.s.allModes.Length];
			DataLogger.LogError ("Loading failed. creating new save");
		} else {
			DataLogger.LogMessage ("Loading successful");
		}
	}

	public static bool isLevelDone (GameSettings levelObject) {
		if (levelObject == null)
			return true;

		int levelId = GS.s.GetGameModeId (levelObject);
		if (levelId == -1) {
			DataLogger.LogError ("Unregistered level: " + levelObject.name);
			return false;
		}
		if (levelId < s.mySave.levelsCompleted.Length) {
			if (s.mySave.levelsCompleted[levelId]) {
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		}
	}

	[HideInInspector]
	public static void LevelFinished (bool isWon, GameSettings finishedLevel){
		if (isWon) {
			int levelId = GS.s.GetGameModeId (finishedLevel);

			try {
				if (levelId != -1) {
					if (!(levelId < s.mySave.levelsCompleted.Length)) {
						bool[] temp = new bool[s.mySave.levelsCompleted.Length];
						s.mySave.levelsCompleted.CopyTo (temp, 0);
						s.mySave.levelsCompleted = new bool[GS.s.allModes.Length];
						temp.CopyTo (s.mySave.levelsCompleted, 0);
					}
					s.mySave.levelsCompleted[levelId] = true;
					s.Save ();
				} else {
					DataLogger.LogError ("Cant complete level: " + levelId.ToString ());
				}
			} catch (System.Exception e) {
				DataLogger.LogError ("Cant complete level: " + levelId.ToString (), e);
			}
		}
	}

	[ContextMenu("Reset Progress")]
	public static void ResetProgress (){
		PlayerPrefs.DeleteAll ();
		s.mySave = new SaveData ();
		InventoryMaster.s.Start ();
		s.Save ();
		SceneMaster.s.LoadMenu ();
		//PlayerPrefs.SetInt ("FirstLevelDone", 1);

		DataLogger.LogError ("Progress Reset");
	}

	[ContextMenu ("Hard Reset Progress")]
	public static void HardReset () {
		PlayerPrefs.DeleteAll ();
		s.mySave = new SaveData ();
		s.mySave.activeEquipment = new SaveData.SaveEquipment (-1, -1);
		s.Save ();
		SceneMaster.s.LoadMenu ();

		DataLogger.LogError ("Progress Hard Reset");
	}

	[ContextMenu ("Unlock Everything")]
	public static void UnlockAll (){
		for (int i = 0; i < s.mySave.levelsCompleted.Length; i++) {
			s.mySave.levelsCompleted [i] = true;
		}
		for (int i = 0; i < s.mySave.unlockedElementLevels.Length; i++) {
			s.mySave.unlockedElementLevels[i] = 4;
		}
		s.Save ();
		SceneMaster.s.LoadMenu ();
	}

	void OnApplicationQuit (){
		Save ();
	}

	public void Save (){
		try {
			if (toSave != null)
				toSave.Invoke ();

			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Create (Application.persistentDataPath + "/" + "TheCardses" + ".banana");

			SaveData data = mySave;

			bf.Serialize (file, data);
			file.Close ();
			DataLogger.LogMessage ("Saving Complete");
		} catch (System.Exception e) {
			DataLogger.LogMessage (e.Message + " - " + e.StackTrace);
		}
	}

	public bool Load(){
		try {
			if (File.Exists (Application.persistentDataPath + "/" + "TheCardses" + ".banana")) {
				BinaryFormatter bf = new BinaryFormatter ();
				FileStream file = File.Open (Application.persistentDataPath + "/" + "TheCardses" + ".banana", FileMode.Open);
				SaveData data = (SaveData)bf.Deserialize (file);
				file.Close ();

				mySave = data;
				return true;
			} else {
				return false;
			}
		} catch (System.Exception e) {
			DataLogger.LogError (this.name, e);
		}
		return false;
	}
}

[Serializable]
public class SaveData {
	public string saveName = "default";
	public bool[] levelsCompleted = new bool[100];

	public SaveEquipment[] myEquipments = new SaveEquipment[0];
	public SaveIngredient[] myIngredients = new SaveIngredient[0];
	public SavePotion[] myPotions = new SavePotion[0];

	public SaveEquipment activeEquipment = new SaveEquipment (-1, -1);

	public int selectedElement = -1;
	public int elementLevel = -1;

	public int[] unlockedElementLevels = new int[7];

	public bool[] triggeredEvents = new bool[5];

	[Serializable]
	public class SavePotion {
		public int potionId = -1;
		public int chargesLeft = -1;

		public SavePotion (int _potionId, int _chargesLeft) {
			potionId = _potionId;
			chargesLeft = _chargesLeft;
		}

		public InventoryMaster.InventoryPotion ConvertToInventory () {
			if (potionId != -1)
				return new InventoryMaster.InventoryPotion (InventoryMaster.s.allPotions[potionId], chargesLeft);
			else
				return null;
		}
	}

	[Serializable]
	public class SaveEquipment {
		public int equipmentId = -1;
		public int chargesLeft = -1;

		public SaveEquipment (int _equipmentId, int _chargesLeft) {
			equipmentId = _equipmentId;
			chargesLeft = _chargesLeft;
		}

		public InventoryMaster.InventoryEquipment ConvertToInventory () {
			if (equipmentId != -1)
				return new InventoryMaster.InventoryEquipment (InventoryMaster.s.allEquipments[equipmentId], chargesLeft);
			else
				return null;
		}
	}

	[Serializable]
	public class SaveIngredient {
		public int ingredientId = -1;
		public int chargesLeft = -1;

		public SaveIngredient (int _ingredientId, int _chargesLeft) {
			ingredientId = _ingredientId;
			chargesLeft = _chargesLeft;
		}

		public InventoryMaster.InventoryIngredient ConvertToInventory () {
			if (ingredientId != -1)
				return new InventoryMaster.InventoryIngredient (InventoryMaster.s.allIngredients[ingredientId], chargesLeft);
			else
				return null;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class UnlockRequirementKeeper {

	public GameSettings[] requiredLevels = new GameSettings[1];

	public int[] questDecisionLockIDs = new int[0];
	public float[] questDecisionLockRequiredValues = new float[0];


	public static bool isUnlocked (GameSettings myLevel, int questDecisionLockId, float questDecisionReqValue) {
		bool isLevelChecked, isLevelDone, isQuestChecked, isQuestDone;

		isLevelChecked = myLevel != null;

		if (isLevelChecked) {
			//check if the level is done
			isLevelDone = SaveMaster.isLevelDone (myLevel);
		} else
			isLevelDone = true;

		isQuestChecked = questDecisionLockId != -1;
		if (isQuestChecked) {
			if (SaveMaster.s.mySave.questDecisions.Length < questDecisionLockId) {
				float[] temp = SaveMaster.s.mySave.questDecisions;
				SaveMaster.s.mySave.questDecisions = new float[questDecisionLockId];
				temp.CopyTo (SaveMaster.s.mySave.questDecisions, 0);
			}

			//check if the quest requirements are met
			isQuestDone = SaveMaster.s.mySave.questDecisions[questDecisionLockId] == questDecisionReqValue;
		} else
			isQuestDone = true;

		return isQuestDone && isLevelDone;
	}
}


/*[CustomPropertyDrawer (typeof (UnlockRequirementKeeper))]
public class IngredientDrawer : PropertyDrawer {
	// Draw the property inside the given rect
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
		// Using BeginProperty / EndProperty on the parent property means that
		// prefab override logic works on the entire property.
		EditorGUI.BeginProperty (position, label, property);

		// Draw label
		position = EditorGUI.PrefixLabel (position, GUIUtility.GetControlID (FocusType.Passive), label);

		// Don't make child fields be indented
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		// Calculate rects
		var amountRect = new Rect (position.x, position.y, 30, position.height);
		var unitRect = new Rect (position.x + 35, position.y, 50, position.height);
		var nameRect = new Rect (position.x + 90, position.y, position.width - 90, position.height);

		// Draw fields - passs GUIContent.none to each so they are drawn without labels
		EditorGUI.PropertyField (amountRect, property.FindPropertyRelative ("amount"), GUIContent.none);
		EditorGUI.PropertyField (unitRect, property.FindPropertyRelative ("unit"), GUIContent.none);
		EditorGUI.PropertyField (nameRect, property.FindPropertyRelative ("name"), GUIContent.none);

		// Set indent back to what it was
		EditorGUI.indentLevel = indent;

		EditorGUI.EndProperty ();
	}
}*/
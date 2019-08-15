using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor (typeof (QuickInspectorCheats))]
public class QuickInspectorCheatsEditor : Editor {


	public override void OnInspectorGUI () {
		DrawDefaultInspector ();
		QuickInspectorCheats myTarget = (QuickInspectorCheats)target;


		if (GUILayout.Button ("Cheat 1")) {
			myTarget.Cheat1 ();
		}
	}
}

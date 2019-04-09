using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSel", menuName = "GameSettings/PlayerSel", order = 4)]
public class PlayerSelectionSettings : ScriptableObject {

	public float checkSpeedPlayer = 0.35f;
	[Tooltip("Write negative to disable it")]
	public float deselectSpeedPlayer = 7f;
}

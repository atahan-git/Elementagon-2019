using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickInspectorCheats : MonoBehaviour
{
	public void Cheat1 () {
		ScoreBoardManager.s.AddScore (0, 0, -1, true);
	}
}

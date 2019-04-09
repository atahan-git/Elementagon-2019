using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "NPC", menuName = "GameSettings/NPC", order = 8)]
public class _NPCSettings : ScriptableObject {

	public bool isNPC = false;
	public int npcId = 4;
	public string npcName = "Dragon";

	public GameObject npcObject;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardSet", menuName = "GameSettings/CardSet", order = 10)]
public class CardSets : ScriptableObject {

	public CardBase[] cards;

	[Space]

	public float defaultSpawnChance = 7f;
	[Tooltip ("if left zero or null the default drop chance will be used")]
	public float[] customSpawnChances = new float[0];
}

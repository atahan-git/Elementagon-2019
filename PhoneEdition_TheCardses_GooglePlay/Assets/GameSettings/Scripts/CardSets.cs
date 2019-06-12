using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardSet", menuName = "GameSettings/CardSet", order = 10)]
public class CardSets : ScriptableObject {

	public const int UtilityCardsCount = 2;
	public const int posionTypeInt = 1;

	public CardBase defCard;
	public CardBase poisonCard;

	[Space]
	public CardBase[] cards;
	[Space]
	public CardBase[] customSpawnChanceCards;
	[Tooltip ("x% chance to spawn, if left zero or null the default drop chance will be used")]
	public float[] customSpawnChances = new float[0];
}

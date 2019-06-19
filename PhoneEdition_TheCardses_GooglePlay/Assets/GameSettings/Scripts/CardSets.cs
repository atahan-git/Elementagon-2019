using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardSet", menuName = "GameSettings/CardSet", order = 10)]
public class CardSets : ScriptableObject {

	public const int UtilityCardsCount = 3;
	public const int defTypeId = 0;
	public const int posionTypeId = 1;
	public const int matchedTypeId = 2;

	public CardBase defCard;
	public CardBase poisonCard;
	public CardBase matchedCard;

	[Space]
	public CardBase[] cards;
	[Space]
	public CardBase[] customSpawnChanceCards;
	[Tooltip ("x% chance to spawn, if left zero or null the default drop chance will be used")]
	public float[] customSpawnChances = new float[0];
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName = "New Card", menuName = "Card")]
public class CardBase : ScriptableObject {

	//--------CARD TYPES---------
	// 0 = empty / already gotten
	// 1-7 = normal cards
	// 8-14 = dragons
	//---------------------------
	//1 = Earth
	//2 = Fire
	//3 = Ice
	//4 = Light
	//5 = Nether
	//6 = Poison
	//7 = Shadow
	//---------------------------
	// 8 = Earth Dragon
	// 9 = Fire Dragon
	//10 = Ice Dragon
	//11 = Light Dragon
	//12 = Nether Dragon
	//13 = Poison Dragon
	//14 = Shadow Dragon
	//---------------------------

	public Sprite mySprite;
	public SpriteAnimationHolder myAnim;
	public bool isAnimated { get { return myAnim != null; } }

	[Tooltip ("//--------CARD TYPES---------\n// 0 = any type\n// 1-7 = normal cards\n// 8-14 = dragons\n//---------------------------\n// 1 = Earth\n// 2 = Fire\n// 3 = Ice\n// 4 = Light\n// 5 = Nether\n// 6 = Poison\n// 7 = Shadow\n//---------------------------\n// 8 = Earth Dragon\n// 9 = Fire Dragon\n//10 = Ice Dragon\n//11 = Light Dragon\n//12 = Nether Dragon\n//13 = Poison Dragon\n//14 = Shadow Dragon\n//---------------------------")]
    public int elementType = 0;

	public bool isAITargetable = true;
	public bool isPowerUpRelated = false;

	public int score = 1;
	public int enemyScore = 0;

	[HideInInspector]
	public int cardType = -1;  //dynamically allocated per level basis.
	public bool isItem { get { return myItem != null; } }
	[HideInInspector]
	public ItemBase myItem; //dynamically allocated per level basis.

	[Header ("Effects")]
	public GameObject onCard_SelectEffect;
	public GameObject onScoreBoard_SelectEffect;
	public GameObject onEnemySbs_SelectEffect;

	[Space]
	public GameObject onCard_MatchEffectBetweenCards;
	public GameObject onScoreBoard_MatchEffectBetweenCards;
	public GameObject onEnemySbs_MatchEffectBetweenCards;

	[Space]
	public GameObject onCard_MatchEffect;
	public GameObject onScoreBoard_MatchEffect;
	public GameObject onEnemySbs_MatchEffect;

}
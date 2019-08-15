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

	[Tooltip("leave at 0 alpha to use default color")]
	public Color effectColor;

	[Space]
	[Tooltip (" -1> normal card\n 1> power card\n 2> item card")]
	public int specialTypeID = -1;

	public bool isAITargetable = true;
	[Tooltip("If this is not null, npcs will use the values in this card to figure out the resulting match score result. Note that overriding is not network ready and will only override score and match gfx properties")]
	public CardBase npcMatchOverride;
	public bool isPowerUpRelated = false;
	[Space]
	public bool isPoison = false;
	public enum PoisonTypes { /*ScorePoison,*/ DeadlyPoison, PoisonCure }
	public PoisonTypes myPoisonType = PoisonTypes.DeadlyPoison;
	public int poisonAmount = 15;

	[Space]
	public int score = 1;
	public int enemyScore = 0;

	[HideInInspector]
	public int dynamicCardID = -1;  //dynamically allocated per level basis.
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
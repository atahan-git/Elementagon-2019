using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gfx", menuName = "GameSettings/Gfx", order = 9)]
public class GfxHolder : ScriptableObject {

	public bool isSpriteBased = true;
	[Tooltip("//--------CARD TYPES---------\n// 0 = empty / already gotten\n// 1-7 = normal cards\n// 8-14 = dragons\n//---------------------------\n// 1 = Earth\n// 2 = Fire\n// 3 = Ice\n// 4 = Light\n// 5 = Nether\n// 6 = Poison\n// 7 = Shadow\n//---------------------------\n// 8 = Earth Dragon\n// 9 = Fire Dragon\n//10 = Ice Dragon\n//11 = Light Dragon\n//12 = Nether Dragon\n//13 = Poison Dragon\n//14 = Shadow Dragon\n//---------------------------")]
	public Sprite[] cardSprites = new Sprite[32];
	[Tooltip("//--------CARD TYPES---------\n// 0 = empty / already gotten\n// 1-7 = normal cards\n// 8-14 = dragons\n//---------------------------\n// 1 = Earth\n// 2 = Fire\n// 3 = Ice\n// 4 = Light\n// 5 = Nether\n// 6 = Poison\n// 7 = Shadow\n//---------------------------\n// 8 = Earth Dragon\n// 9 = Fire Dragon\n//10 = Ice Dragon\n//11 = Light Dragon\n//12 = Nether Dragon\n//13 = Poison Dragon\n//14 = Shadow Dragon\n//---------------------------")]
	public SpriteAnimationHolder[] cardAnimations = new SpriteAnimationHolder [32];
	public GameObject card;
	public GameObject getEffectPrefab;

	[Space]
	public GameObject revealEffect;
	public GameObject posionProtectEffect;

	[Space]
	public GameObject[] selectEffects = new GameObject[1 + 4 + 1 + 1];

	[Space]
	public GameObject npcScoreboard;


	[Header ("Default Effects")]
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

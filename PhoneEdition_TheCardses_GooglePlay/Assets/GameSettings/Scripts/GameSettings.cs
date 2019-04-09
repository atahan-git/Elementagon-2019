using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "GameSetting", menuName = "GameSettings/GameSetting", order = -1)]
public class GameSettings : ScriptableObject {

	[HideInInspector]
	public int id = -1;

	public string PresetName = "Default";

	[Header ("Card & Grid Settings")]

	public GridSettings gridSettings;

	[Space]
	[HideInInspector]
	public CardData[] cards;

	[SerializeField]
	CardBase[] levelCards;

	public float defaultSpawnChance = 7f;
	[Tooltip ("if left zero or null the default drop chance will be used")]
	public float[] customSpawnChances = new float[0];

	public struct CardData {
		public CardBase cBase;
		public float chance;
		public CardData (CardBase b, float c) { cBase = b; chance = c; }
	}
	public CardBase defCard;

	public void SetUpCards () {
		cards = new CardData[levelCards.Length + possibleDrops.Length];

		float[] chances = new float[levelCards.Length];
		customSpawnChances.CopyTo (chances, 0);

		for (int i = 0; i < chances.Length; i++) {
			chances[i] = chances[i] == 0 ? defaultSpawnChance : chances[i];
			cards[i] = new CardData (levelCards[i], chances[i]);
		}

		GenerateItemCards ();

		for (int i = 0; i < cards.Length; i++) {
			cards[i].cBase.cardType = i;
		}
	}

	public float cardReOpenTime = 15f;

	public GfxHolder gfxs;


	[Header ("Player Card Selection Settings")]

	public PlayerSelectionSettings playerSelectionSettings;

	[Header ("Gameplay Settings")]

	

	[Tooltip ("Leave empty if you want it to be auto generated based on the other settings")]
	public string objectiveText = "";

	[Header ("Put -1 if you dont want to limit")]
	public float timer = -1;
	public int turns = -1;
	public int scoreReach = 30;
	public bool killAllNPC = false;

	public bool canCombo = true;

	public enum GameType { Singleplayer, Two_Coop, OneVOne , TwoVTwo,Three_FreeForAll, Four_FreeForAll };
	[Header ("Game Type Settings")]

	public GameType myGameType = GameType.Singleplayer;


	[Header ("Power Up Settings")]

	public PowerUpSettings powerUpSettings;

	/*[Header ("Objective Settings")]

	public ObjectiveSettings objectiveSettings;*/

	[Header ("Menu Settings")]

	public string levelShortName;
	public string levelName;
	[TextArea]
	public string levelDescription;

	[Header ("Level Settings")]

	[TextArea]
	public string startingText;
	public Sprite startingImage;
	[Space]
	public SpriteAnimationHolder bgAnimation;
	public Sprite bgSprite;

	public DialogTreeAsset levelIntroDialog;
	public DialogTreeAsset levelOutroDialog;
	[Space]

	//[Tooltip ("//--------CARD TYPES---------\n// 0 = empty / already gotten\n// 1-7 = normal cards\n// 8-14 = dragons\n//---------------------------\n// 1 = Earth\n// 2 = Fire\n// 3 = Ice\n// 4 = Light\n// 5 = Nether\n// 6 = Poison\n// 7 = Shadow\n//---------------------------\n// 8 = Earth Dragon\n// 9 = Fire Dragon\n//10 = Ice Dragon\n//11 = Light Dragon\n//12 = Nether Dragon\n//13 = Poison Dragon\n//14 = Shadow Dragon\n//---------------------------")]
	//public int [] startingHand = new int [15];

	public GameObject customObject;

	public bool autoSetUpBoard = true;

	[Header ("Item Drop Settings")]
	[Tooltip ("Every new card has a x chance to be the item card, normal cards have 7 chance")]
	public float defaultDropChance = 1f;
	public ItemBase[] possibleDrops = new ItemBase[0];
	[Tooltip("if left zero or null the default drop chance will be used")]
	public float[] customDropChances = new float[0];
	[HideInInspector]
	public int itemTypesStartIndex = 7;

	void GenerateItemCards () {
		itemTypesStartIndex = levelCards.Length;

		float[] chances = new float[possibleDrops.Length];
		customDropChances.CopyTo (chances,0);

		for (int i = 0; i < possibleDrops.Length; i++) {
			chances[i] = chances[i] == 0 ? defaultDropChance : chances[i];
			cards[itemTypesStartIndex + i] = new CardData(Instantiate(defCard), chances[i]);
			cards[itemTypesStartIndex + i].cBase.mySprite = possibleDrops[i].cardSprite != null ? possibleDrops[i].cardSprite : possibleDrops[i].sprite;
			cards[itemTypesStartIndex + i].cBase.myAnim = null;
			cards[itemTypesStartIndex + i].cBase.myItem = possibleDrops[i];
		}
	}


	//npc spawning is handled by game starter
	[Header ("NPC Settings")]
	public NPCBase myNPC;
	public bool isNPCEnabled {
		get { return myNPC != null; }
	}
	[Tooltip("Use -1 for unlimited spawns")]
	public int npcSpawnCount = 1;
	[Tooltip("Use -1 to spawn when the other one dies (not implemented yet)")]
	public float npcSpawnDelay = 10f;

	[Header ("Compound Level Settings")]
	public GameSettings nextStage;

	public GameSettings Copy () {
		GameSettings myCopy = Instantiate (this);
		return myCopy;
	}
}

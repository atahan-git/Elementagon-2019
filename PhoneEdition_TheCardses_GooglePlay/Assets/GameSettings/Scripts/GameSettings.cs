using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Game Settings", menuName = "GameSettings/GameSetting", order = -1)]
public class GameSettings : ScriptableObject {

	[HideInInspector]
	public int id = -1;

	public string PresetName = "New Game Settings";

	[Header ("Card & Grid Settings")]

	public GridSettings gridSettings;

	[Space]

	public CardSets cardSet;

	public float cardReOpenTime = 15f;

	public GfxHolder gfxs;


	[Header ("Player Card Selection Settings")]

	public PlayerSelectionSettings playerSelectionSettings;

	[Header ("Gameplay Settings")]

	public GameRuleTypes myGameRuleType = GameRuleTypes.Standard;
	public GamePlayerTypes myGamePlayerType = GamePlayerTypes.Singleplayer;
	public enum GameRuleTypes { Standard, Farm }
	public enum GamePlayerTypes { Singleplayer, Two_Coop, OneVOne, TwoVTwo, Three_FreeForAll, Four_FreeForAll };
	public bool canCombo = true;

	[Space]
	public Objective[] winObjectives;
	public Objective[] loseObjectives;
	[Space]

	public ItemBase[] winDrops;
	[Tooltip("Leave empty or 0 for dropping exactly one item")]
	public int[] windDropAmounts;


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

	[Tooltip ("Every new card has a x% chance to be an item card, with 100 meaning all cards in the level being a item card")]
	public float dropChance = 1f;
	[Tooltip ("Every 10*difficulty scores the chance of the higher grade item drops double also drop chance doubles")]
	public float farmModeDifficulty = 1f;
	[Tooltip ("Has 70% chance to drop")]
	public ItemBase[] possibleDropsDGrade = new ItemBase[0];
	[Tooltip ("Has 18% chance to drop")]
	public ItemBase[] possibleDropsCGrade = new ItemBase[0];
	[Tooltip ("Has 9% chance to drop")]
	public ItemBase[] possibleDropsBGrade = new ItemBase[0];
	[Tooltip ("Has 3% chance to drop")]
	public ItemBase[] possibleDropsAGrade = new ItemBase[0];


	//npc spawning is handled by game starter & NPC manager
	[Header ("NPC Settings")]
	public NPCBase myNPC;
	public bool isNPCEnabled {
		get { return myNPC != null; }
	}
	[Tooltip("Use -1 for unlimited spawns")]
	public int npcSpawnCount = 1;
	[Tooltip("Use -1 to spawn when the other one dies (not implemented yet)")]
	public float npcSpawnDelay = 5f;


	[Header ("Player stuff override Settings")]
	public bool overrideEquipment;
	public Equipment equipment;

	public bool overridePotions;
	public Potion[] potions;

	public bool overridePower;
	[Tooltip ("//0-7 = dragons\n//---------------------------\n//0 = Earth Dragon\n//1 = Fire Dragon\n//2 = Ice Dragon\n//3 = Light Dragon\n//4 = Nether Dragon\n//5 = Poison Dragon\n//6 = Shadow Dragon\n//---------------------------")]
	public int selectedElement = -1;
	public int elementLevel = 0;

	[Header ("Compound Level Settings")]
	public GameSettings nextStage;

	public GameSettings Copy () {
		GameSettings myCopy = Instantiate (this);
		myCopy.gridSettings = Instantiate (gridSettings);
		myCopy.cardSet = Instantiate (cardSet);
		myCopy.gfxs = Instantiate (gfxs);
		myCopy.playerSelectionSettings = Instantiate (playerSelectionSettings);
		myCopy.powerUpSettings = Instantiate (powerUpSettings);

		return myCopy;
	}
}

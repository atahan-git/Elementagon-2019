using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardTypeRandomizer : MonoBehaviour {
	public static CardTypeRandomizer s;

	public CardBase[] allCards;

	public CardBase itemBaseCard;

	[Header("Auto Assigned")]
	public int utilityStartIndex;
	public int normalCardStartIndex;
	public int customSpawnChanceStartIndex;
	public int forceSpawnStartIndex;
	public int itemStartIndex;

	public int utilityCount;
	public int normalCardsCount;
	public int customSpawnChanceCardsCount;
	public int forceSpawnCardsCount;
	public int itemCount;
	[Space]

	public GameObject farmBoard;
	public GameObject sectionParent;
	public GameObject eachItem;

	public List<GameObject>[] itemsDisps;

	public void Initialize () {
		utilityCount = CardSets.UtilityCardsCount;
		normalCardsCount = GS.a.cardSet.cards.Length;
		customSpawnChanceCardsCount = GS.a.cardSet.customSpawnChanceCards.Length;
		forceSpawnCardsCount = GS.a.cardSet.forceSpawnInPairAtStartCards.Length;
		itemCount = 0;
		itemCount += GS.a.possibleDropsDGrade.Length;
		itemCount += GS.a.possibleDropsCGrade.Length;
		itemCount += GS.a.possibleDropsBGrade.Length;
		itemCount += GS.a.possibleDropsAGrade.Length;
		allCards = new CardBase[utilityCount + normalCardsCount + customSpawnChanceCardsCount + itemCount];

		utilityStartIndex = 0;
		normalCardStartIndex = utilityCount;
		customSpawnChanceStartIndex = normalCardStartIndex + normalCardsCount;
		forceSpawnStartIndex = customSpawnChanceStartIndex + customSpawnChanceCardsCount;
		itemStartIndex = forceSpawnStartIndex + forceSpawnCardsCount;

		allCards[CardSets.defTypeId] = GS.a.cardSet.defCard;
		allCards[CardSets.posionTypeId] = GS.a.cardSet.poisonCard;
		allCards[CardSets.matchedTypeId] = GS.a.cardSet.matchedCard;


		//Normal cards
		for (int i = 0; i < normalCardsCount; i++) {
			allCards[i + normalCardStartIndex] = GS.a.cardSet.cards[i];
		}


		//force spawn cards
		for (int i = 0; i < forceSpawnCardsCount; i++) {
			allCards[i + forceSpawnStartIndex] = GS.a.cardSet.forceSpawnInPairAtStartCards[i];
		}

		//custom spawn chance cards
		for (int i = 0; i < customSpawnChanceCardsCount; i++) {
			allCards[i + customSpawnChanceStartIndex] = GS.a.cardSet.customSpawnChanceCards[i];

			//if the player doesnt have a power up or an equipment dont spawn that card!
			//this need multiplayer support though
			float notEquippedMultiplier = 1;
			if (GS.a.cardSet.customSpawnChanceCards[i].elementType == 16 && !CharacterStuffController.s.isEquippedEquipment)
				notEquippedMultiplier = 0;
			if (GS.a.cardSet.customSpawnChanceCards[i].elementType == 17 && !CharacterStuffController.s.isEquippedPower)
				notEquippedMultiplier = 0;

			GS.a.cardSet.customSpawnChances[i] *= notEquippedMultiplier;
		}


		//items
		int itemCurStartIndex = 0;
		AddItems (GS.a.possibleDropsDGrade, ref itemCurStartIndex);
		AddItems (GS.a.possibleDropsCGrade, ref itemCurStartIndex);
		AddItems (GS.a.possibleDropsBGrade, ref itemCurStartIndex);
		AddItems (GS.a.possibleDropsAGrade, ref itemCurStartIndex);


		//setting up
		for (int i = 0; i < allCards.Length; i++) {
			allCards[i].dynamicCardID = i;
		}


		//farm stuff
		if (GS.a.myGameRuleType == GameSettings.GameRuleTypes.Farm) {
			try {
				DrawFarmBoard ();
			} catch (System.Exception e) {
				DataLogger.LogError (this.name, e);
				farmBoard.SetActive (false);
			}
		} else {
			farmBoard.SetActive (false);
		}

		DataLogger.LogMessage ("Card Types initialized");

	}

	void AddItems (ItemBase[] toAdd, ref int itemCurStartIndex) {
		for (int i = 0; i < toAdd.Length; i++) {
			allCards[i + itemStartIndex] = Instantiate (itemBaseCard);
			allCards[i + itemStartIndex].mySprite = toAdd[i].cardSprite != null ? toAdd[i].cardSprite: toAdd[i].sprite;
			allCards[i + itemStartIndex].myAnim = null;
			allCards[i + itemStartIndex].myItem = toAdd[i];
		}
		itemCurStartIndex += toAdd.Length;
	}

	void DrawFarmBoard () {
		farmBoard.SetActive (true);

		int maxLength = 0;
		maxLength = Mathf.Max (GS.a.possibleDropsDGrade.Length, GS.a.possibleDropsCGrade.Length, GS.a.possibleDropsBGrade.Length, GS.a.possibleDropsAGrade.Length);
		itemsDisps = new List<GameObject>[4];

		DrawSection (GS.a.possibleDropsAGrade, 0, "A");
		DrawSection (GS.a.possibleDropsBGrade, 1, "B");
		DrawSection (GS.a.possibleDropsCGrade, 2, "C");
		DrawSection (GS.a.possibleDropsDGrade, 3, "D");

		UpdateFarmBoard ();
	}

	void DrawSection (ItemBase[] toAdd, int i, string sectionName) {
		if (toAdd.Length > 0) {
			GameObject sParent = Instantiate (sectionParent, farmBoard.transform);
			sParent.GetComponentInChildren<TextMeshProUGUI> ().text = sectionName;

			itemsDisps[i] = new List<GameObject> ();
			foreach (ItemBase item in toAdd) {
				itemsDisps[i].Add (Instantiate (eachItem, sParent.transform));
				itemsDisps[i][itemsDisps[i].Count-1].transform.Find ("Image").GetComponent<Image> ().sprite = item.cardSprite != null ? item.cardSprite : item.sprite;
			}
		}
	}

	/*[Tooltip ("Has 70% chance to drop")]
	public ItemBase[] possibleDropsDGrade = new ItemBase[0];
	[Tooltip ("Has 18% chance to drop")]
	public ItemBase[] possibleDropsCGrade = new ItemBase[0];
	[Tooltip ("Has 9% chance to drop")]
	public ItemBase[] possibleDropsBGrade = new ItemBase[0];
	[Tooltip ("Has 3% chance to drop")]
	public ItemBase[] possibleDropsAGrade = new ItemBase[0];*/

	public int GiveRandomCardType () {
		return GiveRandomCardType (true);
	}

	float[] itChances = { 3f, 9f, 18f, 100f };
	float[] itMaxChances = { 15f, 20f, 30f, 100f };

	public float itemFarmMultiplier = 1;
	public int GiveRandomCardType (bool isVerbose) {
		if (GS.a.myGameRuleType == GameSettings.GameRuleTypes.Farm) {
			try {
				UpdateFarmBoard ();
			} catch {
				DataLogger.LogError ("Cant update farmboard");
			}
		}


		float thisRoll = Random.value;
		thisRoll *= 100;
		
		int myType = -1;
		if (thisRoll < GS.a.dropChance * itemFarmMultiplier) {

			float itemRoll = Random.value * 100;

			int thisType = 0;
			float[] calcChances = CalcItemChances ();
			
			if (itemRoll < calcChances[0] && GS.a.possibleDropsAGrade.Length > 0) {
				thisType = Random.Range (0, GS.a.possibleDropsAGrade.Length);
				myType = itemStartIndex + thisType + GS.a.possibleDropsDGrade.Length + GS.a.possibleDropsCGrade.Length + GS.a.possibleDropsBGrade.Length;

			} else if (itemRoll < (calcChances[0] + calcChances[1]) && GS.a.possibleDropsBGrade.Length > 0) {
				thisType = Random.Range (0, GS.a.possibleDropsBGrade.Length);
				myType = itemStartIndex + thisType + GS.a.possibleDropsDGrade.Length + GS.a.possibleDropsCGrade.Length;

			} else if (itemRoll < (calcChances[0] + calcChances[1] + calcChances[2]) && GS.a.possibleDropsCGrade.Length > 0) {
				thisType = Random.Range (0, GS.a.possibleDropsCGrade.Length);
				myType = itemStartIndex + thisType + GS.a.possibleDropsDGrade.Length;

			} else if (GS.a.possibleDropsDGrade.Length > 0) {
				thisType = Random.Range (0, GS.a.possibleDropsDGrade.Length);
				myType = itemStartIndex + thisType;
			}
			
		}
		if (myType >= allCards.Length)
			DataLogger.LogError ("illegal item roll");

		float cardTypeReRoll = Random.value;
		cardTypeReRoll *= 100;
		if(myType == -1){
			float curPercent = 0;
			int i = 0;
			foreach (float chance in GS.a.cardSet.customSpawnChances) {
				curPercent += chance;
				if (cardTypeReRoll < curPercent) {
					myType = customSpawnChanceStartIndex + i;
					break;
				}
				i++;
			}
		}
		if (myType >= allCards.Length)
			DataLogger.LogError ("illegal special roll");

		if (myType == -1) {
			myType = normalCardStartIndex + Random.Range (0, normalCardsCount);
		}
		if (myType >= allCards.Length)
			DataLogger.LogError ("illegal normal roll");

		if (myType == -1)
			myType = 0;

		if (myType >= allCards.Length || myType <= 0) {
			DataLogger.LogError ("Illegal card type roll detected, fixing");
			myType = 0;
		}

		//Debug.Log ("random type: " + myType.ToString());
		return myType;
	}

	float[] CalcItemChances () {
		itemFarmMultiplier = Mathf.CeilToInt ((ScoreBoardManager.s.allScores[0, 0] + 1) / (5 * GS.a.farmModeDifficulty));
		float[] calcChances = new float[4];
		calcChances[0] = Mathf.Min ((itChances[0]) * itemFarmMultiplier, (itMaxChances[0]));
		calcChances[1] = Mathf.Min ((itChances[1]) * itemFarmMultiplier, (itMaxChances[1]));
		calcChances[2] = Mathf.Min ((itChances[2]) * itemFarmMultiplier, (itMaxChances[2]));
		calcChances[3] = Mathf.Min ((itChances[3]), 
			(100f - (
			GS.a.possibleDropsAGrade.Length > 0 ? calcChances[0] : 0 + 
			GS.a.possibleDropsBGrade.Length > 0 ? calcChances[1] : 0 +
			GS.a.possibleDropsCGrade.Length > 0 ? calcChances[2] : 0
			)));

		return calcChances;
	}

	public void UpdateFarmBoard () {
		//print ("Updating farm board");
		float runningChance = 0;
		float[] calcChances = CalcItemChances ();

		for (int i = 0; i < 4; i++) {
			if (itemsDisps[i] != null) {
				if (itemsDisps[i].Count > 0) {
					for (int n = 0; n < itemsDisps[i].Count; n++) {
						TextMeshProUGUI myText = itemsDisps[i][n].GetComponentInChildren<TextMeshProUGUI> ();
						float thisChance = (calcChances[i] / 100f);
						//print (thisChance.ToString () + " = " + "(" + calcChances[i].ToString () + "/ 100f)");
						//print ((((GS.a.dropChance) * thisChance * itemFarmMultiplier) / itemsDisps[i].Count).ToString () + " = " +
						//	"(((" + GS.a.dropChance.ToString ()+") * " + thisChance.ToString () + ") / " + itemsDisps[i].Count.ToString () + ")");
						if (myText != null) {
							myText.text = (((GS.a.dropChance) * thisChance * itemFarmMultiplier) / itemsDisps[i].Count).ToString ("F2") + "%";
						}
					}
					runningChance += (itChances[i] / 100f) * itemFarmMultiplier;
				}
			}
		}

		//DebugPrintCardChances ();
	}

	public void DebugPrintCardChances () {
		int[] lele = new int[allCards.Length];

		for (int i = 0; i < 10000; i++) {

			int type = GiveRandomCardType (false);
			//if (type >= allCards.Length)
			//print (type);

			//type = utilityCount + Random.Range (0, normalCardsCount);
			if (type < lele.Length && type >= 0)
				lele[type]++;
			else
				print (type);
		}

		print ("Random chances -*-*-*-*-*-*-*-*-*-*-");
		for (int i = 0; i < lele.Length; i++) {
			string keke = "";
			for (int k = 0; k < lele[i] / 10; k++) {
				keke += "|";
			}
			print (i.ToString () + keke);
		}
		print ("Random chances -*-*-*-*-*-*-*-*-*-*-");
	}
}

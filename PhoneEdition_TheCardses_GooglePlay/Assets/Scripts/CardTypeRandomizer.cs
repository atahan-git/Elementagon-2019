using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardTypeRandomizer : MonoBehaviour {
	public static CardTypeRandomizer s;

	public CardBase[] allCards;

	public CardBase itemBaseCard;

	[SerializeField]
	int utilityCount;
	[SerializeField]
	int normalCardsCount;
	[SerializeField]
	int customSpawnChanceCardsCount;
	[SerializeField]
	int itemCount;


	public GameObject farmBoard;
	public GameObject sectionParent;
	public GameObject eachItem;

	public List<GameObject>[] itemsDisps;

	public void Initialize () {
		utilityCount = CardSets.UtilityCardsCount;
		normalCardsCount = GS.a.cardSet.cards.Length;
		customSpawnChanceCardsCount = GS.a.cardSet.customSpawnChanceCards.Length;
		itemCount = 0;
		itemCount += GS.a.possibleDropsDGrade.Length;
		itemCount += GS.a.possibleDropsCGrade.Length;
		itemCount += GS.a.possibleDropsBGrade.Length;
		itemCount += GS.a.possibleDropsAGrade.Length;
		allCards = new CardBase[utilityCount + normalCardsCount + customSpawnChanceCardsCount + itemCount];

		allCards[CardSets.defTypeId] = GS.a.cardSet.defCard;
		allCards[CardSets.posionTypeId] = GS.a.cardSet.poisonCard;
		allCards[CardSets.matchedTypeId] = GS.a.cardSet.matchedCard;

		for (int i = 0; i < normalCardsCount; i++) {
			allCards[i + utilityCount] = GS.a.cardSet.cards[i];
		}
		for (int i = 0; i < customSpawnChanceCardsCount; i++) {
			allCards[i + utilityCount + normalCardsCount] = GS.a.cardSet.customSpawnChanceCards[i];
		}

		int itemCurStartIndex = 0;
		AddItems (GS.a.possibleDropsDGrade, ref itemCurStartIndex);
		AddItems (GS.a.possibleDropsCGrade, ref itemCurStartIndex);
		AddItems (GS.a.possibleDropsBGrade, ref itemCurStartIndex);
		AddItems (GS.a.possibleDropsAGrade, ref itemCurStartIndex);

		for (int i = 0; i < allCards.Length; i++) {
			allCards[i].cardType = i;
		}


		if (GS.a.myGameObjectiveType == GameSettings.GameObjectiveTypes.Farm) {
			DrawFarmBoard ();
		} else {
			farmBoard.SetActive (false);
		}

		DataLogger.LogMessage ("Card Types initialized");

		/*int[] lele = new int[allCards.Length];

		for (int i = 0; i < 10000; i++) {

			int type = GiveRandomCardType ();
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
			print (i.ToString() + keke);
		}
		print ("Random chances -*-*-*-*-*-*-*-*-*-*-");*/

	}

	void AddItems (ItemBase[] toAdd, ref int itemCurStartIndex) {
		for (int i = 0; i < toAdd.Length; i++) {
			allCards[i + utilityCount + normalCardsCount + customSpawnChanceCardsCount + itemCurStartIndex] = Instantiate (itemBaseCard);
			allCards[i + utilityCount + normalCardsCount + customSpawnChanceCardsCount + itemCurStartIndex].mySprite = toAdd[i].cardSprite != null ? toAdd[i].cardSprite: toAdd[i].sprite;
			allCards[i + utilityCount + normalCardsCount + customSpawnChanceCardsCount + itemCurStartIndex].myAnim = null;
			allCards[i + utilityCount + normalCardsCount + customSpawnChanceCardsCount + itemCurStartIndex].myItem = toAdd[i];
		}
		itemCurStartIndex += toAdd.Length;
	}

	void DrawFarmBoard () {
		farmBoard.SetActive (true);

		int maxLength = 0;
		maxLength = Mathf.Max (GS.a.possibleDropsDGrade.Length, GS.a.possibleDropsCGrade.Length, GS.a.possibleDropsBGrade.Length, GS.a.possibleDropsAGrade.Length);
		itemsDisps = new List<GameObject>[4];

		DrawSection (GS.a.possibleDropsDGrade, 0, "D");
		DrawSection (GS.a.possibleDropsCGrade, 1, "C");
		DrawSection (GS.a.possibleDropsBGrade, 2, "B");
		DrawSection (GS.a.possibleDropsAGrade, 3, "A");

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

	float[] itChances = { 3f, 9f, 18f, 70f };

	int itemFarmMultiplier = 1;
	public int GiveRandomCardType () {
		if (GS.a.myGameObjectiveType == GameSettings.GameObjectiveTypes.Farm) {
			itemFarmMultiplier = Mathf.CeilToInt (ScoreBoardManager.s.allScores[0, 0] / (10 * GS.a.farmModeDifficulty));
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

			float itemRoll = Random.value;

			int thisType = 0;
			if (itemRoll < (itChances[0]) * itemFarmMultiplier && GS.a.possibleDropsAGrade.Length > 0) {
				thisType = Random.Range (0, GS.a.possibleDropsAGrade.Length);
				myType = utilityCount + normalCardsCount + thisType + GS.a.possibleDropsDGrade.Length + GS.a.possibleDropsCGrade.Length + GS.a.possibleDropsBGrade.Length;

			} else if (itemRoll < (itChances[0] + itChances[1]) * itemFarmMultiplier && GS.a.possibleDropsBGrade.Length > 0) {
				thisType = Random.Range (0, GS.a.possibleDropsBGrade.Length);
				myType = utilityCount + normalCardsCount + thisType + GS.a.possibleDropsDGrade.Length + GS.a.possibleDropsCGrade.Length;

			} else if (itemRoll < (itChances[0] + itChances[1] + itChances[2]) * itemFarmMultiplier && GS.a.possibleDropsCGrade.Length > 0) {
				thisType = Random.Range (0, GS.a.possibleDropsCGrade.Length);
				myType = utilityCount + normalCardsCount + thisType + GS.a.possibleDropsDGrade.Length;

			} else if (GS.a.possibleDropsDGrade.Length > 0) {
				thisType = Random.Range (0, GS.a.possibleDropsDGrade.Length);
				myType = utilityCount + normalCardsCount + thisType;
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
					myType = utilityCount +normalCardsCount+ i;
					break;
				}
				i++;
			}
		}
		if (myType >= allCards.Length)
			DataLogger.LogError ("illegal special roll");

		if (myType == -1) {
			myType = utilityCount + Random.Range (0, normalCardsCount);
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

	void UpdateFarmBoard () {
		float runningChance = 0;
		for (int i = 0; i < 4; i++) {
			if (itemsDisps[i] != null) {
				if (itemsDisps[i].Count > 0) {
					for (int n = 0; n < itemsDisps[i].Count; n++) {
						TextMeshProUGUI myText = itemsDisps[i][n].GetComponentInChildren<TextMeshProUGUI> ();
						float thisChance = (itChances[i] / 100f) * itemFarmMultiplier;
						float runnerUp = (thisChance + runningChance) - 100f;
						if (runnerUp > 0)
							thisChance -= runnerUp;
						thisChance = Mathf.Min (thisChance, 0);
						if (myText != null) {
							myText.text = (((GS.a.dropChance / 100f) * itemFarmMultiplier * thisChance) / itemsDisps[i].Count).ToString (".xx") + "%";
						}
					}
					runningChance += (itChances[i] / 100f) * itemFarmMultiplier;
				}
			}
		}
	}
}

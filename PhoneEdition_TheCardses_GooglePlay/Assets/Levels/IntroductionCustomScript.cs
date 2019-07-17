using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroductionCustomScript : MonoBehaviour {

	/*public GridSettings only7Cards;
	GridSettings normalGrid;*/

	public void Start () {

		for (int x = 0; x < CardHandler.s.allCards.GetLength (0); x++) {
			CardHandler.s.allCards[x, 0].UpdateCardType (GS.a.cardSet.cards[CardSets.UtilityCardsCount + x].dynamicCardID);
		}

		DialogTree.s.myCustomTriggers[0] += ShowSevenDragons;
		DialogTree.s.myCustomTriggers[1] += ShowMatching;
		DialogTree.s.myCustomTriggers[2] += EndLevel;

		myPanel.SetActive (false);
		GameObject dialogScreen = DialogDisplayer.s.gameObject;
		dialogScreen.GetComponentInChildren<Image> ().color = new Color (1, 1, 1, 0);
	}

	public void ShowSevenDragons () {
		/*normalGrid = GS.a.gridSettings;
		GS.a.gridSettings = only7Cards;*/
		//CardHandler.s.SetUpGrid ();


		for (int x = 0; x < CardHandler.s.allCards.GetLength(0); x++) {
			for (int y = 0; y < CardHandler.s.allCards.GetLength (1); y++) {
				CardHandler.s.allCards[x, y].SelectCard (-1);
			}
		}
	}

	public void ShowMatching () {
		//GS.a.gridSettings = normalGrid;
		//CardHandler.s.SetUpGrid ();
		BeginCardSelecting ();
	}

	public GameObject myPanel;
	public GameObject cardSelectStuff;
	public GameObject scoreStuff;
	public Text myText;


	public GridSettings afterSettings;

	void BeginCardSelecting () {
		StartCoroutine (Tutorial ());
	}

	IEnumerator Tutorial () {
		DialogDisplayer.s.SetDialogScreenState (false);
		for (int x = 0; x < CardHandler.s.allCards.GetLength (0); x++) {
			for (int y = 0; y < CardHandler.s.allCards.GetLength (1); y++) {
				CardHandler.s.allCards[x, y].UnSelectCard ();
			}
		}


		yield return new WaitForSeconds(0.5f);

		CardHandler.s.SetUpGrid (afterSettings);

		yield return new WaitForSeconds (0.5f);
		myPanel.SetActive (true);
		LocalPlayerController.isActive = true;
		cardSelectStuff.SetActive (true);
		scoreStuff.SetActive (false);

		foreach (IndividualCard mycard in CardHandler.s.allCards) {
			if (!(mycard.y == 1 && (mycard.x == 4 || mycard.x == 5)))
				mycard.isSelectable = false;
		}

		CardHandler.s.UpdateCardType (4, 1, CardSets.UtilityCardsCount + 1);
		CardHandler.s.UpdateCardType (5, 1, CardSets.UtilityCardsCount + 1);

		yield return new WaitUntil (() => ScoreBoardManager.s.allScores[0, 0] > 0);

		cardSelectStuff.SetActive (false);
		scoreStuff.SetActive (true);
		myText.text = "Çabuk öğreniyorsun. Bak skorun arttı.";

		yield return new WaitUntil (() => Input.GetMouseButtonDown (0));

		foreach (IndividualCard mycard in CardHandler.s.allCards) {
			if (!(mycard.y == 1 && (mycard.x == 4 || mycard.x == 5)))
				mycard.isSelectable = true;
		}

		LocalPlayerController.isActive = false;
		DialogDisplayer.s.SetDialogScreenState (true);
		DialogDisplayer.s.NextDialog ();
		myPanel.SetActive (false);

		yield return null;
	}

	public GameSettings introLevel;
	public void EndLevel () {
		PlayerPrefs.SetInt ("LastMenuState", 2+5);
		PlayerPrefs.SetInt ("FirstLevelDone",1);
		MenuMasterController.currentPlace = new List<int> () { 2 };
		SaveMaster.LevelFinished (true, introLevel);
		SceneMaster.s.LoadMenu ();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_CardMatching : MonoBehaviour {

	//4-1
	//5-1

	public GameObject myPanel;
	public GameObject cardSelectStuff;
	public GameObject scoreStuff;
	public Text myText;


	bool isCardMatched = false;

	// Use this for initialization
	void Start () {
		myPanel.SetActive (false);
		cardSelectStuff.SetActive (true);
		scoreStuff.SetActive (false);

		isCardMatched = PlayerPrefs.GetInt ("isCardMatched", 0) == 1;

		if(!isCardMatched)
		GameStarter.s.LevelBeginCall += Engage;
	}

	public void Engage () {
		StartCoroutine (Tutorial());
		isCardMatched = true;
		PlayerPrefs.SetInt ("isCardMatched", 1);
	}

	IEnumerator Tutorial () {
		myPanel.SetActive (true);

		foreach (IndividualCard mycard in CardHandler.s.allCards) {
			if (!(mycard.y == 1 && (mycard.x == 4 || mycard.x == 5)))
				mycard.isSelectable = false;
		}

		CardHandler.s.allCards[4, 1].cBase.dynamicCardID = 2;
		CardHandler.s.allCards[5, 1].cBase.dynamicCardID = 2;

		yield return new WaitUntil (() => ScoreBoardManager.s.allScores[0,0] > 0);

		cardSelectStuff.SetActive (false);
		scoreStuff.SetActive (true);
		myText.text = "Başardın! Skorun 5 olana kadar kart eşleştirmeye devam et!";

		yield return new WaitUntil (() => Input.GetMouseButtonDown(0));

		foreach (IndividualCard mycard in CardHandler.s.allCards) {
			if(!(mycard.y == 1 && (mycard.x == 4 || mycard.x == 5)))
				mycard.isSelectable = true;
		}

		myPanel.SetActive (false);

		yield return null;
	}
}

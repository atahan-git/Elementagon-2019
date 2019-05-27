using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardChecker : MonoBehaviour {

	public static CardChecker s;

	// Use this for initialization
	void Start () {
		s = this;
	}

	public void CheckCards (List<IndividualCard> cardsToCheck, bool isInstant) {
		IndividualCard[] _cardsToCheck = (IndividualCard[])cardsToCheck.ToArray().Clone ();

		//empty our original array
		EmptyArray (cardsToCheck);

		if (isInstant) {
			CheckCards (DataHandler.s.myPlayerInteger, _cardsToCheck);
		} else {
			StartCoroutine (CheckCardsCOROT (DataHandler.s.myPlayerInteger, _cardsToCheck));
		}
	}

	public void CheckCards (IndividualCard[] cardsToCheck, bool isInstant) {
		IndividualCard[] _cardsToCheck = (IndividualCard[])cardsToCheck.Clone ();

		//empty our original array
		EmptyArray (cardsToCheck);

		if (isInstant) {
			CheckCards (DataHandler.s.myPlayerInteger, _cardsToCheck);
		} else {
			StartCoroutine (CheckCardsCOROT (DataHandler.s.myPlayerInteger, _cardsToCheck));
		}
	}

	public void CheckCards (int playerId,List<IndividualCard> cardsToCheck, bool isInstant) {
		IndividualCard[] _cardsToCheck = (IndividualCard[])cardsToCheck.ToArray ().Clone ();

		//empty our original array
		EmptyArray (cardsToCheck);

		if (isInstant) {
			CheckCards (playerId, _cardsToCheck);
		} else {
			StartCoroutine (CheckCardsCOROT (playerId, _cardsToCheck));
		}
	}

	IEnumerator CheckCardsCOROT(int myPlayerinteger, IndividualCard[] cardsToCheck) {
		int totalMatched = 0;
		//check Cards
		for (int l = 0; l < cardsToCheck.Length; l++) {
			if (cardsToCheck [l] != null) {
				if (cardsToCheck[l].cBase != null) {
					for (int k = l+1; k < cardsToCheck.Length; k++) {
						if (cardsToCheck [k] != null && cardsToCheck[l] != null) {
							if (cardsToCheck[k].cBase != null && cardsToCheck[l].cBase != null) {
								if (k != l) {
									if (cardsToCheck [k].cBase.cardType == cardsToCheck [l].cBase.cardType) {

										//Ultimate Poison Alert - player matched two poison cards
										if (cardsToCheck [k].isPoison) {
											PowerUpManager.s.ChoosePoisonCard (myPlayerinteger, cardsToCheck [k], "cardchecker");
											//PowerUpManager.s.ChoosePoisonCard (cardsToCheck [l]);
											ScoreBoardManager.s.AddScore (myPlayerinteger, 0, -GS.a.powerUpSettings.poison_combo, false); //this is just poison score
										} else {

											//actual score adding happens here
											ComboDealer.s.ProcessCombo (myPlayerinteger, true, cardsToCheck[k], cardsToCheck[l]);
											totalMatched++;


											//gfxs
											IndividualCard.MatchCards (myPlayerinteger, cardsToCheck[k],cardsToCheck[l]);

											//networking
											DataHandler.s.SendPlayerAction (cardsToCheck[k].x, cardsToCheck[k].y, CardHandler.CardActions.Match, cardsToCheck[l].x, cardsToCheck[l].y);
										}
										yield return new WaitForSeconds (0.1f);
									}
								}
							}
						}
					}

					//normal poison alert - just one poison card
					if (cardsToCheck [l].isPoison) {
						PowerUpManager.s.ChoosePoisonCard (myPlayerinteger, cardsToCheck [l], "cardchecker"); //this is just poison score
					}
				}
			}
		}

		yield return new WaitForSeconds (0.5f);


		if (totalMatched == 0)
			ComboDealer.s.NotMatched ();
		//Rotate unused cards
		UnSelectCards(cardsToCheck);
		//empty our array because no need left
		EmptyArray(cardsToCheck);
	}

	public void CheckCards(int myPlayerinteger, IndividualCard[] cardsToCheck) {
		int totalMatched = 0;
		//check Cards
		for (int l = 0; l < cardsToCheck.Length; l++) {
			if (cardsToCheck [l] != null) {
				if (cardsToCheck [l].cBase != null) {
					for (int k = 1; k < cardsToCheck.Length; k++) {
						if (cardsToCheck[k] != null && cardsToCheck[l] != null) {
							if (cardsToCheck[k].cBase != null && cardsToCheck[l].cBase != null) {
								if (k != l) {

									if (cardsToCheck [k].cBase.cardType == cardsToCheck [l].cBase.cardType) {
										//Ultimate Poison Alert - player matched two poison cards
										if (cardsToCheck [k].isPoison) {
											PowerUpManager.s.ChoosePoisonCard (myPlayerinteger, cardsToCheck [k], "cardchecker");
											//PowerUpManager.s.ChoosePoisonCard (myPlayerinteger, cardsToCheck [l]);
											ScoreBoardManager.s.AddScore (myPlayerinteger, 0, -GS.a.powerUpSettings.poison_combo, false);
										} else {

											//actual score adding happens here
											ComboDealer.s.ProcessCombo (myPlayerinteger, true, cardsToCheck[k], cardsToCheck[l]);
											totalMatched++;


											//gfxs
											IndividualCard.MatchCards (myPlayerinteger, cardsToCheck[k], cardsToCheck[l]);

											//networking
											DataHandler.s.SendPlayerAction (cardsToCheck[k].x, cardsToCheck[k].y, CardHandler.CardActions.Match, cardsToCheck[l].x, cardsToCheck[l].y);
										}
									}
								}
							}
						}
					}

					//check for poison
					if (cardsToCheck [l].isPoison) {
						PowerUpManager.s.ChoosePoisonCard (myPlayerinteger, cardsToCheck [l], "cardchecker");
					}
				}
			}
		}


		if (totalMatched == 0)
			ComboDealer.s.NotMatched ();
		//Rotate unused cards
		UnSelectCards (cardsToCheck);
		//empty our array because no need left
		EmptyArray(cardsToCheck);

	}

	public void UnSelectCards (IndividualCard[] cardsToCheck){
		for (int l = 0; l < cardsToCheck.Length; l++) {
			if (cardsToCheck [l] != null) {
				if (cardsToCheck[l].cBase != null) {
					cardsToCheck [l].UnSelectCard ();
					DataHandler.s.SendPlayerAction (cardsToCheck [l].x, cardsToCheck [l].y, CardHandler.CardActions.UnSelect);
				}
			}
		}
	}

	public void UnSelectCards (List<IndividualCard> cardsToCheck) {
		for (int l = 0; l < cardsToCheck.Count; l++) {
			if (cardsToCheck[l] != null) {
				if (cardsToCheck[l].cBase != null) {
					cardsToCheck[l].UnSelectCard ();
					DataHandler.s.SendPlayerAction (cardsToCheck[l].x, cardsToCheck[l].y, CardHandler.CardActions.UnSelect);
				}
			}
		}
	}

	public void EmptyArray (IndividualCard[] cardsToCheck){
		for (int i = 0; i < cardsToCheck.Length; i++) {
			cardsToCheck[i] = null;
		}
	}

	public void EmptyArray (List<IndividualCard> cardsToCheck) {
		cardsToCheck.Clear();
	}


	public delegate void Callback ();
}

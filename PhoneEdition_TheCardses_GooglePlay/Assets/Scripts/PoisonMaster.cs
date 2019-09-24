using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonMaster : MonoBehaviour
{
	public static PoisonMaster s;

	public GameObject deadlyPoisonEffect;
	public GameObject deadlyPoisonExplodeEffect;

	public GameObject findTheCureUIEffect;

	private void Start () {
		s = this;
		findTheCureUIEffect.SetActive (false);
	}

	Queue<DeadlyRoutineHolder> ActivatedDeadlyPoisons = new Queue<DeadlyRoutineHolder> ();
	//This is used when locally selecting a posion card. Network poisoning happens differently+
	public void ChoosePoisonCard (int myPlayerinteger, IndividualCard myCard, string message) {
		DataLogger.LogMessage("Player: " + myPlayerinteger.ToString() + " picked Poison card!" + myCard.cBase.myPoisonType + " - " + message);

		switch (myCard.cBase.myPoisonType) {
		case CardBase.PoisonTypes.DeadlyPoison:
			DeadlyRoutineHolder myHolder = new DeadlyRoutineHolder(myCard);
			Coroutine myRout = StartCoroutine(DeadlyPoisonCard(myPlayerinteger, myCard, myHolder));
			myHolder.routine = myRout;
			ActivatedDeadlyPoisons.Enqueue(myHolder);
			findTheCureUIEffect.SetActive(true);
			break;
		case CardBase.PoisonTypes.PoisonCure:
			if (CureDeadlyPoison()) {
				myCard.SelectCard(myPlayerinteger);
				myCard.Invoke("MatchCard", 1f);
			} else {
				myCard.SelectCard(myPlayerinteger);
				myCard.Invoke("UnSelectCard", 1f);
			}
			break;
		}
	}

	public bool CureDeadlyPoison () {
		if (ActivatedDeadlyPoisons.Count > 0) {
			DeadlyRoutineHolder toCure = ActivatedDeadlyPoisons.Dequeue ();
			StopCoroutine (toCure.routine);
			Destroy (toCure.effect);
			toCure.card.Invoke ("MatchCard", 1f);
			if (ActivatedDeadlyPoisons.Count == 0)
				findTheCureUIEffect.SetActive (false);

			return true;
		} else {
			return false;
		}
	}

	public class DeadlyRoutineHolder {
		public Coroutine routine;
		public IndividualCard card;
		public GameObject effect;

		public DeadlyRoutineHolder (IndividualCard _card) {
			card = _card;
		}
	}

	IEnumerator DeadlyPoisonCard (int myPlayerinteger, IndividualCard myCard, DeadlyRoutineHolder holder) {
		myCard.SelectCard (myPlayerinteger);
		float timer = myCard.cBase.poisonAmount;

		GameObject myEffect = Instantiate (deadlyPoisonEffect, myCard.transform.position, Quaternion.identity);
		myEffect.GetComponent<DeadlyPoisonEffect> ().SetUp (timer);

		holder.effect = myEffect;

		yield return new WaitForSeconds (timer);

		LocalPlayerController.s.canSelect = false;
		GameObjectiveMaster.s.isGamePlaying = false;

		Instantiate (deadlyPoisonExplodeEffect, myCard.transform.position, Quaternion.identity);

		yield return new WaitForSeconds (1f);

		GameObjectiveMaster.s.EndGame (GameObjectiveMaster.s.WinnerID(false));

		yield return null;
	}
}

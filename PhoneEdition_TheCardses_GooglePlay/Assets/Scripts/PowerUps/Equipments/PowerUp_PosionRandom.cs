using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PowerUp_PosionRandom : PowerUp_Active_Instant {
	public float poisonWaitAmount = 1f;

	public override void Enable (int _elementalType, int _power, float _amount) {
		base.Enable (_elementalType, _power, _amount);

		StartCoroutine (_Enable (power, amount));
	}

	IEnumerator _Enable (int _power, float _amount) {

		yield return new WaitForSeconds (0.1f);

		int n = 0;
		while (n < _amount) {

			List<IndividualCard> myCards = GetRandomizedOccupiedOrSelectabeCardList ();
			power = Mathf.Clamp (power, 0, myCards.Count);

			for (int i = 0; i < power; i++) {
				Activate (myCards[i], n);
				if (activateEffect.GetComponent<ThrowToCardEffect> () != null)
					activateEffect.GetComponent<ThrowToCardEffect> ().waitTime = n == 0 ? 0.5f : 0.2f;
				//yield return new WaitForSeconds (0.05f);
			}

			yield return new WaitForSeconds (n == 0 ? 0.75f : 0.45f);

			PoisonSelectedCards ();

			UnSelectSelectedCards (poisonWaitAmount + (0.05f * _power));

			n++;
		}

		Disable ();
	}

	public void Activate (IndividualCard myCard, float n) {
		base.Activate (myCard);

		StartCoroutine (_Activate (myCard, n));
	}

	IEnumerator _Activate (IndividualCard myCard, float n) {
		yield return new WaitForSeconds (n == 0 ? 0.7f : 0.4f);

		Select (myCard);
	}
}
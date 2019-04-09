using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PowerUp_PosionArea : PowerUp_Active_Instant {
	public override void Enable (int _elementalType, int _power, float _amount) {
		base.Enable (_elementalType, _power, _amount);

		StartCoroutine (_Enable(power,amount));
	}

	IEnumerator _Enable (int power, float amount) {
		yield return new WaitForSeconds (0.4f);

		List<IndividualCard> myCards = GetRandomizedSelectabeCardList ();
		power = Mathf.Clamp (power, 0, myCards.Count);

		for (int i = 0; i < power; i++) {
			if (myCards[i].isRevealed) {
				power++;
				if (power > myCards.Count) {
					break;
				} else {
					continue;
				}
			}
			print ("Revealing: " + myCards[i].x.ToString () + " - " + myCards[i].y.ToString ());
			Reveal (myCards[i], amount);
			yield return new WaitForSeconds (0.05f);
		}

		Disable ();
	}
}
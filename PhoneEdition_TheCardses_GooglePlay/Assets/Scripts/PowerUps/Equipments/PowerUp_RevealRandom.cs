using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PowerUp_RevealRandom : PowerUp_Active_Instant {
	public override void Enable (int _power, float _amount, Color _effectColor) {
		base.Enable (_power, _amount, _effectColor);

		StartCoroutine (_Enable(power,amount));
	}

	IEnumerator _Enable (int _power, float _amount) {
		yield return new WaitForSeconds (0.4f);

		List<IndividualCard> myCards = GetRandomizedSelectabeCardList ();
		_power = Mathf.Clamp (_power, 0, myCards.Count);

		for (int i = 0; i < _power; i++) {
			if (myCards[i].isRevealed) {
				_power++;
				if (_power > myCards.Count) {
					break;
				} else {
					continue;
				}
			}
			//print ("Revealing: " + myCards[i].x.ToString () + " - " + myCards[i].y.ToString ());
			Reveal (myCards[i], _amount);
			yield return new WaitForSeconds (0.05f);
		}

		Disable ();
	}
}
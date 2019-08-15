using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_AddScore : PowerUp_Active_Instant {
	public override void Enable (int _power, float _amount, Color _effectColor) {
		base.Enable (_power, _amount, _effectColor);

		StartCoroutine (_Enable (power, amount));
	}

	IEnumerator _Enable (int _power, float _amount) {

		yield return new WaitForSeconds (0.5f);

		int n = 0;
		while (n < _amount) {

			ScoreBoardManager.s.AddScore (DataHandler.s.myPlayerInteger, 0, 1, false);

			yield return new WaitForSeconds (0.3f);

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_AddTime : PowerUp_Active_Instant {
	public override void Enable (int _elementalType, int _power, float _amount) {
		base.Enable (_elementalType, _power, _amount);

		StartCoroutine (_Enable (power, amount));
	}

	IEnumerator _Enable (int _power, float _amount) {

		yield return new WaitForSeconds (0.1f);

		int n = 0;
		while (n < _amount) {

			GameObjectiveFinishChecker.s.timer -= GS.a.timer > 0 ? -1f : 1f;

			yield return new WaitForSeconds (0.1f);

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
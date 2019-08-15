using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PowerUp_PosionArea : PowerUp_Active_Select {
	public float poisonWaitAmount = 1f;

	public override void Enable (int _power, float _amount, Color _effectColor) {
		base.Enable (_power, _amount, _effectColor);

		HookSelf (true);
	}

	//Power level mapping ->
	/* 1->1
	 * 2->3
	 * 3->5
	 * 4->7
	 */
	public override void Activate (IndividualCard myCard) {
		base.Activate (myCard);

		StartCoroutine (_Activate (myCard, 1 + ((power-1) * 2), amount));
	}

	IEnumerator _Activate (IndividualCard myCard, int _power, float _amount) {

		yield return new WaitForSeconds (0.1f);

		for (int i = 0; i < _power; i++) {
			Select (GetAreaSequence (myCard, i, _power));
			if (i % 4 == 0)
				yield return new WaitForSeconds (0.03f);
		}

		PoisonSelectedCards ();

		UnSelectSelectedCards (poisonWaitAmount + (0.05f * _power));

		amount--;
		if (amount <= 0) {
			Disable ();
		}
	}
}
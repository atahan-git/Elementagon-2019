using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PowerUp_MatchArea : PowerUp_Active_Select {
	public override void Enable (int _elementalType, int _power, float _amount) {
		base.Enable (_elementalType, _power, _amount);

		HookSelf (true);
	}

	//Power level mapping ->
	/* 1->3
	 * 2->5
	 * 3->7
	 * 4->9
	 */
	public override void Activate (IndividualCard myCard) {
		base.Activate (myCard);
		
		StartCoroutine (_Activate (myCard, 1 + (power * 2), amount));
	}
	
	IEnumerator _Activate (IndividualCard myCard, int _power, float _amount) {

		yield return new WaitForSeconds (0.1f);

		for (int i = 0; i < _power; i++) {
			Select (GetAreaSequence (myCard, i, _power));
			if (i % 4 == 0)
				yield return new WaitForSeconds (0.03f);
		}

		CheckCards (0.4f + (0.02f * _power));

		amount--;
		if (amount <= 0) {
			Disable ();
		}
	}
}
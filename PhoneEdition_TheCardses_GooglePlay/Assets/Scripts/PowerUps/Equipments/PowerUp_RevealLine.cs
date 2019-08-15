using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PowerUp_RevealLine : PowerUp_Active_Select {
	public override void Enable (int _power, float _amount, Color _effectColor) {
		base.Enable (_power, _amount, _effectColor);

		HookSelf (true);
	}

	public override void Activate (IndividualCard myCard) {
		base.Activate (myCard);

		StartCoroutine (_Activate (myCard, power, amount));
	}

	IEnumerator _Activate (IndividualCard myCard, int _power, float _amount) {

		for (int i = 0; i < _power; i++) {
			Reveal (GetLineSequence (myCard, i, _power),_amount);
			yield return new WaitForSeconds (0.03f);
		}
		
		Disable ();
	}
}
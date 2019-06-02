using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PowerUp_PosionLine : PowerUp_Active_Select {
	public float poisonWaitAmount = 1f;


	public override void Enable (int _elementalType, int _power, float _amount) {
		base.Enable (_elementalType, _power, _amount);

		HookSelf (true);
	}

	public override void Activate (IndividualCard myCard) {
		base.Activate (myCard);

		StartCoroutine (_Activate (myCard, power, amount));
	}

	IEnumerator _Activate (IndividualCard myCard, int _power, float _amount) {
		activateEffect.transform.localScale = new Vector3 (-((_power - 1f) / 1.9f), 1f + ((_power - 1.5f) / 7f), 1);

		yield return new WaitForSeconds (0.1f);

		for (int i = 0; i < _power; i++) {
			Select (GetLineSequence (myCard, i, _power));
			yield return new WaitForSeconds (0.03f);
		}

		PoisonSelectedCards ();

		UnSelectSelectedCards (poisonWaitAmount + (0.05f * _power));

		amount--;
		if (amount <= 0) {
			Disable ();
		}
	}

	public override void NetworkedActivate (int player, IndividualCard card, int _power, float _amount) {
		base.NetworkedActivate (player, card, _power, _amount);
		networkActiveEffect.transform.localScale = new Vector3 (-((_power - 1f) / 1.9f), 1f + ((_power - 1.5f) / 7f), 1);
	}
}
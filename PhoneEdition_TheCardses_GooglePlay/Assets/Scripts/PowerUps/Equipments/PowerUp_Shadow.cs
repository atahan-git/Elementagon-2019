using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PowerUp_Shadow : PowerUp_Active_Select {
	public override void Enable (int _elementalType, int _power, float _amount) {
		base.Enable (_elementalType, _power, _amount);

		Invoke ("Disable",(_power*3)+2);

		HookSelf (true);
	}

	public override void Activate (IndividualCard myCard) {
		base.Activate (myCard);

		Select (myCard);
	}


	public override void Disable () {
		CheckCards ();

		base.Disable ();
	}
}
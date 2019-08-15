using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PowerUp_Shadow : PowerUp_Active_Select {
	public override void Enable (int _power, float _amount, Color _effectColor) {
		base.Enable (_power, _amount, _effectColor);

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
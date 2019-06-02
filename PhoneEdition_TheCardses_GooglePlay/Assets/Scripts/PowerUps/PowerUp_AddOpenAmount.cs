using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PowerUp_AddOpenAmount : PowerUp_Passive_Always {
	public override void Enable (int _elementalType, int _power) {
		base.Enable (_elementalType, _power);

		LocalPlayerController.s.SetHandSize (2+_power);
	}
}
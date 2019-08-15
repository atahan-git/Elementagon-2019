using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PowerUp_AddOpenAmount : PowerUp_Passive_Always {
	public override void Enable (int _power, Color _effectColor) {
		base.Enable (_power, _effectColor);

		LocalPlayerController.s.SetHandSize (2+_power);
	}


}
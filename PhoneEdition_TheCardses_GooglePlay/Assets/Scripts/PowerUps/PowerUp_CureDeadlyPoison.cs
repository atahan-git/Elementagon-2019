using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_CureDeadlyPoison : PowerUp_Active_Instant {
	public override void Enable (int _power, float _amount, Color _effectColor) {
		base.Enable (_power, _amount, _effectColor);

		StartCoroutine (_Enable (power, amount));
	}

	IEnumerator _Enable (int _power, float _amount) {

		yield return new WaitForSeconds (0.5f);

		PoisonMaster.s.CureDeadlyPoison ();

		Disable ();
	}
}

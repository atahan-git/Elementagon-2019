using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PowerUp_Earth : PowerUp_Active_Select {
	public override void Enable (int _elementalType, int _power, float _amount) {
		if (_amount == 1) {
			_amount = _power * 2;
			_power = 2;
		}
		base.Enable (_elementalType, _power, _amount);

		counter = 0;

		HookSelf (true);
	}

	int counter = 0;
	public override void Activate (IndividualCard myCard) {
		base.Activate (myCard);

		Select (myCard);
		if(counter <= power)
		Select (GetRandomizedOccupiedOrSelectabeCardList()[0],true);

		counter++;

		if (counter >= LocalPlayerController.s.handSize) {
			CheckCards (GS.a.playerSelectionSettings.checkSpeedPlayer + (0.02f * counter));

			counter = 0;
			amount--;
		}

		if (amount <= 0) {
			Disable ();
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PowerUp_Ice : PowerUp_Active_Effect {
	public override void Enable (int _power, float _amount, Color _effectColor) {
		base.Enable (_power, _amount, _effectColor);

		StartCoroutine (_Enable (power, amount));
	}

	IEnumerator _Enable (int _power, float _amount) {

		Invoke ("Disable", (_power * 10) + 30);

		foreach (NPCBase npc in NPCManager.s.ActiveNPCS)
			npc.isFrozen = true;
		
		yield return null;
	}


	public override void Disable () {
		base.Disable ();

		foreach (NPCBase npc in NPCManager.s.ActiveNPCS)
			npc.isFrozen = false;

	}

	public override void NetworkedEnable (int player, int _power, float _amount) {
		CoolIceEnabler.s.isEnabled = true;
		LocalPlayerController.s.canSelect = false;


		foreach (NPCBase npc in NPCManager.s.ActiveNPCS)
			npc.isFrozen = true;
	}

	public override void NetworkedDisable (int player, int _power, float _amount) {
		CoolIceEnabler.s.isEnabled = false;
		LocalPlayerController.s.canSelect = true;

		foreach (NPCBase npc in NPCManager.s.ActiveNPCS)
			npc.isFrozen = false;
	}
}
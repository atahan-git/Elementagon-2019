using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _NPC_IceDragon : _NPCBehaviour {

	public float freezeTime = 7f;
	public Vector2 freezeInterval = new Vector2 (15, 20);
	public Vector3 pickSettings = new Vector3 (0.6f, 1.2f, 0.3f);

	// Use this for initialization
	void Start () {
		SetUp (0, freezeInterval.x, freezeInterval.y, 3, 2);
		NormalSelectSetUp (1, pickSettings.x, pickSettings.y, 0.5f, 3, pickSettings.z, false);
	}

	public override void Activate (){
		//PowerUpManager.s.ReceiveEnemyPowerUpActions (DataHandler.NPCInteger, -1, -1, PowerUpManager.PowerUpType.Ice, PowerUpManager.ActionType.Enable);
		Invoke ("dis", freezeTime);
	}

	void dis (){
		//PowerUpManager.s.ReceiveEnemyPowerUpActions (DataHandler.NPCInteger, -1, -1, PowerUpManager.PowerUpType.Ice, PowerUpManager.ActionType.Disable);
	}
}

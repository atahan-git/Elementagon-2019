using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUp", menuName = "GameSettings/PowerUp", order = 7)]
public class PowerUpSettings : ScriptableObject {

	public int earth_activeCount = 7;
	public float earth_checkSpeed = 0.35f;
	[Space]
	public float ice_activeTime = 14f;
	[Space]
	public float light_activeTime = 14f;
	[Space]
	public float poison_activeTime = 2f;
	public float poison_checkSpeed = 4f;
	public int poison_damage = 5;
	public int poison_combo = 5;
	[Space]
	public float shadow_activeTime = 4f;
	[Tooltip("deprecated")]
	public int shadow_activeCount = 7;
}
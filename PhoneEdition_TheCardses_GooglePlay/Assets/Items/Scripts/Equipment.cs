using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName = "New Equipment", menuName = "Item/Equipment")]
public class Equipment : ItemBase {

	public enum Type {
		revealRandom = 0, revealArea = 1, revealLine = 2,
		matchRandom = 3, matchArea = 4, matchLine = 5,
		poisonRandom = 6, posionArea = 7, poisonLine= 8, neutralizePosion = 9,
		addOpenAmount = 10,
		increaseDamage = 11, increaseScoreMult = 12,
		slowDownTime = 13,
		earth = 14
	}

	public Type myType;

    public int power = 1;
    public float amount = -1f;

    [Tooltip ("//--------CARD TYPES---------\n// 0 = any type\n// 1-7 = normal cards\n// 8-14 = dragons\n//---------------------------\n// 1 = Earth\n// 2 = Fire\n// 3 = Ice\n// 4 = Light\n// 5 = Nether\n// 6 = Poison\n// 7 = Shadow\n//---------------------------\n// 8 = Earth Dragon\n// 9 = Fire Dragon\n//10 = Ice Dragon\n//11 = Light Dragon\n//12 = Nether Dragon\n//13 = Poison Dragon\n//14 = Shadow Dragon\n//---------------------------")]
    public int[] chargeReq = new int[16];

}
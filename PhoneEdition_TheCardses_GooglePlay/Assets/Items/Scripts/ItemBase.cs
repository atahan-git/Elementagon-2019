using System.Collections;
using System.Collections.Generic;
using UnityEngine;


abstract public class ItemBase : ScriptableObject {

	public int ID;

	[Tooltip ("//--------CARD TYPES---------\n// 0 = any type\n// 1-7 = normal cards\n// 8-14 = dragons\n//---------------------------\n// 1 = Earth\n// 2 = Fire\n// 3 = Ice\n// 4 = Light\n// 5 = Nether\n// 6 = Poison\n// 7 = Shadow\n//---------------------------\n// 8 = Earth Dragon\n// 9 = Fire Dragon\n//10 = Ice Dragon\n//11 = Light Dragon\n//12 = Nether Dragon\n//13 = Poison Dragon\n//14 = Shadow Dragon\n//---------------------------")]
	public int elementalType;

    public new string name;
    public Sprite sprite;
	public Sprite cardSprite;

	[Multiline]
    public string description;

	public int durability;
}

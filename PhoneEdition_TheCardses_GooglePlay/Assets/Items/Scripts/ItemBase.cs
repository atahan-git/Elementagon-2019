using System.Collections;
using System.Collections.Generic;
using UnityEngine;


abstract public class ItemBase : ScriptableObject {

	public int ID;

	[Tooltip ("leave at 0 alpha to use default color")]
	public Color effectColor;

    public new string name;
    public Sprite sprite;
	public Sprite cardSprite;

	[Multiline]
    public string description;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Recipe", menuName = "Item/Recipe")]
public class Recipe : ScriptableObject {

	public ItemBase[] requiredItems = new ItemBase[1];
	public int[] requiredAmounts = new int[1];

	public ItemBase resultingItem;
	public int resultingAmount = 1;
}

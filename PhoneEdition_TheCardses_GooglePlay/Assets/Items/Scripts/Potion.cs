using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName = "New Potion", menuName = "Item/Potion")]
public class Potion : ItemBase {

    public enum Type {timePotion, scorePotion, healingPotion, damagingPotion, chargePotion, cureDeadlyPoison}

    public Type myType;

    public int amount = 1;
}

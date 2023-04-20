using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public new string name = "New Item";
    public Sprite icon = null;
    public bool isDefaultItem = false;

    public virtual void Use()
    {
        Debug.Log("Using: " + name);
    }


    /*public bool isSelected = false;
    public bool isEquipped = false;
    public bool isStackable = false;
    public int maxStack = 1;
    public int currentStack = 1;
    public int itemID = 0;
    public int itemValue = 0;
    public int itemWeight = 0;
    public int itemDurability = 0;
    public int itemMaxDurability = 0;
    public int itemLevel = 0;
    public int itemRarity = 0;
    public int itemQuality = 0;
    public int itemDamage = 0;
    public int itemArmor = 0;
    public int itemMagic = 0;
    public int itemRange = 0;
    public int itemSpeed = 0;
    public int itemCrit = 0;
    public int itemDefense = 0;
    public int itemHealth = 0;
    public int itemMana = 0;
    public int itemStamina = 0;*/

}

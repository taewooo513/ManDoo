using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class InventorySlot // 없어도 될거같음.
{
    public Item item;
    public bool IsEmpty => item == null;
}


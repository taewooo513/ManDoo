using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventorySlot : MonoBehaviour // 없어도 될거같음
{
    public Item item;
    public bool IsEmpty => item == null;
}


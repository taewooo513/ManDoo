﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventorySlot : MonoBehaviour
{
    public Item item;
    public bool IsEmpty => item == null;
}


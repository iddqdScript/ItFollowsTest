using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemEventArgs : EventArgs
{
    public InventoryItemEventArgs(InventoryItemBase item)
    {
        Item = item;
    }

    public InventoryItemBase Item;
}


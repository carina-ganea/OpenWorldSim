using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[Serializable]
public class InventorySlot 
{
    // Start is called before the first frame update
    InventoryManager inventory;
    public Collectible item { get; private set; }
    public int stackSize { get; private set; }

    public InventorySlot(Collectible source)
    {
        item = source;
        AddItem();
    }


    public void AddItem()
    {
        stackSize++;
    }

    public void RemoveItem()
    {
        stackSize--;
    }
}

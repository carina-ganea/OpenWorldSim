using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    InventoryManager inventory;
    InventorySlot[] slots;
    #region Singleton

    public static PlayerManager instance;
        
    void Awake(){
        instance = this;
    }

    #endregion

    public GameObject player;
    void Start()
    {
        //inventory = InventoryManager.instance;
        //inventory.onInventoryChangedCallback += UpdateUI; //definesc o metoda ca se apeleaza la aparitia unui eveniment delegat

        //slots = GetComponentsInChildren<InventorySlot>(); //fiecare slot din inventar
    }
/*
    void UpdateUI()
    {

        //actualizare fiecare slot
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count) 
                slots[i].AddItem(null);
            else slots[i].RemoveItem(null);
        }

    }*/
}

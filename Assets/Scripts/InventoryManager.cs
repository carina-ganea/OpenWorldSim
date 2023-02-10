using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public delegate void OnInventoryChanged();
    public OnInventoryChanged onInventoryChangedCallback;
    
    // singleton
    
    public static InventoryManager instance { get; private set; }
    public List<InventorySlot> items { get; private set; }
    private Dictionary<Collectible, InventorySlot> itemDictionary;
    public GameObject inventoryUI;
    public GameObject insideInventoryUI;

    void OnStart()
    {
        
    }
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        instance = this;

        items = new List<InventorySlot>();
        itemDictionary = new Dictionary<Collectible, InventorySlot>();
    }

    //lista de obiecte
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }

    //metode pentru gestionare
    public void Add(Collectible item)
    {
        if(itemDictionary.TryGetValue(item, out InventorySlot value))
        {
            value.AddItem();
        } else
        {
            InventorySlot newItem = new InventorySlot(item);
            items.Add(newItem);
            itemDictionary.Add(item, newItem);
        }
        
        onInventoryChangedCallback.Invoke(); //notifica despre modificare
    }

    public void Remove(Collectible item)
    {
        if (itemDictionary.TryGetValue(item, out InventorySlot value))
        {
            value.RemoveItem();
        
            if(value.stackSize == 0)
            {
                items.Remove(value);
                itemDictionary.Remove(item);
            }
        }

        onInventoryChangedCallback.Invoke(); //notifica despre modificare
    }

    public void Use(Collectible item)
    {
        Debug.Log("Reached Use");
        if (itemDictionary.TryGetValue(item, out InventorySlot value))
        {
            GameObject player = GameObject.Find("Player-URP");
            CharacterStats stats = player.GetComponent<CharacterStats>();
            if(item.name == "Attack Potion")
            {
                stats.Atk += item.value;
            } else
            {
                stats.currentHP += item.value;
                stats.UpdateHealth();
            }
            value.RemoveItem();

            if (value.stackSize == 0)
            {
                items.Remove(value);
                itemDictionary.Remove(item);
            }
        }

        onInventoryChangedCallback.Invoke(); //notifica despre modificare
    }

    public InventorySlot Get(Collectible item)
    {
        if (itemDictionary.TryGetValue(item, out InventorySlot value))
        {
            return value;
        }
        return null;
    }
}

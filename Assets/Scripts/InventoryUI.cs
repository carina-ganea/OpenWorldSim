using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    InventoryManager inventory = InventoryManager.instance;
    public GameObject slotPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
        InventoryManager.instance.onInventoryChangedCallback += OnUpdateInventory;
    }


    // Update is called once per frame
    void OnUpdateInventory()
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }

        DrawInventory();
    }

    void DrawInventory()
    {
        foreach(InventorySlot item in InventoryManager.instance.items)
        {
            AddInventorySlot(item);
        }
    }

    void AddInventorySlot(InventorySlot item)
    {
        GameObject obj = Instantiate(slotPrefab);
        obj.GetComponentInChildren<RawImage>().texture = item.item.icon.texture;
        obj.transform.SetParent(transform, false);

        UiInventorySlot slot = obj.GetComponent<UiInventorySlot>();
        slot.Set(item);
    }
}

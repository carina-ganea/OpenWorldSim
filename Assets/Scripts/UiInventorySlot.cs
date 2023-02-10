using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiInventorySlot : MonoBehaviour
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Sprite selectedIcon;
    [SerializeField]
    private TextMeshProUGUI label;
    [SerializeField]
    private GameObject stackObject;
    [SerializeField]
    private TextMeshProUGUI stackLabel;
    [SerializeField]
    private Collectible obj;

    public bool selected;
    public Sprite unselectedIcon;

    public void Set(InventorySlot item)
    {
        selected = false;
        obj = item.item;
        
        icon.sprite = item.item.icon;
        unselectedIcon = item.item.icon;
        selectedIcon = item.item.selectedIcon;
        label.text = item.item.name;
        if (item.stackSize <= 1)
        {
            stackObject.SetActive(false);
            return;
        }

        stackLabel.text = item.stackSize.ToString();
        
    }

    public void Select()
    {
        selected = !selected;
        OnSelect();
    }

    public void OnSelect()
    {
        if (selected)
        {
            icon.sprite = selectedIcon;
            GameObject recycle = GameObject.Find("RecycleButton");
            recycle.GetComponent<Button>().onClick.AddListener(delegate { InventoryManager.instance.Remove(obj); });
        
            GameObject.Find("UseButton").GetComponent<Button>().onClick.AddListener(delegate { InventoryManager.instance.Use(obj); });
            DontDestroyOnLoad(recycle);
        } else
        {
            icon.sprite = unselectedIcon;
            GameObject recycle = GameObject.Find("RecycleButton");
            recycle.GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("UseButton").GetComponent<Button>().onClick.RemoveAllListeners();
        }
        
    }
}

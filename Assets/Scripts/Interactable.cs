using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 3f;
    bool isFocus = false;
    bool open = false;
    public Collectible item;
    public LayerMask OpenableMask;
  
    InventoryManager inventory = InventoryManager.instance;

    public virtual void Interact(int dmg, bool crit)
    {
        
    }

    void Update() {
        if(1 << gameObject.layer == OpenableMask)
        {
            if (open){
                transform.GetChild(1).transform.rotation =  Quaternion.Slerp(transform.GetChild(1).transform.rotation, new Quaternion(0f,0f,-45f, 1f), Time.deltaTime * 0.1f);
            }
            if (!open){
                transform.GetChild(1).transform.rotation =  Quaternion.Slerp(transform.GetChild(1).transform.rotation, new Quaternion(0f,0f,0f, 1f), Time.deltaTime * 2f);
            }
        }
    }

    public void OpenOrClose(){
        open = !open;
    }

    void OnDrawGizmosSelected () 
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void OnHandlePickUp()
    {
        InventoryManager.instance.Add(item);

        gameObject.SetActive(false);
    }
}

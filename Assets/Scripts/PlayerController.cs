using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Retro.ThirdPersonCharacter
{
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Combat))]
[RequireComponent(typeof(CharacterStats))]
public class PlayerController : MonoBehaviour
{
    Camera cam;
    private PlayerInput _playerInput;
    private Combat _combat;
    private CharacterStats _stats;


    public Interactable focus = null;
    public LayerMask AttackableMask;
    public LayerMask OpenableMask;
    public LayerMask CollectibleMask;

    public bool normalAttackInProgress;
    public bool specialAttackInProgress;
    // public List<Collider> hits = new List<Collider>();
    public bool newCollision;
        public List<GameObject> reachableItems;

    

    void Start()
    {
        cam = Camera.main;
        _playerInput = GetComponent<PlayerInput>();
        _combat = GetComponent<Combat>();
        _stats = GetComponent<CharacterStats>();
    }

    void Update()
    {
        if (!_combat.AttackInProgress && !normalAttackInProgress && !specialAttackInProgress) {
            if ( _playerInput.AttackInput){
                normalAttackInProgress = true;
                newCollision = true;
                Invoke("ClearNormalAttack", 0.9f);
                // for( int i = 0; i < hits.Count; i++ ){
                //     StartCoroutine(Damage("Normal", hits[i], 0.1f));
                // }
                // hits.Clear();
            }
            if ( _playerInput.SpecialAttackInput){
                specialAttackInProgress = true;
                newCollision = true;
                Invoke("ClearSpecialAttack", 2.8f);
                // for( int i = 0; i < hits.Count; i++ ){
                //     StartCoroutine(Damage("Special", hits[i], 2f));
                // }
                // hits.Clear();
            }
            
        }
        // if (!_combat.AttackInProgress && !normalAttackInProgress && !specialAttackInProgress){
        //     if(_playerInput.AttackInput)
        //     {

            // Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            // RaycastHit hit;

            // if (Physics.Raycast(ray, out hit, 4, AttackableMask)) 
            // {
            //     Interactable interactable = hit.collider.GetComponent<Interactable>();
            //     if( interactable != null)
            //     {
            //         focus = interactable;
            //         // Damage("Normal", hit);
            //     }
            // }

        //         normalAttackInProgress = true;
        //     } else if (_playerInput.SpecialAttackInput){
        //         specialAttackInProgress = true;
        //     } 
        // } 

        if(!_playerInput.MovementInput.Equals(Vector3.zero)){
            focus = null;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
                if (reachableItems.Count >= 1)
                {
                    
                    reachableItems[0].GetComponent<Interactable>().OnHandlePickUp();
                    reachableItems.Remove(reachableItems[0]);
                    
                }
        }
    }

    void ClearNormalAttack(){
        normalAttackInProgress = false;
    }
    
    void ClearSpecialAttack(){
        specialAttackInProgress = false;
    }

    void Damage(string type, Collider hit){
        float dmgVal = 0f;
        bool crit = true;
        if( type == "Normal"){
            dmgVal = (int) _stats.Atk * _stats.Lvl / 80f;
            if(Random.Range(0f,1f) > _stats.critChance){
                crit = false;
            }
            if( crit ){
                dmgVal += dmgVal * _stats.critValue;
            }
            Vector3 hit_position = new Vector3((hit.transform.position.x + transform.position.x) / 2f, 
                                                Mathf.Clamp(Mathf.Abs(hit.transform.position.y), 1.5f, 2.5f), 
                                                (hit.transform.position.z + transform.position.z) / 2f);
            DamagePopUp.Create(hit_position, (int)dmgVal, transform.rotation, crit);
        }
        if ( type == "Special") {
            dmgVal = (int) _stats.skill * _stats.Atk * _stats.Lvl / 80f;
            if(Random.Range(0f,1f) > _stats.critChance){
                crit = false;
            }
            if( crit ){
                dmgVal += dmgVal * _stats.critValue;
            }
            Vector3 hit_position = new Vector3((hit.transform.position.x + transform.position.x) / 2f, 
                                                Mathf.Clamp(Mathf.Abs(hit.transform.position.y), 1.5f, 2.5f), 
                                                (hit.transform.position.z + transform.position.z) / 2f);
            DamagePopUp.Create(hit_position, (int)dmgVal, transform.rotation, crit);
        }
        focus.Interact((int)dmgVal, crit);
    }
        private void OnTriggerEnter(Collider collider)
        {
            if (1 << collider.gameObject.layer == CollectibleMask && !reachableItems.Contains(collider.gameObject))
            {
                reachableItems.Add(collider.gameObject);
                //useLookAt = true;
            }
        }
        private void OnTriggerExit(Collider collision)  {
            if (1 << collision.gameObject.layer == CollectibleMask)
            {
                reachableItems.Remove(collision.gameObject);
                //useLookAt = false;
            }
            if (normalAttackInProgress && newCollision)
        {
            
                if (1 << collision.gameObject.layer == AttackableMask) 
            {
                Interactable interactable = collision.GetComponent<Collider>().GetComponent<Interactable>();
                if( interactable != null)
                {
                    focus = interactable;
                    newCollision = false;
                    StartCoroutine(DelayDamage("Normal", collision, 0.3f));
                }
            }
            if (1 << collision.gameObject.layer == OpenableMask){
                Interactable interactable = collision.transform.GetComponent<Collider>().GetComponent<Interactable>();
                if( interactable != null)
                {
                    focus = interactable;
                    newCollision = false;
                    interactable.OpenOrClose();
                }
            }
            
        }
        if(specialAttackInProgress && newCollision){
            
            if (1 << collision.gameObject.layer == AttackableMask) 
            {
                Interactable interactable = collision.GetComponent<Collider>().GetComponent<Interactable>();
                if( interactable != null)
                {
                    focus = interactable;
                    newCollision = false;
                    StartCoroutine(DelayDamage("Special", collision, 0.8f));
                }
            }
        }
    }

    IEnumerator DelayDamage(string type, Collider hit, float delayTime)
    {
   //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);
        Damage(type, hit);
   //Do the action after the delay time has finished.
    }

}
}
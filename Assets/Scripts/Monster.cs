using System.Data.SqlTypes;
using System.Data;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.ExceptionServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Retro.ThirdPersonCharacter
{
    [RequireComponent(typeof(CharacterStats))]
    public class Monster : Interactable {

        public GameObject hp_ui, HPObj;
        public Transform target;
        public bool isAttacking = false;

        Transform HPbar;
        Image healthSlider;
        Transform cam;
        PlayerManager playerManager;
        CharacterStats stats;

        void Start()
        {   
            playerManager = PlayerManager.instance;
            cam = Camera.main.transform;

            foreach (Canvas c in FindObjectsOfType<Canvas>())
            {
                if (c.renderMode == RenderMode.WorldSpace){
                    HPObj = Instantiate(hp_ui, c.transform);
                    HPbar = HPObj.transform;
                    healthSlider = HPbar.GetChild(0).GetChild(0).GetComponent<Image>();
                    break;
                }
            }

            GetComponent<CharacterStats>().OnHealthChange += OnHealthChange;
            stats = GetComponent<CharacterStats>();
        }

        public override void Interact(int dmg, bool crit)
        {
            base.Interact(dmg, crit);
            Combat playerCombat = playerManager.player.GetComponent<Combat>();
            if (playerCombat != null){
                // playerCombat.Attack();
                GetComponent<CharacterStats>().TakeDamage(dmg, crit);
            }
            
        }

        public void Attack() {
            isAttacking = true;
        }

        void OnTriggerEnter(Collider hit){
            if (hit.gameObject.tag == "Player" && isAttacking){
                playerManager.player.GetComponent<CharacterStats>().TakeDamage(stats.Atk, true);
                Vector3 hit_position = new Vector3(hit.transform.position.x , 
                                                Mathf.Clamp(Mathf.Abs(hit.transform.position.y), 1.5f, 2.5f), 
                                                hit.transform.position.z);
                DamagePopUp.Create(hit_position, stats.Atk, hit.gameObject.transform.rotation, false);
                isAttacking = false;
            } 
        }

        void OnHealthChange(int HP, int currHP){
            healthSlider.fillAmount = currHP / (float)HP;
        }

        void LateUpdate()
        {
            HPbar.position = target.position;
            HPbar.forward = -cam.forward;    
        }

        void OnDestroy() {
            Destroy(HPObj);
        }
    }
}
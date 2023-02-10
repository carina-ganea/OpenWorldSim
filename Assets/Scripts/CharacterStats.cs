using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterStats : MonoBehaviour
{
    public float critChance = 0.3f;
    public float critValue = .6f;
    public int Atk = 200;
    public int Def = 170;
    public int HP = 5000;
    public int Lvl = 70;
    public int skill = 3;

    public int currentHP { get; set; }
    
    public event System.Action<int, int> OnHealthChange; 
    Animator _animator;


    void Awake()
    {
        _animator = GetComponent<Animator>();
        currentHP = HP;
    }

    public void TakeDamage(int damage, bool crit)
    {
        _animator.SetTrigger("Damage");
        if( crit ){
            _animator.SetBool("Crit", true);
        } else {
            _animator.SetBool("Crit", false);
        }
        

        damage -= Def;
        damage = Mathf.Clamp(damage, 0, int.MaxValue);
        Debug.Log(damage);
        currentHP -= damage;
        
        if (OnHealthChange != null){
            OnHealthChange(HP, currentHP);
        }

        if (currentHP <= 0){
            Die();
        }
    }

    public void UpdateHealth()
    {
        OnHealthChange(HP, currentHP);
    }
    public virtual void Die()
    {
        _animator.SetBool("IsDead", true);
    }
}

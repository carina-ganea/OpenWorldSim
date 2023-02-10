using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStats : CharacterStats
{
    public float critChance = 0.3f;
    public float critValue = .6f;
    public int Atk = 200;
    public int Def = 170;
    public int HP = 5000;
    public int Lvl = 70;
    
    public int currentHP { get; private set; }

    public event System.Action<int, int> OnHealthChange; 

    public override void Die()
    {
        base.Die();

        Destroy(gameObject);
    }
}

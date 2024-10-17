using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour,IDamagable
{
    public void TakeDamage(float damage,Enums.DamageType damageType)
    {
        Debug.Log($"Eu {name} recebi {damage} de dano");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

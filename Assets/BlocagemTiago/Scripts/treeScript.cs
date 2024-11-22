using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treeScript : MonoBehaviour, IDamagable
{
    public void Die()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(float damage, Enums.DamageType damageType)
    {
        Die();
    }
}

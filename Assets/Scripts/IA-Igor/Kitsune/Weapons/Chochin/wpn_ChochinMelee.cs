using System.Collections;
using UnityEngine;

public class wpn_ChochinMelee : EnemyBaseWeapon
{
    [SerializeField] float disableSeconds;
    Coroutine thisCoroutine, colliderCoroutine;

    [SerializeField] float timeToEnableCollider, timeToDisableCollider;




    protected override void OneExecution()
    {
        thisCoroutine = StartCoroutine(Disable());
        colliderCoroutine = StartCoroutine(ColliderTimer());
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(disableSeconds);
        gameObject.SetActive(false);
    }

    IEnumerator ColliderTimer()
    {
        yield return new WaitForSeconds(timeToEnableCollider);
        weaponManager.EnableCollider();
        yield return new WaitForSeconds(timeToDisableCollider);
        weaponManager.DisableCollider();

    }
    

    
}
using System.Collections;
using UnityEngine;

public class wpn_Timer : EnemyBaseWeapon
{
    [SerializeField] float seconds;

    [SerializeField] bool disableCollider = false;
    [SerializeField] float secondsToDisable;



    void OnEnable()
    {
        StartCoroutine(Disable());
        if (disableCollider)
            StartCoroutine(DisableCollider());
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);

    }

    IEnumerator DisableCollider()
    {
        yield return new WaitForEndOfFrame();
        weaponManager.EnableCollider();
        yield return new WaitForSeconds(secondsToDisable);
        weaponManager.DisableCollider();
    }

}
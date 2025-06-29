using System.Collections;
using UnityEngine;

public class wpn_MagoLazer : EnemyBaseWeapon
{
    protected override void OneExecution()
    {
        transform.position = PlayerStateMachine.Instance.transform.position;
        transform.rotation = PlayerStateMachine.Instance.transform.rotation;
        StartCoroutine(Disable());
    }

    IEnumerator Disable()
    {
        yield return new WaitForEndOfFrame();
        weaponManager.DisableCollider();
        yield return new WaitForSeconds(1.5f);
        weaponManager.EnableCollider();
        yield return new WaitForSeconds(.5f);
        weaponManager.DisableCollider();
        gameObject.SetActive(false);
        
    }
}
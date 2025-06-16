using System.Collections;
using NUnit.Framework.Constraints;
using UnityEngine;


public class wpn_MagoMagicPunch : EnemyBaseWeapon
{
    [SerializeField] CharacterController charControl;
    [SerializeField] float speed;
    [SerializeField] float disableSeconds;
    [SerializeField] Vector3 startPos;
    Coroutine thisCoroutine;



    protected override void CustomStart()
    {
        startPos = transform.localPosition;
        charControl = GetComponent<CharacterController>();
    }

    protected override void OneExecution()
    {
        transform.localPosition = startPos;
        
        Physics.SyncTransforms();
        StartCoroutine(Disable());
    }

    void Update()
    {
        charControl.Move(transform.forward * speed * Time.deltaTime);
    }

    IEnumerator Disable()
    {
        yield return new WaitForFixedUpdate();
        weaponManager.EnableCollider();
        yield return new WaitForSeconds(disableSeconds);
        gameObject.SetActive(false);
    }

}
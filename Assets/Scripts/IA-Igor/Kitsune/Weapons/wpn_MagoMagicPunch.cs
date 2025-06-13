using System.Collections;
using UnityEngine;


public class wpn_MagoMagicPunch : EnemyBaseWeapon
{
    [SerializeField] CharacterController charControl;
    [SerializeField] float speed;
    [SerializeField] int miliseconds;
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
        weaponManager.EnableCollider();
        Physics.SyncTransforms();
        StartCoroutine(Disable());
    }

    void Update()
    {
        charControl.Move(transform.forward * speed * Time.deltaTime);
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }

}
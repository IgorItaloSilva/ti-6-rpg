using System.Collections;
using UnityEngine;


public class wpn_MagoFireTornado : EnemyBaseWeapon
{
    [SerializeField] CharacterController charControl;
    [SerializeField] float speed = 4.5f;
    [SerializeField] float steeringForce;
    float lookTime = 1;
    [SerializeField] int disableSeconds;
    float verticalVel;

    #region Apply Rotation 
    Vector3 directionToPlayer;
    Quaternion rotationDesired;
    #endregion


    Coroutine thisCoroutine;
    float delayTime;

    protected override void OneExecution()
    {
        transform.rotation = transform.parent.rotation;
        transform.position = transform.parent.position - transform.up + transform.forward * 2.5f;
        Physics.SyncTransforms();
        StartCoroutine(Disable());
    }


    void FixedUpdate()
    {
        transform.rotation = ApplyRotation();
        if (Vector3.Distance(transform.position, target.position) > 1)
            charControl.Move(transform.forward * speed * Time.fixedDeltaTime + transform.up * ApplyGravity());
    }

    protected Quaternion ApplyRotation()
    {
        // Rotation
        directionToPlayer = (PlayerStateMachine.Instance.transform.position - transform.position).normalized;
        directionToPlayer.y = 0;
        rotationDesired = Quaternion.LookRotation(directionToPlayer);
        if (lookTime < 1)
            lookTime += steeringForce * Time.fixedDeltaTime;
        return Quaternion.Slerp(charControl.transform.rotation, rotationDesired, lookTime);
    }

    protected float ApplyGravity()
    {
        if (charControl.isGrounded)
            verticalVel = -1f;
        else
            verticalVel -= 9.81f * Time.deltaTime;
        return verticalVel;
    }

    IEnumerator Disable()
    {
        delayTime = disableSeconds * 0.45f;
        for (int i = 0; i < 18; i++) {
            weaponManager.EnableCollider();
            yield return new WaitForSeconds(0.5f);
        }
        weaponManager.DisableCollider();
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }

}
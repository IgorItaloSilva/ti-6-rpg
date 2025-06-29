using System;
using UnityEngine;

public class StateMovingToSpawn : EnemyBaseState
{
    float newSteering;
    Vector3 directionToSpawn;
    Quaternion rotationDesired;


    protected override void OneExecution()
    {
        animator.CrossFade("Run", 0.15f);
        steeringForce = 0.3f;
    }

    public override void StateUpdate()
    {
        if (enemyBehave.GetDistSpawn() < enemyBehave.GetMeleeDist())
            enemyBehave.StartIdle();
    }

    public override void StateFixedUpdate()
    {
        charControl.transform.rotation = ApplyRotation();
        charControl.Move(charControl.transform.forward * speed * Time.fixedDeltaTime + Vector3.up * ApplyGravity());
    }

    protected override Quaternion ApplyRotation()
    {
        newSteering = steeringForce;
        // Rotation
        directionToSpawn = (enemyBehave.GetInitialPos() - charControl.transform.position).normalized;
        directionToSpawn.y = 0;
        rotationDesired = Quaternion.LookRotation(directionToSpawn);
        if(lookTime < 1)
                lookTime += newSteering * Time.fixedDeltaTime;
        return Quaternion.Slerp(charControl.transform.rotation, rotationDesired, lookTime);
    }
    
}
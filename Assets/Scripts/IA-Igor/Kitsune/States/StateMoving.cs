using UnityEngine;

public class StateMoving : EnemyBaseState
{
    float newSteering;
    Vector3 directionToSpawn;
    Quaternion rotationDesired;



    protected override void OneExecution()
    {
        animator.CrossFade("Run", 0.25f);
        steeringForce = 0.1f;
        minDistPlayer = 2f;
    }

    public override void StateUpdate()
    {
        if (GetPlayerDistance() < enemyBehave.GetMeleeDist())
        {
            enemyBehave.currentState = new StateIdle();
            StateExit();
        }

    }

    public override void StateFixedUpdate()
    {
        if (enemyBehave.IsDistFromSpawn() || !enemyBehave.GetTarget()){
            enemyBehave.currentState = new StateMovingToSpawn();
            StateExit();
        }
        charControl.transform.rotation = base.ApplyRotation();
        charControl.Move(charControl.transform.forward * speed * Time.fixedDeltaTime + Vector3.up * ApplyGravity());
    }

}
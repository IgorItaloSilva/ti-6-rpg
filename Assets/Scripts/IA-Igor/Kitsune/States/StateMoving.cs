using UnityEngine;

public class StateMoving : EnemyBaseState
{


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void OneExecution()
    {
        animator.CrossFade("Run", 0.25f);
        steeringForce = 0.1f;
        minDistPlayer = 2f;
    }

    public override void StateUpdate()
    {
        if (GetPlayerDistance() < enemyBehave.GetMeleeDist()) {
            enemyBehave.currentState = new StateIdle();
            StateExit();
        }

    }

    public override void StateFixedUpdate()
    {
        charControl.transform.rotation = ApplyRotation();
        charControl.Move(charControl.transform.forward * speed * Time.fixedDeltaTime + Vector3.up * ApplyGravity());
    }

}
using UnityEngine;

public class NewKitsuneIdle : EnemyBaseState
{
    protected override void OneExecution()
    {
        animator.CrossFade("Idle", 0.25f);
    }

    public override void StateUpdate()
    {
        if(GetPlayerDistance() > minDistPlayer) {
            enemyBehave.currentState = new NewKitsuneMoving();
            StateExit();
        }

    }

}
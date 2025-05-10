using UnityEngine;

public class KasaIdle : EnemyBaseState
{
    protected override void OneExecution()
    {
        animator.CrossFade("Idle", 0.25f);
    }

    public override void StateUpdate()
    {
        if(!enemyBehave.isResting() && enemyBehave.GetTarget()){
            if(GetPlayerDistance() > minDistPlayer) {
                enemyBehave.currentState = new KasaMoving();
                StateExit();
            }else{
                enemyBehave.currentState = enemyBehave.attackState;
                StateExit();

            }

        }
        
    }

}
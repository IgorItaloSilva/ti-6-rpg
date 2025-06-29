using UnityEngine;

public class StateIdle : EnemyBaseState
{
    protected override void OneExecution()
    {
        animator.CrossFade("Idle", 0.25f);
    }

    public override void StateUpdate()
    {
        if(!enemyBehave.isResting() && enemyBehave.GetTarget()){
            if (enemyBehave.IsDistFromSpawn()) {
                enemyBehave.currentState = new StateMovingToSpawn();
                StateExit();
            }else if(GetTargetAngle(charControl.transform, enemyBehave.GetTarget()) > 80) {
                enemyBehave.currentState = new StateTurn();
                StateExit();
            }else if(enemyBehave.IsRangeSkill()) {
                enemyBehave.currentState = enemyBehave.attackState;
                StateExit();
            }else if(GetPlayerDistance() > enemyBehave.GetMeleeDist()) {
                enemyBehave.currentState = new StateMoving();
                StateExit();
            }else {
                enemyBehave.currentState = enemyBehave.attackState;
                StateExit();
            }
            
        }
        
    }
    
}

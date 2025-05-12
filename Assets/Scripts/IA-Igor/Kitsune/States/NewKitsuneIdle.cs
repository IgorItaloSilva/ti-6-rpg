using UnityEngine;

public class NewKitsuneIdle : EnemyBaseState
{
    protected override void OneExecution()
    {
        animator.CrossFade("Idle", 0.25f);
    }

    public override void StateUpdate()
    {
        if(!enemyBehave.isResting() && enemyBehave.GetTarget()){
            if(GetTargetAngle(charControl.transform, enemyBehave.GetTarget()) > 80){
                enemyBehave.currentState = new NewKitsuneTurn();
                StateExit();
            }else if(enemyBehave.IsRangeSkill()) {
                enemyBehave.currentState = enemyBehave.attackState;
                StateExit();
            }else if(GetPlayerDistance() > minDistPlayer) {
                enemyBehave.currentState = new NewKitsuneMoving();
                StateExit();
            }else {
                enemyBehave.currentState = enemyBehave.attackState;
                StateExit();
            }
        }
        
    }

}
using UnityEngine;

public class StateTurn : EnemyBaseState
{
    protected override void OneExecution()
    {
        steeringForce = 0.5f;
        Vector3 directionToPlayer = (enemyBehave.GetTarget().position - charControl.transform.position).normalized;
        directionToPlayer.y = 0;
        if(Vector3.Cross(charControl.transform.forward, directionToPlayer).y > 0){
            animator.Play("TurnMirror", -1, 0.25f);
        }else
            animator.Play("Turn", -1, 0.25f);
        
    }

    public override void StateUpdate()
    {
        if(lookTime >= 0.3f){
            enemyBehave.currentState = new StateIdle();
            StateExit();
        }
    }

    public override void StateFixedUpdate() { charControl.transform.rotation = ApplyRotation(); }
    
}
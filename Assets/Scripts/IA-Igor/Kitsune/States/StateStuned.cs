using UnityEngine;


public class StateStuned : EnemyBaseState
{
    
    protected override void OneExecution() {
        animator.Play("Stun", -1, 0.0f);
    }

    public override void StateUpdate() {
        lookTime += Time.deltaTime;
        if(lookTime >= 4.15f){
            enemyBehave.IdleState();
            enemyBehave.ResetPoise();
            StateExit();
        }

    }

}
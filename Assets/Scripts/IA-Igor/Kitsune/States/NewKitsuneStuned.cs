using UnityEngine;


public class NewKitsuneStuned : EnemyBaseState
{
    
    protected override void OneExecution() {
        animator.Play("Stun", -1, 0.0f);
    }

    public override void StateUpdate() {
        lookTime += Time.deltaTime;
        if(lookTime >= 4.15f){
            enemyBehave.currentState = new NewKitsuneIdle();
            enemyBehave.ResetPoise();
            StateExit();
        }

    }



}
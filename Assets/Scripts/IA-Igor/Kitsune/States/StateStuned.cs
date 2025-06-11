using UnityEngine;


public class StateStuned : EnemyBaseState
{
    
    protected override void OneExecution() {
        animator.Play("Stun", -1, 0.0f);
        
        if (enemyBehave.GetTarget())
            _knockbackDir = (enemyBehave.transform.position - enemyBehave.GetTarget().position).normalized;
        else
            _knockbackDir = -enemyBehave.transform.forward;
    }

    public override void StateUpdate() {
        lookTime += Time.deltaTime;
        if(lookTime >= 4.15f){
            enemyBehave.StartIdle();
            enemyBehave.ResetPoise();
            StateExit();
        }

    }

    public override void StateFixedUpdate()
    {
        Knockback();
    }

}
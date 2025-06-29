using UnityEngine;


public class StateStuned : EnemyBaseState
{
    
    protected override void OneExecution() {
        enemyBehave.enemySounds?.PlaySound(EnemySounds.SoundType.Death, enemyBehave.soundSource);
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
        charControl.Move(Vector3.up * ApplyGravity());

    }

    public override void StateFixedUpdate()
    {
        Knockback();
    }

}
using System;
using UnityEngine;

public class StateDamage : EnemyBaseState
{
    protected override void OneExecution()
    {
        animator.CrossFade("Damage", 0.25f);
        restTime = 2f;
    }

    public override void StateUpdate()
    {
        if (!enemyBehave.isResting()) {
            enemyBehave.SetRest(restTime);
            enemyBehave.StartIdle();
            
        }

    }
    
}
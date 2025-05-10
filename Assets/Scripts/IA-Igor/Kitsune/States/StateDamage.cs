using System;
using UnityEngine;

public class StateDamage : EnemyBaseState
{
    protected override void OneExecution()
    {
        animator.CrossFade("Damage", 0.25f);
    }

    public override void StateUpdate()
    {
        if(!enemyBehave.isResting()){
            enemyBehave.IdleState();
        }

    }
    
}
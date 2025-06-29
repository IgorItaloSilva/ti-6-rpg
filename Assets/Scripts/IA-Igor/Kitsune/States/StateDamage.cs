using System;
using UnityEngine;

public class StateDamage : EnemyBaseState
{


    protected override void OneExecution()
    {
        enemyBehave.enemySounds?.PlaySound(EnemySounds.SoundType.Damage, enemyBehave.soundSource);
        animator.CrossFade("Damage", 0.25f);
        restTime = 2f;
        _appliedMovement.y = -9.8f;
        if (enemyBehave.GetTarget())
            _knockbackDir = (enemyBehave.transform.position - enemyBehave.GetTarget().position).normalized;
        else
            _knockbackDir = -enemyBehave.transform.forward;
    }

    public override void StateUpdate()
    {
        if (!enemyBehave.isResting())
        {
            enemyBehave.SetRest(restTime);
            enemyBehave.StartIdle();
        }
        charControl.Move(Vector3.up * ApplyGravity());

    }

    public override void StateFixedUpdate()
    {
        Knockback();
    }
}
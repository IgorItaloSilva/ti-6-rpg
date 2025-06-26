using UnityEngine;

public class NewKitsuneAttackAOE : EnemyBaseState
{
    protected override void OneExecution()
    {
        restTime = 1.5f;
        animator.CrossFade("AttackAoe", 0.15f);
        lookTime = 0;
        enemyBehave.UseWeapon();

    }

    public override void StateUpdate()
    {
        lookTime += Time.deltaTime;
        if (lookTime >= 4.4f)
        {
            enemyBehave.StartIdle();
            enemyBehave.ChoseSkill();
            StateExit();

        }

    }

}
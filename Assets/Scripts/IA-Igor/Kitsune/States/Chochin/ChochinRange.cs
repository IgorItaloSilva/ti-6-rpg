using UnityEngine;

public class ChochinRange : EnemyBaseState
{
    float timer;
    bool useWeapon;



    protected override void OneExecution()
    {
        animator.CrossFade("AttackRange", 0.25f);
        restTime = 1f;
        timer = 0;
        lookTime = .25f;
        useWeapon = true;
    }

    public override void StateFixedUpdate()
    {
        if (useWeapon && timer >= 1.25f)
        {
            enemyBehave.enemySounds.PlaySound(EnemySounds.SoundType.Attack, enemyBehave.soundSource, 2);
            useWeapon = false;
            enemyBehave.UseWeapon();
        }

        if (timer <= 1.3f)
            charControl.transform.rotation = ApplyRotation();

        if (timer >= 2.125f)
        {
            enemyBehave.SetRest(restTime);
            enemyBehave.StartIdle();
            enemyBehave.ChoseSkill();
        }

        timer += Time.fixedDeltaTime;

    }
    
}
using UnityEngine;

public class YukiAttack3 : EnemyBaseState
{
    bool useWeapon;
    float timer;



    protected override void OneExecution()
    {
        restTime = 1f;
        timer = 0;
        useWeapon = true;
        lookTime = .5f;
        animator.CrossFade("Attack03", 0.25f);
    }

    public override void StateFixedUpdate()
    {
        if (useWeapon && timer >= 0.5f)
        {
            useWeapon = false;
            enemyBehave.UseWeapon();
        }

        if (timer <= .6f)
            charControl.transform.rotation = ApplyRotation();

        if (timer >= 1.25f)
        {
            enemyBehave.SetRest(restTime);
            enemyBehave.StartIdle();
            enemyBehave.ChoseSkill();
        }

        timer += Time.fixedDeltaTime;
            
    }

}
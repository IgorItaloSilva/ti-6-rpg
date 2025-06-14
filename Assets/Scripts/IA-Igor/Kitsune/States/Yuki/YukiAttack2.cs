using System.Threading;
using UnityEngine;

public class YukiAttack2 : EnemyBaseState
{
    bool useWeapon;
    float timer;



    protected override void OneExecution()
    {
        timer = 0;
        useWeapon = true;
        restTime = 2;
        animator.CrossFade("Attack02", 0.2f);
        lookTime = 0.25f;
    }

    // Update is called once per frame
    public override void StateUpdate()
    {
        if (useWeapon && timer > 0.8f)
        {
            useWeapon = false;
            enemyBehave.UseWeapon();
        }
        else if (timer < 0.8f)
            charControl.transform.rotation = ApplyRotation();

        if (timer >= 2.5f)
        {
            enemyBehave.SetRest(restTime);
            enemyBehave.StartIdle();
            enemyBehave.ChoseSkill();
        }
        timer += Time.deltaTime;
    }
    
}
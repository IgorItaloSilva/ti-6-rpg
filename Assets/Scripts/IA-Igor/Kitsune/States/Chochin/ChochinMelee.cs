using System;
using UnityEngine;

public class ChochinMelee : EnemyBaseState
{
    float timer;
    bool useWeapon;



    protected override void OneExecution()
    {
        animator.CrossFade("AttackMelee", 0.25f);
        restTime = 2;
        timer = 0;
        lookTime = 0.25f;
        useWeapon = true;
    }

    public override void StateFixedUpdate()
    {
        if (useWeapon && timer >= 1.12f) {
            useWeapon = false;
            enemyBehave.UseWeapon();
        }

        if (timer <= 1.2f)
            charControl.transform.rotation = ApplyRotation();
        
        if(timer >= 1.9f){
            enemyBehave.SetRest(restTime);
            enemyBehave.StartIdle();
            enemyBehave.ChoseSkill();
        }
        
        timer += Time.fixedDeltaTime;

    }

}
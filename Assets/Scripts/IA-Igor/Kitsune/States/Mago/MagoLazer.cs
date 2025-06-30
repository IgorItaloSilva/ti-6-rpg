using System.Collections;
using UnityEngine;

public class MagoLazer : EnemyBaseState
{
    float timer;
    float activateTimes = 5;
    bool canRecover;



    protected override void OneExecution()
    {
        restTime = 1;
        animator.CrossFade("ComboFinal", 0.15f);
        activateTimes = 5;
        timer = 0;
        canRecover = true;
    }

    public override void StateUpdate()
    {
        if (activateTimes > 0 && timer >= 2.1f) {
            enemyBehave.UseWeapon();
            activateTimes--;
            timer = 0;
            return;
        }

        if (canRecover && timer >= 2.1f)
        {
            animator.CrossFade("FlyRecover", 0.15f);
            canRecover = false;
        }
        else if (timer >= 3.88f)
        {
            enemyBehave.SetRest(restTime);
            enemyBehave.StartIdle();
            enemyBehave.ChoseSkill();
        }

        timer += Time.deltaTime;
        
        
    }

}
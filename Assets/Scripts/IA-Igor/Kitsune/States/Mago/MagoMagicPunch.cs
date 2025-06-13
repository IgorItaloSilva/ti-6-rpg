using UnityEngine;


public class MagoMagicPunch : EnemyBaseState
{
    float timer;
    bool canAttack;



    protected override void OneExecution()
    {
        restTime = 0.5f;
        animator.CrossFade("MagicPunch", 0.15f);
        timer = 0;
        canAttack = true;
        lookTime = 0.3f;
    }

    // Update is called once per frame
    public override void StateUpdate()
    {
        
        if (timer > 1.575f) {
            if (canAttack) {
                enemyBehave.UseWeapon();
                canAttack = false;
            }
        }
        else {
            charControl.transform.rotation = ApplyRotation();
        }

        if (timer >= 3) {
            enemyBehave.SetRest(restTime);
            enemyBehave.StartIdle();
            enemyBehave.ChoseSkill();
        }
        timer += Time.deltaTime;

    }

}
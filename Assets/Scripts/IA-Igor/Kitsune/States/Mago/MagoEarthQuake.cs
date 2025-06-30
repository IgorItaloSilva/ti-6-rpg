using UnityEngine;

public class MagoEarthQuake : EnemyBaseState
{
    float timer;
    bool canAttack;



    protected override void OneExecution()
    {
        restTime = 1;
        animator.CrossFade("EarthQuake", 0.15f);
        lookTime = 0.45f;
        canAttack = true;
        timer = 0;
    }

    public override void StateFixedUpdate()
    {
        if (canAttack)
        {
            if (timer >= 1.3f)
            {
                enemyBehave.UseWeapon();
                canAttack = false;
            }
            else
                charControl.transform.rotation = ApplyRotation();
        }
        else
        {
            if (timer >= 2.92f)
            {
                enemyBehave.SetRest(restTime);
                enemyBehave.StartIdle();
                enemyBehave.ChoseSkill();
            }

        }

        timer += Time.fixedDeltaTime;
        charControl.Move(Vector3.up * ApplyGravity());
    }
    
}
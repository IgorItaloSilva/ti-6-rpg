using UnityEngine;


public class MagoFireTornado : EnemyBaseState
{
    float timer;
    bool canAttack;

    protected override void OneExecution()
    {
        restTime = 12;
        animator.CrossFade("AttackRange", 0.15f);
        lookTime = 0;
        canAttack = true;
        timer = 0;
    }

    public override void StateUpdate()
    {
        charControl.transform.rotation = ApplyRotation();
        if (canAttack && timer >= 0.85f) {
            enemyBehave.UseWeapon();
            canAttack = false;
        }
        else if (timer >= 1.23f) {
            enemyBehave.SetRest(restTime);
            enemyBehave.StartIdle();
            enemyBehave.ChoseSkill();
        }
        timer += Time.deltaTime;
        charControl.Move(Vector3.up * ApplyGravity());
        
    }
    
}
using UnityEngine;

public class YukiAttack1 : EnemyBaseState
{
    bool canDamage;
    float timer;



    protected override void OneExecution()
    {
        timer = 0;
        canDamage = true;
        restTime = 2;
        lookTime = 0.5f;
        animator.CrossFade("Attack1", 0.25f);
    }

    public override void StateUpdate()
    {
        if (timer > 0.9f) {
            if (canDamage) {
                canDamage = false;
                enemyBehave.EnableWeapon();

            }
            if (timer < 1.08)
            {
                charControl.Move(charControl.transform.forward * speed * Time.fixedDeltaTime + Vector3.up * ApplyGravity());

            }
            else
                enemyBehave.DisableWeapon();
        }else
            charControl.transform.rotation = ApplyRotation();
            
        

        if (timer > 1.6f) {
            enemyBehave.SetRest(restTime); // Aplicar descanço da skill
            enemyBehave.StartIdle();
            enemyBehave.ChoseSkill(); // Escolher nova skill
            StateExit(); // Sair do estado atual
        }
        timer += Time.deltaTime;

    }
    
}
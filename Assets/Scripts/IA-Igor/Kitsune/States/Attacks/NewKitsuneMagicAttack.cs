using System.Threading;
using UnityEngine;

public class NewKitsuneMagicAttack : EnemyBaseState
{
    bool useWeapon = false;
    protected override void OneExecution()
    {
        lookTime = 0;
        speed = 2;
        restTime = 2f;
        animator.Play("RangeAttack", -1, 0.0f);
    }

    public override void StateUpdate()
    {
        lookTime += Time.deltaTime;
    }

    public override void StateFixedUpdate()
    {
        if(lookTime < 1.5f )
            charControl.Move(charControl.transform.up * speed * Time.fixedDeltaTime);
        else if(lookTime > 3.5f){
            charControl.Move(charControl.transform.up * -speed * Time.fixedDeltaTime);
            if(charControl.isGrounded){
                enemyBehave.SetRest(restTime); // Aplicar descanço da skill
                enemyBehave.currentState = new NewKitsuneIdle(); // Colocar na posição de idle
                enemyBehave.DisableWeapon(); // Desabilitar arma
                enemyBehave.ChoseSkill(); // Escolher nova skill
                StateExit(); // Sair do estado atual
            }

        }else
            if(!useWeapon){
                enemyBehave.UseWeapon();
                useWeapon = true;
            }

    }
    
}
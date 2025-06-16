using UnityEngine;


public class NewKitsuneMagicAttack : EnemyBaseState
{
    bool useWeapon;
    protected override void OneExecution()
    {
        useWeapon = false;
        lookTime = 0;
        speed = 2;
        restTime = 2f;
        animator.Play("RangeAttack", -1, 0.0f);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Damageble"), true);
    }

    public override void StateUpdate()
    {
        lookTime += Time.deltaTime;
    }

    public override void StateFixedUpdate()
    {
        if(lookTime < 1.5f )
            charControl.Move(charControl.transform.up * speed * Time.fixedDeltaTime);
        else if(lookTime > 2.5f){
            charControl.Move(charControl.transform.up * (-speed * 2)  * Time.fixedDeltaTime);
            if(charControl.isGrounded){
                enemyBehave.SetRest(restTime); // Aplicar descanço da skill
                enemyBehave.StartIdle(); // Colocar na posição de idle
                enemyBehave.DisableWeapon(); // Desabilitar arma
                enemyBehave.ChoseSkill(); // Escolher nova skill
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Damageble"), false);
                StateExit(); // Sair do estado atual
            }

        }else
            if(!useWeapon){
                enemyBehave.UseWeapon();
                useWeapon = true;
            }

    }
    
}
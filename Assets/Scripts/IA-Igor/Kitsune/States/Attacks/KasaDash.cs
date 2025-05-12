using Codice.CM.Common.Replication;
using UnityEngine;

public class KasaDash : EnemyBaseState
{
    bool canDamage;
    protected override void OneExecution()
    {
        animator.CrossFade("Dash", 0.15f);
        restTime = 2f;
        canDamage = true;
    }

    public override void StateUpdate()
    {
        lookTime += Time.deltaTime;

        if(lookTime > 2.15f){ // acabou o ataque
            enemyBehave.SetRest(restTime); // Aplicar descanço da skill
            enemyBehave.IdleState(); // Colocar na posição de idle
            enemyBehave.DisableWeapon(); // Desabilitar arma
            enemyBehave.ChoseSkill(); // Escolher nova skill
            StateExit(); // Sair do estado atual
        }

    }

    public override void StateFixedUpdate()
    {
        if(lookTime < 1.5f){
            // Rotation
            Vector3 directionToPlayer = (enemyBehave.GetTarget().position - charControl.transform.position).normalized;
            directionToPlayer.y = 0;
            Quaternion rotationDesired = Quaternion.LookRotation(directionToPlayer);
            charControl.transform.rotation = Quaternion.Slerp(charControl.transform.rotation, rotationDesired, lookTime);
            lookTime += steeringForce * Time.fixedDeltaTime * 2;
            
        }else if (lookTime < 2.25f){
            if(canDamage){
                enemyBehave.EnableWeapon();
                canDamage = false;
            }
            charControl.Move(charControl.transform.forward * speed * 2 + charControl.transform.up * ApplyGravity());
        }

    }

}
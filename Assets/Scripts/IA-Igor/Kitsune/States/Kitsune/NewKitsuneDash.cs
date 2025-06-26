using System.Dynamic;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

public class NewKitsuneDash : EnemyBaseState
{
    bool canDamage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void OneExecution()
    {
        restTime = 1f;
        animator.Play("Dash", -1, 0f);
        canDamage = true;
        PlayVFXAsync();
    }

    public override void StateUpdate()
    {
        lookTime += Time.deltaTime;

        if(lookTime > 1f){ // acabou o ataque
            enemyBehave.SetRest(restTime); // Aplicar descanço da skill
            enemyBehave.StartIdle();
            enemyBehave.DisableWeapon(); // Desabilitar arma
            enemyBehave.ChoseSkill(); // Escolher nova skill
            StateExit(); // Sair do estado atual
        }
    }

    public override void StateFixedUpdate()
    {
        if(lookTime < 0.5f && enemyBehave.GetTarget()){
            // Rotation
            Vector3 directionToPlayer = (enemyBehave.GetTarget().position - charControl.transform.position).normalized;
            directionToPlayer.y = 0;
            Quaternion rotationDesired = Quaternion.LookRotation(directionToPlayer);
            charControl.transform.rotation = Quaternion.Slerp(charControl.transform.rotation, rotationDesired, lookTime);
            if(lookTime < 1)
                lookTime += steeringForce * Time.fixedDeltaTime * 2;
        
        }else if(lookTime < 0.8f){
            if(canDamage) {
                enemyBehave.EnableWeapon();
                canDamage = false;
            }
            charControl.Move(charControl.transform.forward * speed * Time.fixedDeltaTime * 7 + charControl.transform.up * ApplyGravity());
        }
        
    }

    private async void PlayVFXAsync()
    {
        await Task.Delay(500);
        if (enemyBehave.currentState is NewKitsuneDash && GetPlayerDistance() > enemyBehave.GetMeleeDist())
            enemyBehave._dashVFX.Play();
        enemyBehave._headbuttVFX.Play();
    }
    


}
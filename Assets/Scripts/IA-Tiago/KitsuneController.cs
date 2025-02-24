using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KitsuneController : ActualEnemyController
{
    EnemyActions basicAttack;
    EnemyActions deathAction;
    [Header("Coisas especificas da Kitsune")]
    [SerializeField]float basicAttackDist;
    [SerializeField]float attackTime;
    [SerializeField]float restTime;
    [SerializeField]int pillarID;
    [SerializeField]Transform wanderCenter;
    [SerializeField]float maxWanderDist;
    //Variaveis de controle das actions
    bool isAttacking,isResting,isDead;
    //Variaveis de controle de ifs
    bool halvedVelocity,doubledVelocity,halvedAvoidWeight,doubledAvoidWeight=true;
    protected override void CreateActions()
    {
        basicAttack = new KitsuneBasicAttack(attackTime,basicAttackDist,this);
        restAction = new KitsuneRestAction(restTime,this);
        deathAction = new KitsuneDeathAction(3f,this);
    }
    protected override void AdditionalStart()
    {
        isAttacking=false;
        isResting=false;
        isDead = false;
    }
    protected override void SetSteeringTargetAndCurrentAction(){
        if(isDead)return;
        if(target==null){//colocado aqui devido a bug quando o jogador morre
            if(wanderCenter!=null){
                //Debug.Log((wanderCenter.position-transform.position).sqrMagnitude);
                if((wanderCenter.position-transform.position).sqrMagnitude<maxWanderDist*maxWanderDist){
                    if(!halvedVelocity){
                        maxVelocity/=2;
                        halvedVelocity=true;
                        doubledVelocity=false;
                    }
                    if(!doubledAvoidWeight){
                        avoidObstacleWeight*=2;
                        halvedAvoidWeight=false;
                        doubledAvoidWeight=true;
                    }
                    steeringManager?.Wander();
                    
                }
                else{
                    steeringManager.Seek(wanderCenter.position);
                }
                steeringManager?.AvoidObstacle();
            }
        }
        else{
            if(isAttacking){
                steeringManager.LookAtTargetToAttack(target.GetPosition());
                return;
            }
            if(isResting)return;
            if(actionsPerformed>=numberOfActionsBeforeRest){
                Debug.Log("Trocando ação para rest");
                ChangeAction(restAction);
                isResting=true;
                actionsPerformed=0;
            }
            else{
                if(Vector3.SqrMagnitude(target.GetPosition()-transform.position)>minDistToAttack*minDistToAttack){
                    steeringManager?.Seek(target.GetPosition());
                    steeringManager?.AvoidObstacle();
                    if(!doubledVelocity){
                        maxVelocity*=2;
                        halvedVelocity=false;
                        doubledVelocity=true;
                    }
                    if(!halvedAvoidWeight){
                        avoidObstacleWeight/=2;
                        halvedAvoidWeight=true;
                        doubledAvoidWeight=false;
                    }
                }
                else{
                    if(!isAttacking){
                        Debug.Log("troquei a ação pra ataque");
                        ChangeAction(basicAttack);
                        actionsPerformed++;
                        isAttacking=true;
                    }
                }
            }
        }
    }
    public override void TakeDamage(float damage, Enums.DamageType damageType, bool wasCrit)
    {
        if(isDead)return;
        base.TakeDamage(damage, damageType, wasCrit);
    }
    new void FixedUpdate(){
        SetSteeringTargetAndCurrentAction();
        //steeringManager?.AvoidObstacle();
        steeringManager?.Update();
        currentAction?.UpdateAction(); 
    }
    public override void Die()
    {
        isDead=true;
        ChangeAction(deathAction);
    }
    public void ActualDeath(){
        base.Die();
        GameEventsManager.instance.levelEvents.KitsuneDeath(pillarID);
    }
    public override void Respawn()
    {
        base.Respawn();
        isDead=false;
        animator.SetBool("isDeadBool",false);
        animator.ResetTrigger("isDead");
    }
    protected override void ResetControlBooleans()//chamado no changeAction
    {
        base.ResetControlBooleans();
        isAttacking=false;
        isResting=false;
    }
}

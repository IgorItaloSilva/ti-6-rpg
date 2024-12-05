using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitsuneBoss : ActualEnemyController
{
    EnemyActions basicAttack;
    EnemyActions rangedAttack;
    [SerializeField]float basicAttackDist;
    [SerializeField]float minDistToRangedAttack;
    [SerializeField]float attackTime;
    [SerializeField]float rangedattackTime;
    [SerializeField]float restTime;
    [SerializeField]GameObject prefabRangedAttack;
    bool isAttacking;
    bool isResting;
    protected override void CreateActions()
    {
        basicAttack = new KitsuneBasicAttack(attackTime,basicAttackDist,this);
        restAction = new KitsuneRestAction(restTime,this);
        rangedAttack = new KitSuneRangedAttack(rangedattackTime,prefabRangedAttack,this);
    }
    protected override void AdditionalStart()
    {
        isAttacking=false;
        isResting=false;
    }
    protected override void SetSteeringTargetAndCurrentAction(){
        if(isAttacking)return;
        if(isResting)return;
        if(target==null){
            ;
        }
        else{
            steeringManager?.AvoidObstacle();
            if(actionsPerformed>=numberOfActionsBeforeRest){
                Debug.Log("Trocando ação para rest");
                ChangeAction(restAction);
                isResting=true;
                actionsPerformed=0;
            }
            else{
                float sqrDistToPlayer = Vector3.SqrMagnitude(target.GetPosition()-transform.position);
                if(sqrDistToPlayer>minDistToRangedAttack*minDistToAttack){
                    Debug.Log("troquei a ação pra ataque ranged");
                    ChangeAction(rangedAttack);
                    actionsPerformed+=3;
                    isAttacking=true;
                }else{
                    if(sqrDistToPlayer>basicAttackDist*basicAttackDist){
                        steeringManager.Seek(target.GetPosition());
                        //Debug.Log("Seek");
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
    }
    new void FixedUpdate(){
        SetSteeringTargetAndCurrentAction();
        //steeringManager?.AvoidObstacle();
        steeringManager?.Update();
        currentAction?.UpdateAction(); 
    }
    protected override void ResetControlBooleans()//chamado no changeAction
    {
        base.ResetControlBooleans();
        isAttacking=false;
        isResting=false;
    }
}


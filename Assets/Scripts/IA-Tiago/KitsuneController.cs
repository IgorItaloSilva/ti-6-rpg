using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KitsuneController : ActualEnemyController
{
    EnemyActions basicAttack;
    [SerializeField]float basicAttackDist;
    [SerializeField]float attackTime;
    [SerializeField]float restTime;
    bool isAttacking;
    bool isResting;
    protected override void CreateActions()
    {
        basicAttack = new KitsuneBasicAttack(attackTime,basicAttackDist,this);
        restAction = new KitsuneRestAction(restTime,this);
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
            //Debug.Log("wander");
            //adicionar depois logica de patrulha
            steeringManager.Wander();
        }
        else{
            if(actionsPerformed>=numberOfActionsBeforeRest){
                Debug.Log("Trocando ação para rest");
                ChangeAction(restAction);
                isResting=true;
                actionsPerformed=0;
            }
            else{
                if(Vector3.SqrMagnitude(target.GetPosition()-transform.position)>minDistToAttack*minDistToAttack){
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
    protected override void ResetControlBooleans()//chamado no changeAction
    {
        base.ResetControlBooleans();
        isAttacking=false;
        isResting=false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarController : ActualEnemyController
{
    EnemyActions basicAttack;
    EnemyActions stayStill;
    [SerializeField]float basicAttackDist;
    [SerializeField]float attackTime;
    [SerializeField]float restTime;
    bool isAttacking;
    bool isResting;
    bool isStill;
    protected override void CreateActions()
    {
        basicAttack = new KitsuneBasicAttack(attackTime,basicAttackDist,this);
        restAction = new KitsuneRestAction(restTime,this);
        stayStill = new StayStillAction(this);
    }
    protected override void AdditionalStart()
    {
        isStill=false;
        isAttacking=false;
        isResting=false;
    }
    protected override void SetSteeringTargetAndCurrentAction(){
        if(isAttacking)return;
        if(isResting)return;
        if(target==null){
            //Debug.Log("wander");
            //adicionar depois logica de patrulha
            if(!isStill){
                ChangeAction(stayStill);
                isStill=true;
            }
        }
        else{
            if(isStill){
                ChangeAction(new nullAction());
            }
            if(actionsPerformed>=numberOfActionsBeforeRest){
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
        isStill=false;
        isAttacking=false;
        isResting=false;
    }
}

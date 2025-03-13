using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitsuneBoss : KitsuneController
{
    EnemyActions rangedAttack;
    [Header("Coisas especificas da kitsuneBoss")]
    [SerializeField]float minDistToRangedAttack;
    [SerializeField]float rangedattackTime;
    [SerializeField]GameObject prefabRangedAttack;
    [SerializeField]public Transform[] rangedAttackPos;
    [SerializeField]Enums.PowerUpType rewardPowerUpType;
    string nome = "Kitsune, a guardiã";
    bool hasDisplayedLife;
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
            if(hasDisplayedLife==true){
                UIManager.instance?.HideBossLife();
                hasDisplayedLife=false;
            }
        }
        else{
            if(hasDisplayedLife==false){
                UIManager.instance?.BossLifeSettup(CurrentHp,maxHp,nome);
                hasDisplayedLife=true;
            }
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
    override public void TakeDamage(float damage, Enums.DamageType damageType,bool wasCrit){
        base.TakeDamage(damage,damageType,wasCrit);
        //Caso queiramos fazer ela ser imune a um tipo de dano, ou ter um cap de dano pra n ser obliterada 
    }
    protected override void BossDeath()
    {
        Debug.Log("Chamado a boss death da classe filha");
        base.BossDeath();
        SkillTree.instance?.GainMoney((int)rewardPowerUpType);
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


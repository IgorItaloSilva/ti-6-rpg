using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitsuneBoss : KitsuneController
{
    [Header("Coisas especificas da kitsuneBoss")]
    [SerializeField]Enums.PowerUpType rewardPowerUpType;
    string nome = "Kitsune, a guardiã";
    bool hasDisplayedLife;
    bool lastActionWasMagic;
    protected override void SetSteeringTargetAndCurrentAction(){
        if(isDead)return;
        if(isDashing)return;
        if(isResting)return;
        if(isCasting)return;
        if(target==null){
            if(hasDisplayedLife){
                UIManager.instance?.HideBossLife();
                hasDisplayedLife=false;
            }
        }
        else{
            if(!hasDisplayedLife){
                UIManager.instance?.BossLifeSettup(CurrentHp,maxHp,nome);
                hasDisplayedLife=true;
            }
            if(isAttacking){
                steeringManager.LookAtTargetToAttack(target.GetPosition());
                return;
            }
            if(actionsPerformed>=numberOfActionsBeforeRest){
                Debug.Log("Trocando ação para rest");
                ChangeAction(restAction);
                isResting=true;
                actionsPerformed=0;
            }
            else{
                float distToPlayerSqr = Vector3.SqrMagnitude(target.GetPosition()-transform.position);
                if(distToPlayerSqr>minDistToAttack*minDistToAttack){
                    steeringManager?.Seek(target.GetPosition());
                    steeringManager?.AvoidObstacle();
                    if((distToPlayerSqr<dashAttackDist*dashAttackDist) && dashCharges>0){
                        ChangeAction(dashAttack);
                        actionsPerformed++;
                        dashCharges--;
                        lastActionWasMagic=false;
                    }
                    else if(magicCharges>0&&!lastActionWasMagic){
                        ChangeAction(magicAttack);
                        actionsPerformed++;
                        magicCharges--;
                        lastActionWasMagic=true;
                    }
                }
                else{
                    if(!isAttacking){
                        Debug.Log("troquei a ação pra ataque");
                        ChangeAction(basicAttack);
                        actionsPerformed++;
                        lastActionWasMagic=false;
                    }
                }
            }
        }
    }
    override public void TakeDamage(float damage, Enums.DamageType damageType,bool wasCrit){
        //Caso queiramos fazer ela ser imune a um tipo de dano, ou ter um cap de dano pra n ser obliterada 
        if(damage>100)damage=100;
        base.TakeDamage(damage,damageType,wasCrit);
    }
    protected override void BossDeath()
    {
        Debug.Log("Chamado a boss death da classe filha");
        base.BossDeath();
    }
    
}


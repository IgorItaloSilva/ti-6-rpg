using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KitsuneController : ActualEnemyController
{
    protected EnemyActions basicAttack;
    protected EnemyActions deathAction;
    protected EnemyActions dashAttack;
    protected EnemyActions magicAttack;
    [Header("Coisas especificas da Kitsune")]
    [SerializeField]protected float basicAttackDist;
    [SerializeField]protected float attackTime;
    [SerializeField]protected float dashAttackDist;
    [SerializeField]protected float dashAttackTime;
    [SerializeField]protected float rangedAttackTime;
    [SerializeField]protected float rangedAttackDamage;
    [SerializeField]protected int nDashCharges;
    [SerializeField]protected int nMagicCharges;
    [SerializeField]protected float restTime;
    [SerializeField]int pillarID;
    [SerializeField]Transform wanderCenter;
    [SerializeField]float maxWanderDist;
    //[SerializeField] private Canvas healthBar;
    [SerializeField]float maxWanderTime;
    [SerializeField]protected GameObject prefabRangedAttack;
    public Transform[] rangedAttackPos;
    float timeWandering;
    protected int dashCharges;
    protected int magicCharges;
    //Variaveis de controle das actions
    [HideInInspector]public bool isAttacking,isResting,isDead,isDashing,isCasting;
    //Variaveis de controle de ifs
    bool halvedVelocity,doubledVelocity,halvedAvoidWeight,doubledAvoidWeight=true;
    protected override void CreateActions()
    {
        basicAttack = new KitsuneBasicAttack(attackTime,this);
        restAction = new KitsuneRestAction(restTime,this);
        deathAction = new KitsuneDeathAction(3f,this);
        dashAttack = new KitsuneDashAttack(dashAttackTime,.3f,this);
        magicAttack = new KitSuneRangedAttack(rangedAttackTime,rangedAttackDamage,prefabRangedAttack,this);
    }
    protected override void AdditionalStart()
    {
        ResetSpecialAttacksCharges();
        isAttacking=false;
        isDead = false;
        isCasting=false;
        ChangeAction(restAction);
    }
    protected override void SetSteeringTargetAndCurrentAction(){
        if(isDead)return;
        if(isDashing)return;
        if(isResting)return;
        if(isCasting)return;
        if(target==null){//colocado aqui devido a bug quando o jogador morre
            if(wanderCenter!=null){
                //Debug.Log((wanderCenter.position-transform.position).sqrMagnitude);
                if((wanderCenter.position-transform.position).sqrMagnitude<maxWanderDist*maxWanderDist){
                    if(!halvedVelocity){
                        maxVelocity/=2;
                        animator.SetBool("isRunning",false);
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
                timeWandering+=Time.deltaTime;
                if(timeWandering>=maxWanderTime){
                    ChangeAction(restAction);
                    isResting=true;
                    timeWandering=0;
                }
            }
        }
        else{
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
                    if(magicCharges>0){
                        ChangeAction(magicAttack);
                        actionsPerformed++;
                        magicCharges--;
                    }
                    if((distToPlayerSqr<dashAttackDist*dashAttackDist) && dashCharges>0){
                        ChangeAction(dashAttack);
                        actionsPerformed++;
                        dashCharges--;
                    }
                    if(!doubledVelocity){
                        maxVelocity*=2;
                        animator.SetBool("isRunning",true);
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
                    }
                }
            }
        }
    }
    public override void TakeDamage(float damage, Enums.DamageType damageType, bool wasCrit)
    {
        if(isDead)return;
        base.TakeDamage(damage, damageType, wasCrit);
        AudioPlayer.instance.PlaySFX("SwordHit");
        AudioPlayer.instance.PlaySFX("Stab");
    }
    new void FixedUpdate(){
        SetSteeringTargetAndCurrentAction();
        steeringManager?.Update();
        currentAction?.UpdateAction(); 
    }
    public override void Die()
    {
        isDead=true;
        base.Die();
        ChangeAction(deathAction);
    }
    public void ActualDeath(){
        base.ActualDeath();
        GameEventsManager.instance.levelEvents.KitsuneDeath(pillarID);
    }
    public override void Respawn()
    {
        base.Respawn();
        isDead=false;
        animator.SetBool("isDeadBool",false);
        animator.ResetTrigger("isDead");
        ResetSpecialAttacksCharges();
        target=null;
    }
    protected override void ResetControlBooleans()//chamado no changeAction
    {
        base.ResetControlBooleans();
        isAttacking=false;
        isResting=false;
        isDashing=false;
        isCasting=false;
    }
    public void ResetSpecialAttacksCharges(){
        dashCharges = nDashCharges;
        magicCharges = nMagicCharges;
    }
}

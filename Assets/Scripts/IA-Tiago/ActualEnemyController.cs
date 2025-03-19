using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ActualEnemyController : MonoBehaviour,ISteeringAgent,IDamagable
{
    [Header("Coisas de Save e Load")]
    [SerializeField]bool ignoreSaveLoad;
    [field:SerializeField]public string SaveId{get;private set;}//USED TO LOAD DATA
    [Header ("Coisas que precisam ser colocadas")]
    [SerializeField]protected LayerMask obstaclesLayerMask;
    [SerializeField]protected float maxVelocity;
    [SerializeField]protected float maxForce;
    [SerializeField]protected float charWidth;
    [SerializeField]protected float charHeight;
    [SerializeField]protected float maxHp;
    [SerializeField]protected float minDistToAttack;
    [SerializeField]protected int numberOfActionsBeforeRest;
    [SerializeField]protected int exp;
    [SerializeField]protected int nAttacksToPoiseBreak = 4;
    [SerializeField]protected bool IsABoss;
    [field:Header ("Coisas só pra ver mais facil")]
    [field:SerializeField]public float CurrentHp{get; protected set;}
    [field:SerializeField]public bool IsDead {get; protected set;}
    protected HealthBar healthBar;
    protected Slider poiseSlider;
    public ISteeringAgent target;
    [HideInInspector]public Rigidbody rb;
    [HideInInspector]public Animator animator;
    protected EnemyActions currentAction;
    protected EnemyActions restAction;
    [HideInInspector]public SteeringManager steeringManager;
    protected int actionsPerformed;
    protected Vector3 startingPos;
    bool initiationThroughLoad;
    protected int hitsTaken;
    [Header("Pesos dos Steering Behaviours")]
    public float lookAtTargetWeight=20;
    public float seekWeight =2;
    public float avoidObstacleWeight = 10;
    public float fleeWeight=1;
    public float wanderWeight = 10;
    
    void OnValidate(){
        steeringManager?.SetWeights(seekWeight,fleeWeight,wanderWeight,avoidObstacleWeight,lookAtTargetWeight);
    }
    public virtual void Awake(){
        //Pegar os game components precisa ficar antes do start, caso eles começem mortos e n rodem o start
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        healthBar = GetComponentInChildren<HealthBar>();
        poiseSlider = GetComponentInChildren<Slider>();
        if(poiseSlider!=null){
            poiseSlider.maxValue=nAttacksToPoiseBreak;
            poiseSlider.value = 0;
        }
        if(!ignoreSaveLoad){
            if(LevelLoadingManager.instance==null){
                Debug.LogWarning($"O inimigo {gameObject.name} está tentando se adicionar na lista de inimigos, mas não temos um LevelLoadingManger na cena");
            }
            LevelLoadingManager.instance.enemies.Add(this);
            if(SaveId==""){
                //Debug.LogWarning($"O GameObject "+gameObject.name+" está sem id e marcado para salvar, colocando o nome dele como saveId");
                SaveId=gameObject.name;
            }
            startingPos=transform.position;
        }
    }
    public void Start() { 
        
        if(animator==null)Debug.LogWarning("Enemy controller não conseguiu achar um animator");
        steeringManager=new SteeringManager(this,rb);
        if(!initiationThroughLoad)CurrentHp=maxHp;
        if(healthBar!=null){
            healthBar.SettupBarMax(maxHp);
            healthBar.SetValue(CurrentHp,false);
        }
        restAction=new nullAction();
        CreateActions();
        AdditionalStart();
    }
    protected abstract void CreateActions();//Enemy must have at least one action
    protected abstract void AdditionalStart();
    protected virtual void ResetControlBooleans(){

    }
    protected virtual void SetSteeringTargetAndCurrentAction(){
        if(target==null){
            //Debug.Log("wander");
            //adicionar depois logica de patrulha
            steeringManager.Wander();
        }
        else{
            if(actionsPerformed>=numberOfActionsBeforeRest){
                ChangeAction(restAction);
                actionsPerformed=0;
            }
            else{
                if(Vector3.SqrMagnitude(target.GetPosition()-transform.position)>minDistToAttack*minDistToAttack){
                    steeringManager.Seek(target.GetPosition());
                }
                else{
                    //if(variavelde controle == false) e etc, para controlar as ações
                    actionsPerformed++;
                    ChangeAction(new nullAction());
                }
            }
        }
    }
    public virtual void ChangeAction(EnemyActions newAction){
        ResetControlBooleans();
        currentAction?.ExitAction();
        currentAction = newAction;
        currentAction.EnterAction();
    }
    //Defines wich action will be the current, and also sets targets and weights for steeringBehaviour
    public void FixedUpdate(){
        SetSteeringTargetAndCurrentAction();
        steeringManager?.AvoidObstacle();
        steeringManager?.Update();
        currentAction?.UpdateAction(); 
    }
    public void SetTarget(ISteeringAgent steeringAgent){//controlada pela detectionArea
        if(steeringAgent==null)Debug.Log($"DefiniramMeuTarget para null");
        else Debug.Log($"DefiniramMeuTarget para alguma coisa kkkk");
        target = steeringAgent;
    }
    //Getters das interfaces
    public Vector3 GetVelocity(){return rb.linearVelocity;}

    public float GetMaxVelocity(){return maxVelocity;}

    public Vector3 GetPosition(){return transform.position;}

    public float GetMaxForce(){return maxForce;}

    public float GetSphereRadius(){return Mathf.Max(charHeight,charWidth);}

    public float GetCharHeight(){return charHeight;}

    public LayerMask GetObstaclesLayerMask(){return obstaclesLayerMask;}
    //Metodos das interfaces
    public virtual void TakeDamage(float damage, Enums.DamageType damageType,bool wasCrit)
    {
        hitsTaken++;
        if(hitsTaken>=nAttacksToPoiseBreak){
            animator.ResetTrigger("tookDamage");
            animator.SetTrigger("tookDamage");
            animator.SetBool("damageMirror", !animator.GetBool("damageMirror"));
            hitsTaken=0;
        }
        if(poiseSlider!=null){
            poiseSlider.value=hitsTaken;
        }
        switch(damageType){
            case Enums.DamageType.Regular:
                CurrentHp-=damage;
                //
            break;
            case Enums.DamageType.Magic:
                CurrentHp-=damage;
                //
            break;
        }
        if(IsABoss)UIManager.instance?.UpdateBossLife(CurrentHp,wasCrit);
        if(healthBar!=null)healthBar.SetValue(CurrentHp,wasCrit);
        if(CurrentHp<=0){
            if(IsABoss)BossDeath();
            Die();
        }
    }
    public virtual void Die(){
        GameEventsManager.instance.playerEvents.PlayerGainExp(exp);
        IsDead=true;
        Save();
        gameObject.SetActive(false);
    }
    protected virtual void BossDeath(){
        //Debug.Log("Chamei a boss death base");
        UIManager.instance?.HideBossLife();
    }
    public virtual void Load(EnemyData data){
        if(ignoreSaveLoad)return;
        IsDead=data.isDead;
        CurrentHp=data.currentLife;
        transform.position=data.lastPosition;
        Physics.SyncTransforms();
        initiationThroughLoad=true;
        if(IsDead)gameObject.SetActive(false);
    }
    public virtual void Respawn(){
        hitsTaken=0;
        if(poiseSlider!=null)poiseSlider.value=hitsTaken;
        CurrentHp = maxHp;
        IsDead=false;
        if(healthBar!=null)healthBar.SetValue(CurrentHp,false);
        transform.position=startingPos;
    }
    public virtual void Save(){

        if(ignoreSaveLoad)return;
        if(LevelLoadingManager.instance==null){
            Debug.Log($"O inimigo {SaveId} está tentando se salvar, mas não temos um LevelLoadingManger na cena");
        }
        //Debug.Log(LevelLoadingManager.instance.CurrentLevelData);
        //see if we have this data in dictionary        
        if(LevelLoadingManager.instance.CurrentLevelData.enemiesData.ContainsKey(SaveId)){
            //if so change it
            EnemyData newData = new EnemyData(this);
            LevelLoadingManager.instance.CurrentLevelData.enemiesData[SaveId]=newData;
        }
        else{
            //if not add it
            EnemyData newData = new EnemyData(this);
            LevelLoadingManager.instance.CurrentLevelData.enemiesData.Add(SaveId,newData);
        }
        
    }
    
}

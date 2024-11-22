using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public abstract class ActualEnemyController : MonoBehaviour,ISteeringAgent,IDamagable
{
    [SerializeField]protected LayerMask obstaclesLayerMask;
    [SerializeField]protected float maxVelocity;
    [SerializeField]protected float maxForce;
    [SerializeField]protected float charWidth;
    [SerializeField]protected float charHeight;
    [SerializeField]protected float maxHp;
    [SerializeField]protected float currentHp;
    [SerializeField]protected float minDistToAttack;
    [SerializeField]protected int numberOfActionsBeforeRest;
    [SerializeField]protected int exp;
    protected Slider healthSlider;
    public ISteeringAgent target;
    public Rigidbody rb;
    public Animator animator;
    protected EnemyActions currentAction;
    protected EnemyActions restAction;
    protected SteeringManager steeringManager;
    protected int actionsPerformed;
    
    public void Start() { 
        animator = GetComponentInChildren<Animator>();
        if(animator==null)Debug.LogWarning("Enemy controller não conseguiu achar um animator");
        rb = GetComponent<Rigidbody>();
        healthSlider = GetComponentInChildren<Slider>();
        steeringManager=new SteeringManager(this,rb);
        currentHp=maxHp;//mudar para save depois;
        if(healthSlider!=null){
            healthSlider.minValue=0;
            healthSlider.maxValue=maxHp;
            healthSlider.value = currentHp;
        }
        restAction=new nullAction();
        AdditionalStart();
        CreateActions();
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
    public Vector3 GetVelocity(){return rb.velocity;}

    public float GetMaxVelocity(){return maxVelocity;}

    public Vector3 GetPosition(){return transform.position;}

    public float GetMaxForce(){return maxForce;}

    public float GetSphereRadius(){return Mathf.Max(charHeight,charWidth);}

    public float GetCharHeight(){return charHeight;}

    public LayerMask GetObstaclesLayerMask(){return obstaclesLayerMask;}
    //Metodos das interfaces
    public void TakeDamage(float damage, Enums.DamageType damageType)
    {
        animator.ResetTrigger("tookDamage");
        animator.SetTrigger("tookDamage");
        animator.SetBool("damageMirror", !animator.GetBool("damageMirror"));
        switch(damageType){
            case Enums.DamageType.Regular:
                currentHp-=damage;
                //
            break;
            case Enums.DamageType.Magic:
                currentHp-=damage;
                //
            break;
        }
        healthSlider.value=currentHp;
        if(currentHp<=0){
            Die();
        }
    }
    public void Die(){
        GameEventsManager.instance.playerEvents.PlayerGainExp(exp);
        Destroy(gameObject);
    }
    
}

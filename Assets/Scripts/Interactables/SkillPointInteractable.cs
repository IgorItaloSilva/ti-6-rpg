using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System;

public class SkillPointInteractable : Interactable, IDamagable
{
    [SerializeField]int hitsToDestroy = 3;
    [SerializeField]GameObject canvas;
    bool didntAddInteract;
    int life;
    void OnEnable(){
        if(!PlayerStateMachine.Instance)didntAddInteract=true;
        PlayerStateMachine.Instance?.AddActionToInteract(Interact);
    }
    void OnDisable(){
        PlayerStateMachine.Instance.RemoveActionFromInteract(Interact);
    }

    protected override void Start()
    {
        if(didntAddInteract){
            PlayerStateMachine.Instance?.AddActionToInteract(Interact);
        }
        base.Start();
        life = hitsToDestroy;
    }

    public void TakeDamage(float damage, Enums.DamageType damageType, bool wasCrit)
    {
        if(CanInteract){
            life--;
            if(life<=0){
                Die();
            }
        }
    }
    virtual public void Die()
    {
        SkillTree.instance?.GainMoney((int)Enums.PowerUpType.Dark);
        AlreadyInterated=true;
        Active=false;
        Save();
        gameObject.SetActive(false);
        
    }
    protected override void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player")){
            canvas.gameObject.SetActive(true);
            inRange=true;
        }
    }
    void OnTriggerExit(Collider collider){
        if(collider.CompareTag("Player")){
            inRange = false;
            canvas.gameObject.SetActive(false);
        }
    }
    void OnTriggerStay(Collider collider){
        if(collider.CompareTag("Player")){
            Vector3 lookAt = new Vector3(collider.transform.position.x,canvas.transform.position.y,collider.transform.position.z);
            canvas.transform.LookAt(lookAt);
            canvas.transform.Rotate(new Vector3(0,180,0));
        }
    }
    void Interact(InputAction.CallbackContext context){
        if(inRange&&CanInteract&&!AlreadyInterated){
            PlayerStateMachine.Instance.CanInteract=false;
            PlayerStateMachine.Instance.Animator.ResetTrigger(PlayerStateMachine.Instance.HasPrayedHash);
            PlayerStateMachine.Instance.Animator.SetTrigger(PlayerStateMachine.Instance.HasPrayedHash);
            PlayerStateMachine.Instance.Animator.SetFloat(PlayerStateMachine.Instance.PlayerVelocityXHash, 0f);
            PlayerStateMachine.Instance.Animator.SetFloat(PlayerStateMachine.Instance.PlayerVelocityYHash, 0f);
            PlayerStateMachine.Instance.LockPlayer();
            Rescue();
        }
    }
    protected virtual void Rescue(){
        SkillTree.instance?.GainMoney((int)Enums.PowerUpType.Light);
        AlreadyInterated=true;
        Active=false;
        Save();
        gameObject.SetActive(false);
        //se monstro
        //deixa o corpo parado ali?
        //vfx de purificar?
        //se arvore
        //destroy a arvore? toca um vfx?
    }
    public override void Load(InteractableData interactableData)
    {
        base.Load(interactableData);
        if(AlreadyInterated||!Active)gameObject.SetActive(false);
    }
}

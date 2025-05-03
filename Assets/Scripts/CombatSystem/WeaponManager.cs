using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class WeaponManager : MonoBehaviour
{
    protected float damage;
    [SerializeField]protected bool showDebug = false;
    [SerializeField]protected Enums.DamageType damageType;
    [SerializeField]protected float baseDamage = 10f;
    protected Collider damageCollider;
    protected List<IDamagable>damagedTargets = new List<IDamagable>();
    protected ActualEnemyController actualEnemyController;
    
    protected virtual void Start(){
        damageCollider=GetComponent<Collider>();
        actualEnemyController = GetComponentInParent<ActualEnemyController>();
        if(actualEnemyController==null)Debug.LogWarning("A arma não achou o enemyControllerDela");
        if(damageCollider==null){
            Debug.LogWarning($"O weapon manager do {name} não achou o collider dela");
        }
        damage=baseDamage;
    }
    protected virtual void OnTriggerEnter(Collider other){
        //Debug.Log("A arma colidiu com algo");
        //if(!other.gameObject.CompareTag("EnemyDetection")&&!other.CompareTag("Enemy")) 
        //não tem pra que tirar todas as excessoes quando a gente pode só colocar o caso do jogador aqui
        if(other.CompareTag("Player"))
        {
            IDamagable alvoAtacado = other.gameObject.GetComponentInParent<IDamagable>();
            //Debug.Log($"A interface Idamageble que eu peguei foi {alvoAtacado}");
            if (alvoAtacado != null)
            {
                if(PlayerStateMachine.Instance.IsBlocking){
                    if(PlayerStateMachine.Instance.ShouldParry){
                        actualEnemyController.WasParried();
                        AudioPlayer.instance.PlaySFX("Parry");
                        PlayerStateMachine.Instance.Animator.ResetTrigger(PlayerStateMachine.Instance.HasParried);
                        PlayerStateMachine.Instance.Animator.SetTrigger(PlayerStateMachine.Instance.HasParried);
                        DisableCollider();
                    }
                    else{
                        AudioPlayer.instance.PlaySFX("Block");
                        PlayerStateMachine.Instance.Animator.ResetTrigger(PlayerStateMachine.Instance.TookHitHash);
                        PlayerStateMachine.Instance.Animator.SetTrigger(PlayerStateMachine.Instance.TookHitHash);
                        DealDamage(alvoAtacado,damage/3);
                    }
                }
                else{
                    AudioPlayer.instance.PlaySFX("PlayerDamage" + Random.Range(1, 3));
                    PlayerStateMachine.Instance.Animator.ResetTrigger(PlayerStateMachine.Instance.TookHitHash);
                    PlayerStateMachine.Instance.Animator.SetTrigger(PlayerStateMachine.Instance.TookHitHash);
                    PlayerStateMachine.Instance.ResetAttacks();
                    DealDamage(alvoAtacado, damage);
                }
            }
        }
    }
    protected virtual void DealDamage(IDamagable alvo, float dano){
        if(damagedTargets.Contains(alvo)){
            return;
        }
        damagedTargets.Add(alvo);
        if(!PlayerStateMachine.Instance.IsDodging)
        {
            alvo.TakeDamage(dano, damageType, false);
            //Criar um texto de dano na tela
            if(showDebug)Debug.Log($"Enviei {dano} de dano para ser tomado para {alvo}");
        }
        else
        {
            if(showDebug)Debug.Log($"Não enviei {dano} de dano para ser tomado para {alvo}. Ele está desviando!");
        }
    }
    public void EnableCollider(){
        damageCollider.enabled=true;
    }
    public void DisableCollider(){
        damageCollider.enabled=false;
        damagedTargets.Clear();
    }
    
}

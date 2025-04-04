using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class WeaponManager : MonoBehaviour
{
    protected float damage;
    [SerializeField]bool showDebug = false;
    [SerializeField]protected Enums.DamageType damageType;
    [SerializeField]protected float baseDamage = 10f;
    protected Collider damageCollider;
    protected List<IDamagable>damagedTargets = new List<IDamagable>();
    
    protected virtual void Start(){
        damageCollider=GetComponent<Collider>();
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
                DealDamage(alvoAtacado, damage);
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
            alvo.TakeDamage(damage, damageType, false);
            //Criar um texto de dano na tela
            Debug.Log($"Enviei {damage} de dano para ser tomado para {alvo}");
        }
        else
        {
            Debug.Log($"Não enviei {damage} de dano para ser tomado para {alvo}. Ele está desviando!");
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

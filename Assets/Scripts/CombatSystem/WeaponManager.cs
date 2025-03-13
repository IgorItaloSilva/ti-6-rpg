using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class WeaponManager : MonoBehaviour
{
    protected float damage;
    [SerializeField]protected Enums.DamageType damageType;
    [SerializeField]protected float baseDamage = 10f;
    protected Collider damageCollider;
    protected List<IDamagable>damagedTargets = new List<IDamagable>();
    
    protected virtual void Start(){
        damageCollider=GetComponent<Collider>();
        if(damageCollider==null){
            Debug.LogWarning("A arma não achou o collider dela");
        }
        damage=baseDamage;
    }
    protected virtual void OnTriggerEnter(Collider other){
        //Debug.Log("A arma colidiu com algo");
        if(!other.gameObject.CompareTag("EnemyDetection")&&!other.CompareTag("Enemy"))
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
        alvo.TakeDamage(damage,damageType,false);
        //Criar um texto de dano na tela
        //Debug.Log($"Enviei {damage} de dano para ser tomado para {alvo}");
    }
    public void EnableCollider(){
        damageCollider.enabled=true;
    }
    public void DisableCollider(){
        damageCollider.enabled=false;
        damagedTargets.Clear();
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class WeaponManager : MonoBehaviour
{
    private float damage;
    [SerializeField]private float strModifier = 0.5f;
    [SerializeField]private float dexModifier = 0.1f;
    [SerializeField]private float baseDamage = 10f;
    Collider damageCollider;
    //Weapon pinduricalhos
    private void Start(){
        damageCollider=GetComponent<Collider>();
        if(damageCollider==null){
            Debug.LogWarning("A arma não achou o collider dela");
        }
        damage=baseDamage;//Deveria ser substituido por uma chamada do controller de quem tem essa arma
    }
    List<IDamagable>damagedTargets = new List<IDamagable>();
    private void OnTriggerEnter(Collider collider){
        Debug.Log("A arma colidiu com algo");
        IDamagable alvoAtacado = collider.gameObject.GetComponentInParent<IDamagable>();
        if(alvoAtacado!=null){
            DealDamage(alvoAtacado,damage);
        }
    }
    private void DealDamage(IDamagable alvo, float dano){
        if(damagedTargets.Contains(alvo)){
            return;
        }
        alvo.TakeDamage(damage);
        Debug.Log($"Enviei {damage} de dano para ser tomado para {alvo}");
        damagedTargets.Add(alvo);
    }
    public void EnableCollider(){
        damageCollider.enabled=true;
    }
    public void DisableCollider(){
        damageCollider.enabled=false;
        damagedTargets.Clear();
    }
    public void SetDamage(int str,int dex){
        damage = baseDamage + str*strModifier + dex*dexModifier;
    }
    
}

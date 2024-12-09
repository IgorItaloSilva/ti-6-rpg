using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class WeaponManager : MonoBehaviour
{
    private float damage;
    [SerializeField] Enums.DamageType damageType;
    [SerializeField]private float strModifier = 0.5f;
    [SerializeField]private float dexModifier = 0.3f;
    [SerializeField]private float baseDamage = 10f;
    [SerializeField]bool isPlayerWeapon;
    //public float bonusDamageFromRune;
    Collider damageCollider;
    //Weapon pinduricalhos
    void OnEnable(){
        GameEventsManager.instance.runeEvents.onRuneDamageBuff+=RuneDamageBuff;
    }   
    void OnDisable(){
        GameEventsManager.instance.runeEvents.onRuneDamageBuff-=RuneDamageBuff;
    }
    private void Start(){
        damageCollider=GetComponent<Collider>();
        if(damageCollider==null){
            Debug.LogWarning("A arma não achou o collider dela");
        }
        damage=baseDamage;
    }
    List<IDamagable>damagedTargets = new List<IDamagable>();
    private void OnTriggerEnter(Collider collider){
        //Debug.Log("A arma colidiu com algo");
        IDamagable alvoAtacado = collider.gameObject.GetComponentInParent<IDamagable>();
        if(alvoAtacado!=null){
            DealDamage(alvoAtacado,damage);
        }
    }
    private void DealDamage(IDamagable alvo, float dano){
        if(damagedTargets.Contains(alvo)){
            return;
        }
        alvo.TakeDamage(damage,damageType);
        //Debug.Log($"Enviei {damage} de dano para ser tomado para {alvo}");
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
    public void RuneDamageBuff(bool isApply, int value){
        if(!isPlayerWeapon)return;
        Debug.Log($"Estou mudando o meu dano de {damage}, o novo valor é {value} e o isApply é {isApply}");
        if(isApply){
            damage+=value;
        }
        else{
            damage-=value;
        }
    }
    
}

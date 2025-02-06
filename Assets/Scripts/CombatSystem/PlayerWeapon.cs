using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : WeaponManager
{
    float critRate = 5;
    float heavyAttackDamage = 0;
    float lightAttackDamage = 0;
    float strBonusDamage;//recived from playerStats
    float dexBonusDamage;//recived from playerStats
    //Weapon pinduricalhos
    float runeBonusDamage = 0;
    void OnEnable(){
        GameEventsManager.instance.runeEvents.onRuneDamageBuff+=RuneDamageBuff;
    }   
    void OnDisable(){
        GameEventsManager.instance.runeEvents.onRuneDamageBuff-=RuneDamageBuff;
    }
    protected override void DealDamage(IDamagable alvo, float dano)
    {
        if(damagedTargets.Contains(alvo)){
            return;
        }
        damagedTargets.Add(alvo);
        if(Random.Range(0f,100f)<=critRate){
            alvo.TakeDamage(damage*2,damageType);
        }
        alvo.TakeDamage(damage,damageType);
        //Criar um texto de dano na tela
        Debug.Log($"Enviei {damage} de dano para ser tomado para {alvo}");
    }
    public void SetDamageAndValues(float strongAttackBonus, float fastAttackBonus){
        strBonusDamage = strongAttackBonus;
        dexBonusDamage = fastAttackBonus;
        SetDamage();
    }
    void SetDamage(){
        heavyAttackDamage = baseDamage+strBonusDamage+runeBonusDamage;
        lightAttackDamage = baseDamage+dexBonusDamage+runeBonusDamage;
    }
    public void SetDamageType(int attackType){
        if(attackType==1){
            damage = lightAttackDamage;
        }
        else{
            damage = heavyAttackDamage;
        }
    }
     public void RuneDamageBuff(bool isApply, int value){
        if(isApply){
            runeBonusDamage = value;
        }
        else{
            runeBonusDamage=0;;
        }
        SetDamage();
    }
}

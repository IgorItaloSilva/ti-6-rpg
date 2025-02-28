using System.Threading.Tasks;
using UnityEditor.VersionControl;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

public class PlayerWeapon : WeaponManager
{
    [SerializeField]float critRate = 5;
    float heavyAttackDamage = 0;
    float lightAttackDamage = 0;
    float strBonusDamage;//recived from playerStats
    float dexBonusDamage;//recived from playerStats
    //Weapon pinduricalhos
    float runeBonusDamage = 0;
    //Skill tree powerUps
    int doubleDamageMultiplier =1;
    bool executeEnemiesPUActive;
    bool lifeStealPUActive;
    [SerializeField] Animator _animator;
    
    void OnEnable(){
        GameEventsManager.instance.runeEvents.onRuneDamageBuff+=RuneDamageBuff;
        GameEventsManager.instance.skillTreeEvents.onActivatePowerUp+=ActivatePowerUps;
    }   
    void OnDisable(){
        GameEventsManager.instance.runeEvents.onRuneDamageBuff-=RuneDamageBuff;
        GameEventsManager.instance.skillTreeEvents.onActivatePowerUp-=ActivatePowerUps;
    }
    protected override void DealDamage(IDamagable alvo, float dano)
    {
        float damageDealt;
        bool crited = false;
        if(damagedTargets.Contains(alvo)){
            return;
        }
        damagedTargets.Add(alvo);
        //CritLogic
        if(Random.Range(0f,100f)<=critRate){
            damageDealt=damage*2*doubleDamageMultiplier;
            crited=true;
        }
        else{
            damageDealt=damage*doubleDamageMultiplier;
        }
        if(lifeStealPUActive){
            GameEventsManager.instance.skillTreeEvents.LifeStealHit(damageDealt/2);
            Debug.Log($"Curando o jogador com lifeSteal de {damageDealt/2}");
        }
        alvo.TakeDamage(damageDealt,damageType,crited);
        Debug.Log($"Enviei {damageDealt} de dano para ser tomado para {alvo}");

        if(_animator){
            HitAnimatorPause();
        }
    }

    private async void HitAnimatorPause()
    {
        await Task.Delay(25);
        _animator.speed = 0f;
        await Task.Delay(125);
        _animator.speed = 1f;
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
    public void SetDamageType(int attackType){//Chamado pelo playerStateMachine
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
    void ActivatePowerUps(int id){
        switch(id){
            //DoubleDamage
            case 10:
                doubleDamageMultiplier=2;
            break;
            //Execute
            case 13:
            break;
            //LifeSteal
            case 14:
                lifeStealPUActive=true;
            break;
        }
    }
}

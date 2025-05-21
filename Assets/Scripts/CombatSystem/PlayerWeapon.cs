using UnityEngine;
using Task = System.Threading.Tasks.Task;

public class PlayerWeapon : WeaponManager
{
    [SerializeField]float critRate = 5;
    float heavyAttackDamage = 0;
    float lightAttackDamage = 0;
    float strBonusDamage;//recived from playerStats
    float dexBonusDamage;//recived from playerStats
    float critRateBonus;//recived from runeManager
    //Weapon pinduricalhos
    float runeBonusDamage = 0;
    //Skill tree powerUps
    int doubleDamageMultiplier =1;
    bool executeEnemiesPUActive;
    bool lifeStealPUActive;
    
    void OnEnable(){
        GameEventsManager.instance.runeEvents.onRuneDamageBuff+=RuneDamageBuff;
        GameEventsManager.instance.runeEvents.onRuneOtherBuff+=RuneOtherBuff;
        GameEventsManager.instance.skillTreeEvents.onActivatePowerUp+=ActivatePowerUps;
    }   
    void OnDisable(){
        GameEventsManager.instance.runeEvents.onRuneDamageBuff-=RuneDamageBuff;
        GameEventsManager.instance.runeEvents.onRuneOtherBuff-=RuneOtherBuff;
        GameEventsManager.instance.skillTreeEvents.onActivatePowerUp-=ActivatePowerUps;
    }
    protected override void Start()
    {
        damageCollider=GetComponent<Collider>();
        if(damageCollider==null){
            Debug.LogWarning($"O weapon manager do {name} não achou o collider dela");
        }
        damage=baseDamage;
    }
    override protected void OnTriggerEnter(Collider other){
        Debug.Log($"A arma colidiu com um {other.name}");
        if(!other.gameObject.CompareTag("EnemyDetection"))
        {
            IDamagable alvoAtacado = other.gameObject.GetComponentInParent<IDamagable>();
            Debug.Log($"A interface Idamageble que eu peguei foi {alvoAtacado}");
            if (alvoAtacado != null && alvoAtacado.GetType() != typeof(PlayerStats))
            {
                DealDamage(alvoAtacado, damage);
                DisableCollider();
            }
        }
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
        if(Random.Range(0f,100f)<=critRate+critRateBonus){
            damageDealt=damage*2*doubleDamageMultiplier;
            crited=true;
        }
        else{
            damageDealt=damage*doubleDamageMultiplier;
        }
        if(lifeStealPUActive){
            GameEventsManager.instance.skillTreeEvents.LifeStealHit(damageDealt/2);
            if(showDebug)Debug.Log($"Curando o jogador com lifeSteal de {damageDealt/2}");
        }
        alvo.TakeDamage(damageDealt,damageType,crited);
        if(showDebug)Debug.Log($"Enviei {damageDealt} de dano para ser tomado para {alvo}");
        
        HitAnimatorPause();
    }

    private async void HitAnimatorPause()
    {
        if(showDebug)Debug.Log("Paused animation");
        PlayerStateMachine.Instance.Animator.speed = 0f;
        await Task.Delay(100);
        PlayerStateMachine.Instance.Animator.speed = 1f;
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
    public void SetDamageType(Enums.AttackType attackType){//Chamado pelo playerStateMachine
        switch(attackType){
            case Enums.AttackType.LightAttack:
                damage = lightAttackDamage;
            break;
            case Enums.AttackType.HeavyAttack:
                damage = heavyAttackDamage;
            break;
        }
        
    }
     public void RuneDamageBuff(bool isActivate, int value){
        if(isActivate){
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
    public void RuneOtherBuff(bool isActivate,Enums.RuneOtherCode code,int amount){
        if(isActivate){
            switch(code){
                case Enums.RuneOtherCode.Critico:
                    critRateBonus = amount;
                break;
            }
        }
        else{
            switch(code){
                case Enums.RuneOtherCode.Critico:
                    critRateBonus=0;
                break;
            }
        }
    }
}

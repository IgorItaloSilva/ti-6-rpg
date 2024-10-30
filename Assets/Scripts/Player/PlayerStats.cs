using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStats : MonoBehaviour, IDataPersistence,IDamagable
{
    /*  Essa classe foi pensada em ser a classe principal para lidar com os status do jogador,
        ela mantem vários atributos que serão usados em vários outros scripts, e é responsavel por eles.

        Atualemente ela tem poucas funcionalidades, e nem todos os atributos que ela provavelmente deveria ter,
        eles serão adicionados conforme eles forem implementados e nos tivermos uma ideia melhor de como eles vão
        funcionar

        POSSIVELMENTE ADICIONAR MANA E STAMINA AINDA
    */
    //Esses valores estão aqui para testes, depeois de definidos eles devem ser colocados no scriptableObjects
    [SerializeField]float vidaConsMod;
    [SerializeField]float manaIntMod;
    [SerializeField]float magicDamageMod;
    [SerializeField]float lightAttackDamageMod;
    [SerializeField]float heavyAttackDamageMod;
    public int Con {get; private set;}
    public int Str {get; private set;}
    public int Dex {get; private set;}
    public int Int {get; private set;}
    public float Exp {get; private set;}
    public int Level {get; private set;}
    public float CurrentLife{get; private set;}
    public float CurrentMana{get; private set;}
    public float BaseLife{get;private set;}
    public float BaseMana{get;private set;}
    public float BaseMagicDamage{get;private set;}
    public float BaseLightAttackDamage{get;private set;}
    public float BaseHeavyAttackDamage{get;private set;}
    private float maxLife;//testar com valores 1000 + 25*Con
    private float maxMana;
    float magicDamage;
    float lightAttackDamage;
    float heavyAttackDamage;
    private PlayerStatsData statsLoadados; 
    //CONTROLES DOS POWER UPS
    private bool PUArmorActive;
    private bool PULifeRegenActive;
    // Start is called before the first frame update
    void Start()
    {
        CalculateStats();
        GameEventsManager.instance.uiEvents.UpdateSliders(0,0,maxLife);//Essas duas funções deveriam ser chamadas
        GameEventsManager.instance.uiEvents.LifeChange(CurrentLife);//             pra stamina e mana tambem
    }
    void OnEnable(){
        GameEventsManager.instance.uiEvents.onRequestBaseStatsInfo+=SendBaseStatsInfo;
        GameEventsManager.instance.uiEvents.onRequestExpStatsInfo+=SendExpStatsInfo;
        GameEventsManager.instance.uiEvents.onRequestAdvancedStatsInfo+=SendAdvancedStatsInfo;
    }
    void OnDisable(){
        GameEventsManager.instance.uiEvents.onRequestBaseStatsInfo-=SendBaseStatsInfo;
        GameEventsManager.instance.uiEvents.onRequestExpStatsInfo-=SendExpStatsInfo;
        GameEventsManager.instance.uiEvents.onRequestAdvancedStatsInfo-=SendAdvancedStatsInfo;
    }

    // Update is called once per frame
    void Update()
    {
        GameEventsManager.instance.uiEvents.UpdateSliders(0,0,maxLife);//Essas duas funções deveriam ser chamadas
        GameEventsManager.instance.uiEvents.LifeChange(CurrentLife);
    }
    public void TakeDamage(float dano){
        CurrentLife -= dano;
        GameEventsManager.instance.uiEvents.LifeChange(CurrentLife);
        if(CurrentLife<=0){
            Die();
        }
    }
    public void HealLife(float life){
        if(CurrentLife<maxLife){
            CurrentLife += life;
            if(CurrentLife>maxLife)CurrentLife=maxLife;
            GameEventsManager.instance.uiEvents.LifeChange(CurrentLife);
        }
        
    }
    private void Die(){
        GameEventsManager.instance.playerEvents.PlayerDied();
        //Adicionar logica de reload
        //Aviso de UI
    }
    public void SaveData(GameData data){
        PlayerStatsData playerStatsData = new PlayerStatsData(this);
    }
    public void LoadData(GameData data){
        this.Con = data.playerStatsData.con;
        this.Str = data.playerStatsData.str;
        this.Dex = data.playerStatsData.dex;
        this.Int = data.playerStatsData.inte;
        this.Exp = data.playerStatsData.exp;
        this.Level = data.playerStatsData.level;
        this.CurrentLife = data.playerStatsData.currentLife;
        this.BaseLife = data.playerStatsData.baseLife;
        this.BaseMana = data.playerStatsData.baseMana;
        this.CurrentMana = data.playerStatsData.currentMana;
        this.BaseMagicDamage = data.playerStatsData.baseMagicDamage;
        this.BaseLightAttackDamage = data.playerStatsData.baseLightAttackcDamage;
        this.BaseHeavyAttackDamage = data.playerStatsData.baseHeavyAttackDamage;
        CalculateStats();
    }
    public void TakeDamage(float damage,Enums.DamageType damageType)
    {
        if(PUArmorActive) damage/=2;
        TakeDamage(damage);
    }
    private void ActivatePowerUp(int id){//OBS OS IDS SÃO HARD CODED, SE MUDAR A ORDEM DELES PRECISA MUDAR AQUI!!!!!!!
        switch(id){
            case 3: PUArmorActive=true;Debug.Log("Ativei o powerUp armor");break;
            case 7: PULifeRegenActive=true;InvokeRepeating("LifeRegenPowerUp",0f,5f);Debug.Log("Ativei o powerUp 7"); break;
            default: break;
        }
    }
    private void LifeRegenPowerUp(){
        if(PUArmorActive){
            if(CurrentLife<maxLife){
                float lifeMissing = maxLife-CurrentLife;
                float healRatio = lifeMissing/10;
                if(healRatio<50f)lifeMissing=50f;
                HealLife(healRatio);
            }
        }
    }
    void CalculateStats(){
        maxLife = BaseLife + vidaConsMod * Con;
        maxMana = BaseMana + manaIntMod * Int;
        magicDamage = BaseMagicDamage + magicDamageMod * Int;
        lightAttackDamage = BaseLightAttackDamage + lightAttackDamageMod * Dex;
        heavyAttackDamage = BaseHeavyAttackDamage + heavyAttackDamageMod * Str;
    }
    //ADICIONAR COISAS DE GANHAR EXP E LEVEL UP
    // EXP PARA O PROXIMO NIVEL = 100*2^(L-1) onde L é o nivel atual
    void SendBaseStatsInfo(){
        GameEventsManager.instance.uiEvents.ReciveBaseStatsInfo(Con,Str,Dex,Int);
    }
    void SendExpStatsInfo(){
        GameEventsManager.instance.uiEvents.ReciveExpStatsInfo(Level,Exp);
    }
    void SendAdvancedStatsInfo(){
        GameEventsManager.instance.uiEvents.ReciveAdvancedStatsInfo(CurrentLife,maxLife,CurrentMana,maxMana,magicDamage
        ,lightAttackDamage,heavyAttackDamage);
    }
}

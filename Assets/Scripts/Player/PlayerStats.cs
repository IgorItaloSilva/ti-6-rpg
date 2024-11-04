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
    [SerializeField]float vidaConsMod = 25;
    [SerializeField]float manaIntMod = 10;
    [SerializeField]float magicDamageMod = 10;
    [SerializeField]float lightAttackDamageMod = 5;
    [SerializeField]float heavyAttackDamageMod = 8;
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
    //Coisas de level up
    public int levelUpPoints{get;private set;}//adicionar no save/load depois
    private int spentPointsIfCancel;
    public int[] simulatedStatChange;
    public bool IsNearCampfire ;//{get;private set;}//adicionar no save/load depois
    private bool playerIsDead; //Talvez Adicionar no save e load depois
    void Start()
    {
        levelUpPoints=3;
        IsNearCampfire=true;
        simulatedStatChange = new int[4];
        CalculateStats();
        GameEventsManager.instance.uiEvents.UpdateSliders(0,0,maxLife);//Essas duas funções deveriam ser chamadas
        GameEventsManager.instance.uiEvents.LifeChange(CurrentLife);//             pra stamina e mana tambem
    }
    void OnEnable(){
        GameEventsManager.instance.uiEvents.onRequestBaseStatsInfo+=SendBaseStatsInfo;
        GameEventsManager.instance.uiEvents.onRequestExpStatsInfo+=SendExpStatsInfo;
        GameEventsManager.instance.uiEvents.onRequestAdvancedStatsInfo+=SendAdvancedStatsInfo;
        GameEventsManager.instance.uiEvents.onRequestLevelUpInfo+=SendLevelUpInfo;
        GameEventsManager.instance.uiEvents.onChangeStatusButtonPressed+=SimulateStatusBuyOrSell;
        GameEventsManager.instance.uiEvents.onConfirmLevelUp+=ConfirmChanges;
        GameEventsManager.instance.uiEvents.onDiscardLevelUp+=DiscardChanges;
        GameEventsManager.instance.playerEvents.onPlayerRespawned+=PlayerRespawn;
    }
    void OnDisable(){
        GameEventsManager.instance.uiEvents.onRequestBaseStatsInfo-=SendBaseStatsInfo;
        GameEventsManager.instance.uiEvents.onRequestExpStatsInfo-=SendExpStatsInfo;
        GameEventsManager.instance.uiEvents.onRequestAdvancedStatsInfo-=SendAdvancedStatsInfo;
        GameEventsManager.instance.uiEvents.onRequestLevelUpInfo-=SendLevelUpInfo;
        GameEventsManager.instance.uiEvents.onChangeStatusButtonPressed-=SimulateStatusBuyOrSell;
        GameEventsManager.instance.uiEvents.onConfirmLevelUp-=ConfirmChanges;
        GameEventsManager.instance.uiEvents.onDiscardLevelUp+=DiscardChanges;
        GameEventsManager.instance.playerEvents.onPlayerRespawned-=PlayerRespawn;
    }

    // Update is called once per frame
    void Update()
    {
        GameEventsManager.instance.uiEvents.UpdateSliders(0,0,maxLife);//Essas duas funções deveriam ser chamadas
        GameEventsManager.instance.uiEvents.LifeChange(CurrentLife);
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
        playerIsDead = true;
        //DesativarInputs
    }
    private void PlayerRespawn(){
        playerIsDead=false;
    }
    public void SaveData(GameData data){
        PlayerStatsData playerStatsData = new PlayerStatsData(this);
        data.playerStatsData = playerStatsData;
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
        CurrentLife -= damage;
        GameEventsManager.instance.uiEvents.LifeChange(CurrentLife);
        if(CurrentLife<=0&&!playerIsDead){
            Die();
        }
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
        maxLife = BaseLife + vidaConsMod * (Con-10);
        maxMana = BaseMana + manaIntMod * (Int-10);
        magicDamage = BaseMagicDamage + magicDamageMod * (Int-10);
        lightAttackDamage = BaseLightAttackDamage + lightAttackDamageMod * (Dex-10);
        heavyAttackDamage = BaseHeavyAttackDamage + heavyAttackDamageMod * (Str-10);
    }
    //ADICIONAR COISAS DE GANHAR EXP E LEVEL UP
    // EXP PARA O PROXIMO NIVEL = 100*2^(L-1) onde L é o nivel atual
    //FUNCÕES PARA ENVIAR OS EVENTOS
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
    void SendLevelUpInfo(){
        GameEventsManager.instance.uiEvents.ReciveLevelUpInfo(levelUpPoints,IsNearCampfire);
    }
    //Coisas de level up
    public void SimulateStatusBuyOrSell(int statusId,bool isBuying){
        
        if(isBuying){
            if(levelUpPoints<=0)return;
            simulatedStatChange[statusId]++;
            levelUpPoints--;
            spentPointsIfCancel++;
        }
        else{
            if(simulatedStatChange[statusId]<1)return;
            simulatedStatChange[statusId]--;
            levelUpPoints++;
            spentPointsIfCancel--;
        }
        SendLevelUpInfo();
        SimulateStatusChange(statusId);
        
    }
    void SimulateStatusChange(int id){
        int displayValue = 0;
        bool isDifferent = (simulatedStatChange[id]!=0) ? true:false;
        switch(id){
            case 0:
                displayValue = Con+simulatedStatChange[id];
            break;
            case 1:
                displayValue = Dex+simulatedStatChange[id];
            break;
            case 2:
                displayValue = Str+simulatedStatChange[id];
            break;
            case 3:
                displayValue = Int+simulatedStatChange[id];
            break;
        }
        GameEventsManager.instance.uiEvents.SimulateChangeBaseValue(id,displayValue,isDifferent);
        CalculateAdvancedInfoAndSend(id);
    }
    void CalculateAdvancedInfoAndSend(int id){
        bool isDifferent = (simulatedStatChange[id]!=0) ? true:false;
        switch(id){
            case 0:
                float simuMaxLife = BaseLife + vidaConsMod * (Con+simulatedStatChange[0]-10);
                GameEventsManager.instance.uiEvents.SimulateChangeAdvancedValue(0,CurrentLife,simuMaxLife,isDifferent);
            break;
            case 1:
                float simuLightAttackDamage = BaseLightAttackDamage + lightAttackDamageMod * (Dex+simulatedStatChange[1]-10);
                GameEventsManager.instance.uiEvents.SimulateChangeAdvancedValue(2,0,simuLightAttackDamage,isDifferent);
            break;
            case 2:
                float simuHeavyAttackDamage = BaseHeavyAttackDamage + heavyAttackDamageMod * (Str+simulatedStatChange[2]-10);
                GameEventsManager.instance.uiEvents.SimulateChangeAdvancedValue(3,0,simuHeavyAttackDamage,isDifferent);
            break;
            case 3:
            int newIntValue = Int+simulatedStatChange[3];
                float simuMaxMana = BaseMana + manaIntMod * newIntValue-10;
                float simuMagicDamage = BaseMagicDamage + magicDamageMod * newIntValue-10;
                GameEventsManager.instance.uiEvents.SimulateChangeAdvancedValue(1,CurrentMana,simuMaxMana,isDifferent);
                GameEventsManager.instance.uiEvents.SimulateChangeAdvancedValue(4,0,simuMagicDamage,isDifferent);
            break;
        }
    }
    void ConfirmChanges(){
        Con+=simulatedStatChange[0];
        Dex+=simulatedStatChange[1];
        Str+=simulatedStatChange[2];
        Int+=simulatedStatChange[3];
        CalculateStats();
        SendBaseStatsInfo();
        SendAdvancedStatsInfo();
        for(int i=0;i<simulatedStatChange.Length;i++)simulatedStatChange[i]=0;
        spentPointsIfCancel=0;
        SendLevelUpInfo();
    }
    void DiscardChanges(){
        levelUpPoints= levelUpPoints+spentPointsIfCancel;
        spentPointsIfCancel=0;
    }
}

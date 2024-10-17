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
    [SerializeField]float vidaModCons;
    public int Con {get; private set;}
    public int Str {get; private set;}
    public int Dex {get; private set;}
    public int Int {get; private set;}
    public float Exp {get; private set;}
    public float Level {get; private set;}
    public float CurrentLife{get; private set;}
    public float BaseLife{get;private set;}
    private float maxLife;//testar com valores 1000 + 25*Con
    private PlayerStatsData statsLoadados; 
    //CONTROLES DOS POWER UPS
    private bool PUArmorActive;
    private bool PULifeRegenActive;
    // Start is called before the first frame update
    void Start()
    {
        maxLife = BaseLife + vidaModCons * Con;
        GameEventsManager.instance.uiEvents.UpdateSliders(0,0,maxLife);//Essas duas funções deveriam ser chamadas
        GameEventsManager.instance.uiEvents.LifeChange(CurrentLife);//             pra stamina e mana tambem
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
    public void DisplayStats(){
        //provavelmente vou mudar essa função e esse event como um todo
        GameEventsManager.instance.uiEvents.StatsDisplay(Con,Str,Dex,Int,Level,Exp);
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
        this.CurrentLife = data.playerStatsData.vidaAtual;
        this.BaseLife = data.playerStatsData.vidaBase;
        UpdateStatusEvent();
    }
    private void UpdateStatusEvent(){
        //GameEventsManager.instance.playerEvents.onStatusChanged
    }

    public void TakeDamage(float damage,Enums.DamageType damageType)
    {
        if(PUArmorActive) damage/=2;
        TakeDamage(damage);
    }
    private void ActivatePowerUp(int id){//OBS OS IDS SÃO HARD CODED, SE MUDAR A ORDEM DELES PRECISA MUDAR AQUI!!!!!!!
        switch(id){
            case 3: PUArmorActive=true;Debug.Log("Ativei o powerUp armor");break;
            case 7: PULifeRegenActive=true;InvokeRepeating("LifeRegen",0f,5f);Debug.Log("Ativei o powerUp 7"); break;
            default: break;
        }
    }
    private void LifeRegen(){
        if(PUArmorActive){
            if(CurrentLife<maxLife){
                float lifeMissing = maxLife-CurrentLife;
                float healRatio = lifeMissing/10;
                if(healRatio<50f)lifeMissing=50f;
                HealLife(healRatio);
            }
        }
    }
}

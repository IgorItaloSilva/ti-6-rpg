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
    public float VidaAtual{get; private set;}
    public float VidaBase{get;private set;}
    private float vidaMax;//testar com valores 1000 + 25*Con
    private PlayerStatsData statsLoadados; 
    // Start is called before the first frame update
    void Start()
    {
        vidaMax = VidaBase + vidaModCons * Con;
        GameEventsManager.instance.uiEvents.UpdateSliders(0,0,vidaMax);//Essas duas funções deveriam ser chamadas
        GameEventsManager.instance.uiEvents.LifeChange(VidaAtual);//             pra stamina e mana tambem
    }

    // Update is called once per frame
    void Update()
    {
        GameEventsManager.instance.uiEvents.UpdateSliders(0,0,vidaMax);//Essas duas funções deveriam ser chamadas
        GameEventsManager.instance.uiEvents.LifeChange(VidaAtual);
    }
    public void TomarDano(float dano){
        VidaAtual -= dano;
        GameEventsManager.instance.uiEvents.LifeChange(VidaAtual);
        if(VidaAtual<=0){
            Morrer();
        }
    }
    public void CurarVida(float vida){
        if(VidaAtual<vidaMax){
            VidaAtual += vida;
            if(VidaAtual>vidaMax)VidaAtual=vidaMax;
            GameEventsManager.instance.uiEvents.LifeChange(VidaAtual);
        }
        
    }
    private void Morrer(){
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
        this.VidaAtual = data.playerStatsData.vidaAtual;
        this.VidaBase = data.playerStatsData.vidaBase;
        UpdateStatusEvent();
    }
    private void UpdateStatusEvent(){
        //GameEventsManager.instance.playerEvents.onStatusChanged
    }

    public void TakeDamage(float damage)
    {
        TomarDano(damage);
    }
}

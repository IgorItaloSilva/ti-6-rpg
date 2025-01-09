using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class StatsUIManager : MonoBehaviour
{
    public static StatsUIManager instance;
    [Header("Stats Base")]
    [SerializeField]TextMeshProUGUI con;
    [SerializeField]TextMeshProUGUI dex;
    [SerializeField]TextMeshProUGUI str;
    [SerializeField]TextMeshProUGUI inte;
    [Header("Exp e Level")]
    [SerializeField]Slider expSlider;
    [SerializeField]TextMeshProUGUI level;
    [SerializeField]TextMeshProUGUI lastLevelExp;
    [SerializeField]TextMeshProUGUI nextLevelExp;
    [Header("Advanced stats")]
    [SerializeField]TextMeshProUGUI lifeInfo;
    [SerializeField]TextMeshProUGUI manaInfo;
    [SerializeField]TextMeshProUGUI ligthAttackDamage;
    [SerializeField]TextMeshProUGUI heavyAtackDamage;
    [SerializeField]TextMeshProUGUI magicAttackDamage;
    [Header("Coisas Level Up")]
    [SerializeField]GameObject levelUpStuff;
    [SerializeField]TextMeshProUGUI pointsToSpend;
    [Header("Cor dos Textos")]
    [SerializeField]Color textColor = Color.white; //ESTÁ AQUI CASO A GENTE QUEIRA MUDAR ELAS DEPOIS
    public bool isSimulating{get;private set;}

    void Awake(){
        if(instance==null){
            instance=this;
        }
        else{
            Debug.Log("Já tinhamos um statsUIManager, então me destrui");
            Destroy(gameObject);
        }
    }
    void OnEnable(){//REMOVIDO PARA USAR SINGLETON
        //GameEventsManager.instance.uiEvents.onReviceBaseStatsInfo+=ReciveBaseStatsInfo;
        //GameEventsManager.instance.uiEvents.onReviceExpStatsInfo+=ReciveExpStatsInfo;
        //GameEventsManager.instance.uiEvents.onReviceAdvancedStatsInfo+=ReciveAdvancedStatsInfo;
        //GameEventsManager.instance.uiEvents.onSimulateChangeBaseValue+=SimulateChangeBaseStat;
        //GameEventsManager.instance.uiEvents.onSimulateChangeAdvancedValue+=SimulateChangeAdvancedValue;
        //GameEventsManager.instance.uiEvents.onReciveLevelUpInfo+=ReciveLevelUpInfo;
    }
    void OnDisable(){//REMOVIDO PARA USAR SINGLETON
        //GameEventsManager.instance.uiEvents.onReviceBaseStatsInfo-=ReciveBaseStatsInfo;
        //GameEventsManager.instance.uiEvents.onReviceExpStatsInfo-=ReciveExpStatsInfo;
        //GameEventsManager.instance.uiEvents.onReviceAdvancedStatsInfo-=ReciveAdvancedStatsInfo;
        //GameEventsManager.instance.uiEvents.onSimulateChangeBaseValue-=SimulateChangeBaseStat;
        //GameEventsManager.instance.uiEvents.onSimulateChangeAdvancedValue-=SimulateChangeAdvancedValue;
        //GameEventsManager.instance.uiEvents.onReciveLevelUpInfo-=ReciveLevelUpInfo;
    }
    void Start()
    {
        UpdateValues();
        //Setup Sliders
    }
    public void UpdateValues(){
        RequestAllStatsInfo();
    }

    public void ReciveBaseStatsInfo(int con,int str,int dex,int inte){
        this.con.text=con.ToString();
        this.str.text=str.ToString();
        this.dex.text=dex.ToString();
        this.inte.text=inte.ToString();
        //Set cores caso elas tenham mudado ao simular elas
        this.con.color = textColor;
        this.str.color = textColor;
        this.dex.color = textColor;
        this.inte.color = textColor;
    }
    public void ReciveExpStatsInfo(int level,float currentExp){
        this.level.text=level.ToString();
        expSlider.minValue = ExpToNextLevel(level-1);
        lastLevelExp.text=expSlider.minValue.ToString("F0");
        expSlider.maxValue = ExpToNextLevel(level);
        nextLevelExp.text=expSlider.maxValue.ToString("F0");
        expSlider.value = currentExp;
    }
    public void ReciveAdvancedStatsInfo(float currentLife,float maxLife,float currentMana, float maxMana,
                                float magicDamage, float ligthAttackDamage,float heavyAtackDamage){
        lifeInfo.text = currentLife.ToString("F0") + "/" + maxLife.ToString("F0");
        manaInfo.text = currentMana.ToString("F0") + "/" + maxMana.ToString("F0");
        magicAttackDamage.text = magicDamage.ToString("F0");
        this.ligthAttackDamage.text = ligthAttackDamage.ToString("F0");
        this.heavyAtackDamage.text = heavyAtackDamage.ToString("F0");
        //Set cores
        lifeInfo.color = textColor;
        manaInfo.color = textColor;
        magicAttackDamage.color = textColor;
        this.ligthAttackDamage.color = textColor;
        this.heavyAtackDamage.color = textColor;
    }
    public void ReciveLevelUpInfo(int pointsToSpend,bool isNearCampfire){
        if(isNearCampfire){
            levelUpStuff.SetActive(true);
            this.pointsToSpend.text = pointsToSpend.ToString();
        }
        else{
            levelUpStuff.SetActive(false);
        }
    }
    void RequestAllStatsInfo(){
        GameEventsManager.instance.uiEvents.RequestBaseStatsInfo();
        GameEventsManager.instance.uiEvents.RequestExpStatsInfo();
        GameEventsManager.instance.uiEvents.RequestAdvancedStatsInfo();
        GameEventsManager.instance.uiEvents.RequestLevelUpInfo();
    }
    int ExpToNextLevel(int level){
        if(level==0){
            return 0;
        }
        return 100*(int)MathF.Pow(2,(level-1));
    }
    //COISAS DO LEVEL UP
    public void SimulateChangeBaseValue(int id,int newValue,bool isDifferent){//Chamado pelo PlayerStats
        Color aux = isDifferent ? Color.green : textColor;
        switch(id){
            case 0:
                this.con.text=newValue.ToString();
                this.con.color = aux;
            break;
            case 1:
                this.dex.text=newValue.ToString();
                this.dex.color = aux;
            break;
            case 2:
                this.str.text=newValue.ToString();
                this.str.color = aux;
            break;
            case 3:
                this.inte.text=newValue.ToString();
                this.inte.color = aux;
            break;
        }
    }
    public void SimulateChangeAdvancedValue(int hardcodedId,float currentLifeOrMana,float value,bool isDifferent){//Chamado pelo PlayerStats
        Color aux = isDifferent ? Color.green : textColor;
        switch(hardcodedId){
            case 0:
                lifeInfo.text = currentLifeOrMana.ToString("F0") + "/" + value.ToString("F0");
                lifeInfo.color = aux;
            break;
            case 1:
                manaInfo.text = currentLifeOrMana.ToString("F0") + "/" + value.ToString("F0");
                manaInfo.color = aux;
            break;
            case 2:
                this.ligthAttackDamage.text = value.ToString("F0");
                this.ligthAttackDamage.color = aux;
            break;
            case 3:
                this.heavyAtackDamage.text = value.ToString("F0");
                this.heavyAtackDamage.color = aux;
            break;
            case 4:
                magicAttackDamage.text = value.ToString("F0");
                magicAttackDamage.color = aux;
            break;
        }
    }
    public void IncreaseStatusButtonPressed(int statusId){//Chamado pelo Botão da UI
        isSimulating=true;
        GameEventsManager.instance.uiEvents.ChangeStatusButtonPressed(statusId,true);
    }
    public void DecreaseStatusButtonPressed(int statusId){//Chamado pelo Botão da UI
        isSimulating=true;
        GameEventsManager.instance.uiEvents.ChangeStatusButtonPressed(statusId,false);
    }
    public void ConfirmLevelUp(){//Chamado pelo Botão da UI
        isSimulating=false;
        GameEventsManager.instance.uiEvents.ConfirmLevelUp();
    }
    public void CancelSimulation(){//Chamado ao sair da tela de Stats da Ui, se ainda estivermos simulando
        isSimulating=false;
        GameEventsManager.instance.uiEvents.DiscardLevelUp();
    }
}

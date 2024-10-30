using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class StatsUIManager : MonoBehaviour
{
    [Header("Stats Base")]
    [SerializeField]TextMeshProUGUI con;
    [SerializeField]TextMeshProUGUI str;
    [SerializeField]TextMeshProUGUI dex;
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

    
    void Start()
    {
        UpdateValues();
        //Setup Sliders
    }
    void OnEnable(){
        GameEventsManager.instance.uiEvents.onReviceBaseStatsInfo+=ReciveBaseStatsInfo;
        GameEventsManager.instance.uiEvents.onReviceExpStatsInfo+=ReciveExpStatsInfo;
        GameEventsManager.instance.uiEvents.onReviceAdvancedStatsInfo+=ReciveAdvancedStatsInfo;
    }
    void OnDisable(){
        GameEventsManager.instance.uiEvents.onReviceBaseStatsInfo-=ReciveBaseStatsInfo;
        GameEventsManager.instance.uiEvents.onReviceExpStatsInfo-=ReciveExpStatsInfo;
        GameEventsManager.instance.uiEvents.onReviceAdvancedStatsInfo-=ReciveAdvancedStatsInfo;
    }
    public void UpdateValues(){
        RequestAllStatsInfo();
    }

    void ReciveBaseStatsInfo(int con,int str,int dex,int inte){
        this.con.text=con.ToString();
        this.str.text=str.ToString();
        this.dex.text=dex.ToString();
        this.inte.text=inte.ToString();
    }
    void ReciveExpStatsInfo(int level,float currentExp){
        this.level.text=level.ToString();
        expSlider.minValue = ExpToNextLevel(level-1);
        lastLevelExp.text=expSlider.minValue.ToString("F0");
        expSlider.maxValue = ExpToNextLevel(level);
        nextLevelExp.text=expSlider.maxValue.ToString("F0");
        expSlider.value = currentExp;
    }
    void ReciveAdvancedStatsInfo(float currentLife,float maxLife,float currentMana, float maxMana,
                                float magicDamage, float ligthAttackDamage,float heavyAtackDamage){
        lifeInfo.text = currentLife.ToString("F0") + "/" + maxLife.ToString("F0");
        manaInfo.text = currentMana.ToString("F0") + "/" + maxMana.ToString("F0");
        magicAttackDamage.text = magicDamage.ToString("F0");
        this.ligthAttackDamage.text = ligthAttackDamage.ToString("F0");
        this.heavyAtackDamage.text = heavyAtackDamage.ToString("F0");
    }
    void RequestAllStatsInfo(){
        GameEventsManager.instance.uiEvents.RequestBaseStatsInfo();
        GameEventsManager.instance.uiEvents.RequestExpStatsInfo();
        GameEventsManager.instance.uiEvents.RequestAdvancedStatsInfo();
    }
    int ExpToNextLevel(int level){
        if(level==0){
            return 0;
        }
        return 100*(int)MathF.Pow(2,(level-1));
    }
}

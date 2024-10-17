using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillTreeMoneyUI : MonoBehaviour
{
    [SerializeField]Enums.PowerUpType powerUpType;
    [SerializeField]String textMoeda;
    [SerializeField]TextMeshProUGUI tmp;
    int money;
    void Start()
    {
        tmp.text = textMoeda + money.ToString();
    }
    void OnEnable(){
        GameEventsManager.instance.uiEvents.onSkillTreeMoneyChange+=AjustText;
    }
    void OnDisable(){
        GameEventsManager.instance.uiEvents.onSkillTreeMoneyChange-=AjustText;
    }
    void AjustText(int index, int quantidade){
        if(index==(int)powerUpType){
            money = quantidade;
            tmp.text = textMoeda + quantidade.ToString();
        }
    }
}

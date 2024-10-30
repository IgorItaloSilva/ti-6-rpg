using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIEvents
{
    
    public event Action<float> onLifeChange;
    public void LifeChange(float vidaAtual){
        if(onLifeChange!=null){
            onLifeChange(vidaAtual);
        }
    }
    
    public event Action<int,float,float> onUpdateSliders;
    public void UpdateSliders(int id,float minValue,float maxValue){
        if(onUpdateSliders!=null){
            onUpdateSliders(id,minValue,maxValue);
        }
    }
    public event Action onSavedGame;
    public void SavedGame(){
        if(onSavedGame!=null){
            onSavedGame();
        }
    }
    public event Action<int,int> onSkillTreeMoneyChange;

    public void SkillTreeMoneyChange(int index,int value){
        if(onSkillTreeMoneyChange!=null){
            onSkillTreeMoneyChange(index,value);
        }
    }
    public event Action<int,int,int,int> onReviceBaseStatsInfo;
    public void ReciveBaseStatsInfo(int con,int str,int dex,int inte){
        if(onReviceBaseStatsInfo!=null){
            onReviceBaseStatsInfo(con,str,dex,inte);
        }
    }
    public event Action<int,float> onReviceExpStatsInfo;
    public void ReciveExpStatsInfo(int level,float currentExp){
        if(onReviceExpStatsInfo!=null){
            onReviceExpStatsInfo(level,currentExp);
        }
    }
    public event Action<float,float,float,float,float,float,float> onReviceAdvancedStatsInfo;
    public void ReciveAdvancedStatsInfo(float currentLife,float maxLife,float currentMana, float maxMana,
                                float magicDamage, float ligthAttackDamage,float heavyAtackDamage){
        if(onReviceAdvancedStatsInfo!=null){
            onReviceAdvancedStatsInfo(currentLife,maxLife,currentMana,maxMana,magicDamage,ligthAttackDamage,heavyAtackDamage);
        }
    }
    public event Action onRequestBaseStatsInfo;
    public void RequestBaseStatsInfo(){
        if(onRequestBaseStatsInfo!=null){
            onRequestBaseStatsInfo();
        }
    }
    public event Action onRequestExpStatsInfo;
    public void RequestExpStatsInfo(){
        if(onRequestExpStatsInfo!=null){
            onRequestExpStatsInfo();
        }
    }
    public event Action onRequestAdvancedStatsInfo;
    public void RequestAdvancedStatsInfo(){
        if(onRequestAdvancedStatsInfo!=null){
            onRequestAdvancedStatsInfo();
        }
    }


}

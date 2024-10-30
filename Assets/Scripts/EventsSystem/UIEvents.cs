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
    public event Action<int,int,int,int,float,float> onStatsDisplay;
    public void StatsDisplay(int con,int str,int dex,int inte, float level,float exp){
        if(onStatsDisplay!=null){
            onStatsDisplay(con,str,dex,inte,level,exp);
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


}

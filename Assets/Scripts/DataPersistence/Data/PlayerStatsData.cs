using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerStatsData
{
    public int con;
    public int str;
    public int dex;
    public int inte;
    public int exp;
    public int level;
    public float currentLife;
    public float baseLife;
    public float currentMana;
    public float baseMana;
    public float baseMagicDamage;
    public float baseLightAttackcDamage;
    public float baseHeavyAttackDamage;

    //ESSE É O CONSTRUTOR DEFALT QUE É CRIADO COM O INICIO DO JOGO
    public PlayerStatsData(PlayerStatsDefaultSO playerStatsDefault){
        
        this.con = playerStatsDefault.baseCon;
        this.str = playerStatsDefault.baseStr;
        this.dex = playerStatsDefault.baseDex;
        this.inte = playerStatsDefault.baseInt;
        this.exp = playerStatsDefault.baseExp;
        this.level = playerStatsDefault.baseLevel;
        this.currentLife = playerStatsDefault.baseLife + playerStatsDefault.baseCon * 25;
        this.baseLife = playerStatsDefault.baseLife;
        this.currentLife = playerStatsDefault.baseMana + playerStatsDefault.baseMana * 10;
        this.baseMana = playerStatsDefault.baseMana;
        this.baseMagicDamage = playerStatsDefault.baseMagicDamage;
        this.baseLightAttackcDamage = playerStatsDefault.baseLightAttackDamage;
        this.baseHeavyAttackDamage = playerStatsDefault.baseHeavyAttackDamage;
    }
    public PlayerStatsData(){
        //Debug.Log("O player não tem um ScriptableObject com os stats dele, usando valores hardcoded");
        con = 1;
        str = 1;
        dex = 1;
        inte = 1;
        exp = 1;
        level = 1;
        currentLife = 1000;
        baseLife = 975;
    }
     public PlayerStatsData(PlayerStats playerStats){//Manter como Adapter
        this.con = playerStats.Con;
        this.str = playerStats.Str;
        this.dex = playerStats.Dex;
        this.inte = playerStats.Int;
        this.exp = playerStats.Exp;
        this.level = playerStats.Level;
        this.currentLife = playerStats.CurrentLife;
        this.baseLife = playerStats.BaseLife;
        this.baseMana = playerStats.BaseMana;
        this.baseMagicDamage = playerStats.BaseMagicDamage;
        this.baseLightAttackcDamage = playerStats.BaseLightAttackDamage;
        this.baseHeavyAttackDamage = playerStats.BaseHeavyAttackDamage;
    }
    public PlayerStatsData(int con, int str,int dex, int inte, int exp, int level, float currentLife,float baseLife){
        this.con = con;
        this.str = str;
        this.dex = dex;
        this.inte = inte;
        this.exp = exp;
        this.level = level;
        this.currentLife = currentLife;
        this.baseLife = baseLife;
    }

}

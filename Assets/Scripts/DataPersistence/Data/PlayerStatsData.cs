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
    public int carriedExp;
    public int level;
    public float currentLife;
    public float baseLife;
    public float currentMana;
    public float baseMana;
    public float baseMagicDamage;
    public float baseLightAttackcDamage;
    public float baseHeavyAttackDamage;
    public bool isNearCampfire;
    public bool playerIsDead;
    public int levelUpPoints;
    public int potionsAmmount;
    public int potionLevel;

    //ESSE É O CONSTRUTOR DEFALT QUE É CRIADO COM O INICIO DO JOGO
    public PlayerStatsData(PlayerStatsDefaultSO playerStatsDefault){
        
        this.con = playerStatsDefault.baseCon;
        this.str = playerStatsDefault.baseStr;
        this.dex = playerStatsDefault.baseDex;
        this.inte = playerStatsDefault.baseInt;
        this.carriedExp = playerStatsDefault.baseExp;
        this.level = playerStatsDefault.baseLevel;
        this.currentLife = playerStatsDefault.baseLife;
        this.baseLife = playerStatsDefault.baseLife;
        this.currentMana = playerStatsDefault.baseMana;
        this.baseMana = playerStatsDefault.baseMana;
        this.baseMagicDamage = playerStatsDefault.baseMagicDamage;
        this.baseLightAttackcDamage = playerStatsDefault.baseLightAttackDamage;
        this.baseHeavyAttackDamage = playerStatsDefault.baseHeavyAttackDamage;
        this.playerIsDead = false;
        this.isNearCampfire=false;
        this.levelUpPoints=0;
        this.potionLevel=1;
        this.potionsAmmount=playerStatsDefault.potionsAmmount;
    }
    public PlayerStatsData(PlayerStats playerStats){//Manter como Adapter
        this.con = playerStats.Con;
        this.str = playerStats.Str;
        this.dex = playerStats.Dex;
        this.inte = playerStats.Int;
        this.carriedExp = playerStats.CarriedExp;
        this.level = playerStats.Level;
        this.currentLife = playerStats.CurrentLife;
        this.baseLife = playerStats.BaseLife;
        this.baseMana = playerStats.BaseMana;
        this.baseMagicDamage = playerStats.BaseMagicDamage;
        this.baseLightAttackcDamage = playerStats.BaseLightAttackDamage;
        this.baseHeavyAttackDamage = playerStats.BaseHeavyAttackDamage;
        this.playerIsDead = playerStats.PlayerIsDead;
        this.isNearCampfire= playerStats.isNearCampfire;
        this.levelUpPoints= playerStats.LevelUpPoints;
        this.potionsAmmount=playerStats.PotionsAmmount;
        this.potionLevel=playerStats.PotionLevel;
    }
    public PlayerStatsData(){
        //Debug.Log("O player não tem um ScriptableObject com os stats dele, usando valores hardcoded");
        con = 1;
        str = 1;
        dex = 1;
        inte = 1;
        carriedExp = 1;
        level = 1;
        currentLife = 1000;
        baseLife = 975;
        this.playerIsDead = false;
        this.isNearCampfire=false;
        this.levelUpPoints=0;
    }

}

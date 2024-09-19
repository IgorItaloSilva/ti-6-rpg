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
    public float exp;
    public float level;
    public float vidaAtual;
    public float vidaBase;

    //ESSE É O CONSTRUTOR DEFALT QUE É CRIADO COM O INICIO DO JOGO
    public PlayerStatsData(PlayerStatsDefaultSO playerStatsDefault){
        
        this.con = playerStatsDefault.conBase;
        this.str = playerStatsDefault.strBase;
        this.dex = playerStatsDefault.dexBase;
        this.inte = playerStatsDefault.IntBase;
        this.exp = playerStatsDefault.expBase;
        this.level = playerStatsDefault.levelBase;
        this.vidaAtual = playerStatsDefault.vidaBase + playerStatsDefault.conBase * 25;
        this.vidaBase = playerStatsDefault.vidaBase;
    }
    public PlayerStatsData(){
        //Debug.Log("O player não tem um ScriptableObject com os stats dele, usando valores hardcoded");
        con = 1;
        str = 1;
        dex = 1;
        inte = 1;
        exp = 1;
        level = 1;
        vidaAtual = 1000;
        vidaBase = 975;
    }
     public PlayerStatsData(PlayerStats playerStats){//Manter como Adapter
        this.con = playerStats.Con;
        this.str = playerStats.Str;
        this.dex = playerStats.Dex;
        this.inte = playerStats.Int;
        this.exp = playerStats.Exp;
        this.level = playerStats.Level;
        this.vidaAtual = playerStats.VidaAtual;
        this.vidaBase = playerStats.VidaBase;
    }
    public PlayerStatsData(int con, int str,int dex, int inte, float exp, float level, float vidaAtual,float vidaBase){
        this.con = con;
        this.str = str;
        this.dex = dex;
        this.inte = inte;
        this.exp = exp;
        this.level = level;
        this.vidaAtual = vidaAtual;
        this.vidaBase = vidaBase;
    }

}

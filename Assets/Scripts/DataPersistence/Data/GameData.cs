using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

[Serializable]
public class GameData {
    public long lastUpdated;
    public Vector3 pos;
    public PlayerStatsData playerStatsData;

    //esses valores são os valores iniciais pra quando a gente começar o jogo.
    public GameData(PlayerStatsDefaultSO playerStatsDefaultSO){
        //Debug.Log("Feita a versao com o SO");
        pos = new Vector3(0,0,0);
        playerStatsData = new PlayerStatsData(playerStatsDefaultSO);
    }
    public GameData(){
        //Debug.Log("Feita a versao sem o SO");
        pos = new Vector3(0,0,0);
        playerStatsData = new PlayerStatsData();
    }

}

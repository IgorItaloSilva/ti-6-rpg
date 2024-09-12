using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

[Serializable]
public class GameData {
    public long lastUpdated;
    public Vector3 pos;

    //esses valores são os valores iniciais pra quando a gente começar o jogo.
    public GameData(){
        pos = new Vector3(0,0,0);
    }
}

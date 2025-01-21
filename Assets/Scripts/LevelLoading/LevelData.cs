using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData{

    public SerializableDictionary<string,EnemyData>enemiesData;
    public SerializableDictionary<string,InteractableData>interactablesData;

    public LevelData(){
        enemiesData=new SerializableDictionary<string,EnemyData>();
        interactablesData=new SerializableDictionary<string,InteractableData>();
    }
}


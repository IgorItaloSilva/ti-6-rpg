using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

[Serializable]
public class GameData
{
    public long lastUpdated;
    public Vector3 pos;
    public PlayerStatsData playerStatsData;
    public SkillTreeData skillTreeData;
    public SerializableDictionary<string,LevelData> levelsData;
    public RuneData runeData;
    public string currentLevel;
    public DroppedExpData droppedExpData;

    //esses valores são os valores iniciais pra quando a gente começar o jogo.
    public GameData(PlayerStatsDefaultSO playerStatsDefaultSO)
    {
        //Debug.Log("Feita a versao com o SO");
        pos = new Vector3(0f, 1f, 0f);
        currentLevel = "";
        playerStatsData = new PlayerStatsData(playerStatsDefaultSO);
        skillTreeData = new SkillTreeData();
        levelsData = new SerializableDictionary<string,LevelData>();
        droppedExpData = new DroppedExpData();
        runeData=new RuneData();
    }

    public GameData()
    {
        //Debug.Log("Feita a versao sem o SO");
        pos = new Vector3(0f, 1f, 0f);
        currentLevel = "";
        playerStatsData = new PlayerStatsData();
        skillTreeData = new SkillTreeData();
        levelsData = new SerializableDictionary<string,LevelData>();
        runeData=new RuneData();
    }
}
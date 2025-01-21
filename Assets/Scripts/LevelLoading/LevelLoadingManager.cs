using System.Collections;
using System.Collections.Generic;
using log4net.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class LevelLoadingManager : MonoBehaviour,IDataPersistence
{
    public static LevelLoadingManager instance;
    public List<ActualEnemyController>enemies;
    public List<Interactable>interactables;
    public LevelData CurrentLevelData;
    public Vector3 respawnPoint;
    public string LevelName {get;private set;}
    bool loadSucceded;
    void Awake(){//singleton, só que queremos só o mais recente
        if(instance!=null){
            Destroy(instance.gameObject);
        }
        instance=this;
        CurrentLevelData = new LevelData();
        enemies=new List<ActualEnemyController>();
        interactables=new List<Interactable>();
        LevelName = SceneManager.GetActiveScene().name;
    }
    void Start()
    {
        DataPersistenceManager.instance.SaveLevelName();
        if(!loadSucceded){
            //no data to be loaded
            CurrentLevelData = new LevelData();
        }
        else{
            foreach(ActualEnemyController enemy in enemies){
                EnemyData enemieData;
                if(CurrentLevelData.enemiesData.TryGetValue(enemy.Id,out enemieData)){
                    enemy.Load(enemieData);
                }
            }
            foreach(Interactable interactable in interactables){
                InteractableData interactableData;
                if(CurrentLevelData.interactablesData.TryGetValue(interactable.Id,out interactableData)){
                    interactable.Load(interactableData);
                }
            }
        }
    }
    public void LoadData(GameData gameData)
    {
        loadSucceded= gameData.levelsData.TryGetValue(LevelName,out CurrentLevelData);
    }

    public void SaveData(GameData gameData)
    {
        foreach(ActualEnemyController enemy in enemies){
            if(enemy.gameObject.activeInHierarchy){
                enemy.Save();
            }
        }
        if(gameData.levelsData.ContainsKey(LevelName)){
            gameData.levelsData[LevelName]=CurrentLevelData;
        }
        else{
            gameData.levelsData.Add(LevelName,CurrentLevelData);
        }
    }
    public void RespawnEnemies(){
        foreach(ActualEnemyController enemy in enemies){
            if(!enemy.gameObject.activeInHierarchy){
                enemy.gameObject.SetActive(true);
            }
            enemy.Respawn();
            enemy.Save();
        }
    }
}

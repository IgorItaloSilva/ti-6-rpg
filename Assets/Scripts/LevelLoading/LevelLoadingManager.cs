using System.Collections.Generic;
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
    void Awake(){
        if(instance!=null){
            Destroy(gameObject);
        }
        Debug.Log("Estou no awake do level loading manager");
        instance=this;
        CurrentLevelData = new LevelData();
        enemies=new List<ActualEnemyController>();
        interactables=new List<Interactable>();
        LevelName = SceneManager.GetActiveScene().name;
        Debug.Log(CurrentLevelData);
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
                if(CurrentLevelData.interactablesData.TryGetValue(interactable.saveId,out interactableData)){
                    interactable.Load(interactableData);
                }
            }
        }
    }
    public void LoadData(GameData gameData)
    {
        LevelData auxLevelData;
        loadSucceded= gameData.levelsData.TryGetValue(LevelName,out auxLevelData);
        if(loadSucceded)CurrentLevelData=auxLevelData;
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

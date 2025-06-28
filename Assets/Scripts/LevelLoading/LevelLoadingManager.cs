using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class LevelLoadingManager : MonoBehaviour, IDataPersistence
{
    public static LevelLoadingManager instance;
    public List<ActualEnemyController> enemies;
    public List<EnemyBehaviour> enemiesIgor;
    public List<Interactable> interactables;
    public LevelData CurrentLevelData;
    public Vector3 respawnPoint;
    public Vector3 lastUsedRespawnPos;
    public bool hasOtherSpawnPos;
    public bool showDebug;
    public string LevelName { get; private set; }
    bool loadSucceded;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        if (showDebug) Debug.Log("Estou no awake do level loading manager");
        instance = this;
        CurrentLevelData = new LevelData();
        enemies = new List<ActualEnemyController>();
        interactables = new List<Interactable>();
        int countLoaded = SceneManager.sceneCount;
        Scene[] loadedScenes = new Scene[countLoaded];
        for (int i = 0; i < countLoaded; i++)
        {
            loadedScenes[i] = SceneManager.GetSceneAt(i);
            if (showDebug) Debug.Log(loadedScenes[i].name);
            if (loadedScenes[i].name != "Hud") LevelName = loadedScenes[i].name;
        }
        if (showDebug) Debug.Log(LevelName);
    }
    void Start()
    {
        DataPersistenceManager.instance.SetLevelName(LevelName);
        if (!loadSucceded)
        {
            //no data to be loaded
            CurrentLevelData = new LevelData();
        }
        else
        {
            if (enemies != null)
                foreach (ActualEnemyController enemy in enemies)
                {
                    EnemyData enemieData;
                    if (CurrentLevelData.enemiesData.TryGetValue(enemy.SaveId, out enemieData))
                    {
                        enemy.Load(enemieData);
                    }
                }
            if (enemiesIgor != null)
                foreach (EnemyBehaviour enemy in enemiesIgor)
                {
                    EnemyData enemyData;
                    if (CurrentLevelData.enemiesData.TryGetValue(enemy.SaveId, out enemyData))
                    {
                        enemy.Load(enemyData);
                    }
                }
            if (interactables != null)
                foreach (Interactable interactable in interactables)
                {
                    InteractableData interactableData;
                    if (CurrentLevelData.interactablesData.TryGetValue(interactable.saveId, out interactableData))
                    {
                        interactable.Load(interactableData);
                    }
                }
        }
    }
    public void LoadData(GameData gameData)
    {
        LevelData auxLevelData;
        loadSucceded = gameData.levelsData.TryGetValue(LevelName, out auxLevelData);
        if (loadSucceded)
        {
            CurrentLevelData = auxLevelData;
            lastUsedRespawnPos = CurrentLevelData.lastUsedRespawnPos;
            hasOtherSpawnPos = CurrentLevelData.hasOtherSpawnPos;
        }
    }

    public void SaveData(GameData gameData)
    {
        foreach (ActualEnemyController enemy in enemies)
        {
            if (enemy.gameObject.activeInHierarchy)
            {
                enemy.Save();
            }
        }
        foreach (EnemyBehaviour enemy in enemiesIgor)
        {
            if (enemy.gameObject.activeInHierarchy)
            {
                enemy.Save();
            }
        }
        CurrentLevelData.hasOtherSpawnPos = hasOtherSpawnPos;
        CurrentLevelData.lastUsedRespawnPos = lastUsedRespawnPos;
        if (gameData.levelsData.ContainsKey(LevelName))
        {
            gameData.levelsData[LevelName] = CurrentLevelData;
        }
        else
        {
            gameData.levelsData.Add(LevelName, CurrentLevelData);
        }
    }
    public void RespawnEnemies()
    {
        foreach (ActualEnemyController enemy in enemies)
        {
            if (!enemy.gameObject.activeInHierarchy)
            {
                enemy.gameObject.SetActive(true);
            }
            enemy.Respawn();
            enemy.Save();
        }
        foreach (EnemyBehaviour enemyBehaviour in enemiesIgor)
        {
            if (!enemyBehaviour.gameObject.activeInHierarchy)
            {
                enemyBehaviour.gameObject.SetActive(true);
            }
            enemyBehaviour.Respawn();
            enemyBehaviour.Save();
        }
    }
    public void SetNewSpawn(Vector3 newSpawn)
    {
        lastUsedRespawnPos = newSpawn + Vector3.forward;
        hasOtherSpawnPos = true;
    }
    public Vector3 RespawnPos()
    {
        if (hasOtherSpawnPos)
            return lastUsedRespawnPos;
        else
            return respawnPoint;
    }
}

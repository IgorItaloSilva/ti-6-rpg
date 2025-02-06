using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField]private bool disableDataPersistence = false;
    [SerializeField] private bool initializeDataIfNull= false;
    [SerializeField] private bool overrideSelectedProfileId = false;
    [SerializeField] private string testSelectedProfileId = "test";
    [Header("File Storage Config")]
    [SerializeField]private string fileName;
    [SerializeField]private bool useEncryption;
    [Header("Scriptable Objects Data")]
    [SerializeField]private PlayerStatsDefaultSO playerStatsDefaultSO;
    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    private string selectedProfileId = "";
    public static DataPersistenceManager instance {get; private set;}
    void Awake(){
        if(instance!=null){
            //if(DebugManager.debugManager.DEBUG){
                Debug.Log("Já temos um DataPersistenceManager, então me destrui");
            //}
            Destroy(this.gameObject);
            return;
        }
        instance=this;
        DontDestroyOnLoad(this.gameObject);
        if(disableDataPersistence){
            Debug.LogWarning("Salvamento está desativado!!");
        }
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName,useEncryption);
        InitializeSelectedProfileId();
    }
    public void NewGame(){
        Debug.LogWarning("Foi criada uma nova gameData");
        if(playerStatsDefaultSO==null){
            this.gameData = new GameData();
        }
        else{
            //Debug.Log("o data manager tem um SO");
            this.gameData = new GameData(playerStatsDefaultSO);
        }
    }
    public void LoadGame(){
        if(disableDataPersistence){
            return;
        }
        this.gameData = dataHandler.Load(selectedProfileId);
        //if no data can be found initialize new game
        if(this.gameData==null && initializeDataIfNull == true){
            NewGame();
        }
        if(this.gameData==null){
            Debug.LogWarning("No data was found. We must create a new save first");
            return;
        }
        //TODO- push the loaded data to all scripts that need it
        foreach (IDataPersistence dataPersistenceObj in  dataPersistenceObjects){
            dataPersistenceObj.LoadData(gameData);
        }
    }
    public void SaveGame(){
        if(disableDataPersistence){
            return;
        }
        if(this.gameData==null){
            Debug.LogWarning("No data was Found, must start a new save before saving");
            return;
        }
        Debug.LogWarning("We are saving the game!");
        GameEventsManager.instance?.uiEvents.SavedGame();
        //pass data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in  dataPersistenceObjects){
            dataPersistenceObj.SaveData(gameData);
        }
        //timestamp the data so we know when is was last saved
        gameData.lastUpdated = System.DateTime.Now.ToBinary();
        //save current scene name
        Scene scene = SceneManager.GetActiveScene();
        if(scene.name !="MainMenu")gameData.currentLevel=scene.name;

        //save data into a file using the data handler
        dataHandler.Save(gameData,selectedProfileId);
    }
    void OnApplicationQuit(){
        //SaveGame();
    }
    private List<IDataPersistence> FindAllDataPersistenceObjects(){
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }
    void OnEnable(){
        SceneManager.sceneLoaded +=OnSceneLoaded;
    }
    void OnDisable(){
        SceneManager.sceneLoaded -=OnSceneLoaded;
    }

    public bool HasData(){
        Debug.Log(gameData);
        return gameData != null;
    }
    public Dictionary<string, GameData> GetAllProfilesGameData(){
        return dataHandler.LoadAllProfiles();
    }
    public void ChangeSelectedProfileId(string newProfileId){
        this.selectedProfileId=newProfileId;
        LoadGame();
    }
    public void Delete(string profileId){
        dataHandler.Delete(profileId);
        InitializeSelectedProfileId();
        LoadGame();
    }
    void InitializeSelectedProfileId(){
        this.selectedProfileId = dataHandler.GetMostRecentProfileId();
        if(overrideSelectedProfileId){
            this.selectedProfileId=testSelectedProfileId;
            Debug.LogWarning("Demos override no profile que será loadado pelo profile: "+ testSelectedProfileId);
        }
    }
    public void SaveLevelName(){
        //save current scene name
        Scene scene = SceneManager.GetActiveScene();
        if(scene.name !="MainMenu")gameData.currentLevel=scene.name;
    }
}

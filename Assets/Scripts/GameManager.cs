using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {get;private set;}
    [SerializeField]bool shouldOpenHud;
    [SerializeField] GameObject canvasChangingScene;
    [SerializeField]Image backgroundImage;
    [SerializeField] Sprite[] loadingImages;
    [SerializeField] Slider sliderLoadScene;
    [SerializeField] GameObject textoAperteQualquerTecla;
    [SerializeField] bool skipWaitForKeyPressToLoad;
    public bool cheatsEnabled;
    
    public bool shouldLoadTutorial;
    public bool shouldShowTutorials = true;
    public bool showDebug;
    [HideInInspector] public AudioManager audioManager;//negocio do igor
    void OnEnable(){
        GameEventsManager.instance.playerEvents.onPlayerDied+=PlayerDied;
    }
    void OnDisable(){
        GameEventsManager.instance.playerEvents.onPlayerDied-=PlayerDied;
    }
    void Awake(){
        if(!instance){
            instance=this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            if(showDebug)Debug.Log("Temos 2 gameManagers, estou me destruindo");
            Destroy(gameObject);  
        }
    }
    void Start(){
        if (canvasChangingScene == null)
        {
            canvasChangingScene = GetComponentInChildren<Canvas>().gameObject;
        }
        canvasChangingScene.SetActive(false);
        #if UNITY_EDITOR
        if (shouldOpenHud)
        {
            SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
        }
        #endif
    }
    /* void Update(){
        if(Keyboard.current.hKey.wasPressedThisFrame){
            if(hudIsOpen==false){
                SceneManager.LoadScene("Hud",LoadSceneMode.Additive);
                hudIsOpen=true;
            }
        }
    } */
    public void PauseGameAndUnlockCursor(){
        Time.timeScale=0f;
        Cursor.lockState=CursorLockMode.Confined;
        Cursor.visible=true;
        GameEventsManager.instance.uiEvents.PauseGame();
    }
    public void UnpauseGameAndLockCursor(){
        Time.timeScale=1f;
        Cursor.lockState=CursorLockMode.Locked;
        Cursor.visible=false;
        GameEventsManager.instance.uiEvents.UnpauseGame();
    }
    private void PlayerDied(){
        //A ui vai pausar o jogo apos o vfx
        //PauseInputs
    }
    public void ExitGame(bool shouldSave){
        if(shouldSave){
            DataPersistenceManager.instance.SaveGame();
        }
        Application.Quit();
    }
    public void ReturnFromDeath(){
        if(showDebug)Debug.Log("game manager chamou o respawn");
        DataPersistenceManager.instance.LoadGame();
        GameEventsManager.instance.playerEvents.PlayerRespawn();
        UnpauseGameAndLockCursor();
    }
    public void ChangeLevel(string levelName,Vector3 posToArrive){
        string currentLevel = LevelLoadingManager.instance.LevelName;
        GameEventsManager.instance.playerEvents.SetPosition(posToArrive);
        DataPersistenceManager.instance.SaveGame();
        StartCoroutine(UnloadLevelAsync(currentLevel));
        StartCoroutine(LoadLevelAsync(levelName));
    }
    public void ChangeLevelFromMainMenu(string levelName, Vector3 posToArrive)
    {
        GameEventsManager.instance.playerEvents.SetPosition(posToArrive);
        DataPersistenceManager.instance.SaveGame();
        StartCoroutine(LoadLevelAsync(levelName,false));
    }
    IEnumerator LoadLevelAsync(string levelName,bool isAdditive = true)
    {
        AsyncOperation loadOperation;
        if (!isAdditive)loadOperation = SceneManager.LoadSceneAsync(levelName);
        else loadOperation = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        loadOperation.allowSceneActivation = false;
        canvasChangingScene.SetActive(true);
        textoAperteQualquerTecla.gameObject.SetActive(false);
        int code = levelName == "MageMap" ? 1 : 0;
        backgroundImage.sprite = loadingImages[code];
        while (!loadOperation.isDone)
        {
            sliderLoadScene.value = loadOperation.progress;
            if (loadOperation.progress >= 0.9f)
            {
                textoAperteQualquerTecla.gameObject.SetActive(true);
                if (Keyboard.current.anyKey.wasPressedThisFrame||skipWaitForKeyPressToLoad)
                {
                    loadOperation.allowSceneActivation = true;
                }
            }
            yield return null;
        }
        canvasChangingScene.SetActive(false);
    }
    IEnumerator UnloadLevelAsync(string levelName){
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(levelName);
        //Pausar coisas que estão atrapalhando
        while(!unloadOperation.isDone){
            yield return null;
        }
        //Despausar
    }
}
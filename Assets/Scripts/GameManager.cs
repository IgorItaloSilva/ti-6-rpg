using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {get;private set;}
    [SerializeField]bool shouldOpenHud;
    public bool shouldLoadTutorial;
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
        #if UNITY_EDITOR
            if(shouldOpenHud){
                SceneManager.LoadScene("Hud",LoadSceneMode.Additive);
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
        //Ligar a tela de carregamento, se quiser
        StartCoroutine(UnloadLevelAsync(currentLevel));
        StartCoroutine(LoadLevelAsync(levelName));
    }
    IEnumerator LoadLevelAsync(string levelName){
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelName,LoadSceneMode.Additive);
        while(!loadOperation.isDone){
            //float progressValue = mathf.Clamp01(loadOperation.progress/0.9f);
            //atualizar o slider de progresso
            yield return null;
        }
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
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {get;private set;}
    [HideInInspector] public AudioManager audioManager;//negocio do igor

    void OnEnable(){
        GameEventsManager.instance.playerEvents.onPlayerDied+=PlayerDied;
    }
    void OnDisable(){
        GameEventsManager.instance.playerEvents.onPlayerDied-=PlayerDied;
    }
    void Start(){
        if(!instance){
            instance=this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Debug.Log("Temos 2 gameManagers, estou me destruindo");
            Destroy(gameObject);
            
        }
    }
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
        Debug.Log("game manager chamou o respawn");
        DataPersistenceManager.instance.LoadGame();
        GameEventsManager.instance.playerEvents.PlayerRespawn();
        UnpauseGameAndLockCursor();
    }
}
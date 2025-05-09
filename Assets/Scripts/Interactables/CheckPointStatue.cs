using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CheckPointStatue : Interactable
{
    [SerializeField]TextMeshPro keyIndicationText;
    [SerializeField]bool isDefaultSpawnPoint;
    PlayerStats playerStats;
    bool didntAddInteract = false;
    protected override void Awake()
    {
        ignoreSaveLoad=true;
    }
    void OnEnable(){
        if(!PlayerStateMachine.Instance)didntAddInteract=true;
        PlayerStateMachine.Instance?.AddActionToInteract(Interact);
    }
    void OnDisable(){
        PlayerStateMachine.Instance?.RemoveActionFromInteract(Interact);
    }
    protected override void Start()
    {
        base.Start();
        if(didntAddInteract){
            PlayerStateMachine.Instance?.AddActionToInteract(Interact);
        }
        /* playerInput = new PlayerInput(); REMOVIDO POIS EU CRIEI O PlayerStateMachine.Instance.AddFunctionToInteract
        playerInput.Gameplay.Enable();
        playerInput.Gameplay.Interact.performed+=Interact; */
        if(isDefaultSpawnPoint){
            if(LevelLoadingManager.instance==null){
                Debug.LogWarning("Não temos um loading manager para definir a posição de spawn");
            }
            else{
                LevelLoadingManager.instance.respawnPoint=transform.position;
            }
        }
    }
    protected override void OnTriggerEnter(Collider collider)
    {
        myEvent.Invoke();
        //Debug.Log("Entrei na area da status de save");
        if(collider.CompareTag("Player")){
            //Debug.Log("Um jogadore ntrou aqui");
            keyIndicationText.gameObject.SetActive(true);
            inRange=true;
            playerStats=collider.GetComponent<PlayerStats>();
            if(playerStats!=null){
                playerStats.isNearCampfire=true;
            }
            UIManager.instance.NearCampfire(true);
        }
    }
    void OnTriggerExit(Collider collider){
        //Debug.Log("Sai da statue de save");
        if(collider.CompareTag("Player")){
                        Debug.Log("Um jogadore saiu aqui");
            inRange = false;
            playerStats=collider.GetComponent<PlayerStats>();
            if(playerStats!=null){
                playerStats.isNearCampfire=false;
            }
            keyIndicationText.gameObject.SetActive(false);
            UIManager.instance.NearCampfire(false);
        }
    }
    void OnTriggerStay(Collider collider){
        if(collider.CompareTag("Player")){
            Vector3 lookAt = new Vector3(collider.transform.position.x,keyIndicationText.transform.position.y,collider.transform.position.z);
            keyIndicationText.transform.LookAt(lookAt);
            keyIndicationText.transform.Rotate(new Vector3(0,180,0));
        }
    }
    void Interact(InputAction.CallbackContext context){
        if(inRange && !PlayerStateMachine.Instance.IsLocked){
            PlayerStateMachine.Instance.Animator.ResetTrigger(PlayerStateMachine.Instance.HasPrayedHash);
            PlayerStateMachine.Instance.Animator.SetTrigger(PlayerStateMachine.Instance.HasPrayedHash);
            PlayerStateMachine.Instance.Animator.SetFloat(PlayerStateMachine.Instance.PlayerVelocityXHash, 0f);
            PlayerStateMachine.Instance.Animator.SetFloat(PlayerStateMachine.Instance.PlayerVelocityYHash, 0f);
            PlayerStateMachine.Instance.LockPlayer();
            LevelLoadingManager.instance?.RespawnEnemies();
            playerStats?.CheckPointStatue();
            DataPersistenceManager.instance.SaveGame();
            UIManager.instance.SwitchToScreen((int)UIManager.UIScreens.MainPause);
            UIManager.instance.SwitchToScreen((int)UIManager.UIScreens.Stats);
        }
    }
}

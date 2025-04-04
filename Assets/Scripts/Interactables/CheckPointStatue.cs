using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CheckPointStatue : Interactable
{
    [SerializeField]TextMeshPro keyIndicationText;
    [SerializeField]bool isDefaultSpawnPoint;
    PlayerStats playerStats;
    protected override void Awake()
    {
        ignoreSaveLoad=true;
    }
    void OnEnable(){
        PlayerStateMachine.Instance?.AddActionToInteract(Interact);
    }
    void OnDisable(){
        PlayerStateMachine.Instance?.RemoveActionFromInteract(Interact);
    }
    protected override void Start()
    {
        base.Start();
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
        //Debug.Log("Entrei na area da status de save");
        if(collider.CompareTag("Player")){
            keyIndicationText.gameObject.SetActive(true);
            inRange=true;
            playerStats=collider.GetComponent<PlayerStats>();
            if(playerStats!=null){
                playerStats.isNearCampfire=true;
            }
        }
    }
    void OnTriggerExit(Collider collider){
        //Debug.Log("Sai da statue de save");
        if(collider.CompareTag("Player")){
            inRange = false;
            playerStats=collider.GetComponent<PlayerStats>();
            if(playerStats!=null){
                playerStats.isNearCampfire=false;
            }
            keyIndicationText.gameObject.SetActive(false);
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
        if(inRange){
            PlayerStateMachine.Instance.Animator.ResetTrigger(PlayerStateMachine.Instance.HasSavedHash);
            PlayerStateMachine.Instance.Animator.SetTrigger(PlayerStateMachine.Instance.HasSavedHash);
            PlayerStateMachine.Instance.LockPlayer();
            LevelLoadingManager.instance?.RespawnEnemies();
            playerStats?.CheckPointStatue();
            DataPersistenceManager.instance.SaveGame();
        }
    }
}

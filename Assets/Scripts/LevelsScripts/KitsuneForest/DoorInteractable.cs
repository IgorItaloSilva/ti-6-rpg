using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorInteractable : Interactable
{
    [SerializeField]GameObject closedDoor;
    [SerializeField]GameObject openDoor;
    [SerializeField]GameObject keyIndicationCanvas;
    [SerializeField]GameObject otherSideDialogInteractable;
    bool didntAddInteract;
    //PlayerInput playerInput;
    void OnEnable()
    {
        PlayerStateMachine.Instance?.AddActionToInteract(Interact);
        if (!PlayerStateMachine.Instance) didntAddInteract = true;
    }
    protected override void Awake()
    {
        base.Awake();
        if (keyIndicationCanvas == null)
        {
            keyIndicationCanvas = GetComponentInChildren<Canvas>().gameObject;
        }
    }
    protected override void Start()
    {
        if (didntAddInteract)
        {
            PlayerStateMachine.Instance?.AddActionToInteract(Interact);
        }
    }
    void OnDisable(){
        PlayerStateMachine.Instance.RemoveActionFromInteract(Interact);
    }
    protected override void OnTriggerEnter(Collider collider)
    {
        //Debug.Log("Entrei numa porta");
        if(collider.CompareTag("Player")){
            if(AlreadyInterated)return;
            keyIndicationCanvas.SetActive(true);
            inRange=true;
        }
    }
    void OnTriggerExit(Collider collider){
        //Debug.Log("Sai duma porta");
        if(collider.CompareTag("Player")){
            inRange = false;
            keyIndicationCanvas.SetActive(false);
        }
    }
    void Interact(InputAction.CallbackContext context){
        if(!AlreadyInterated){
            if(inRange){
                OpenDoor();
            }
        }
    }
    void OpenDoor()
    {
        closedDoor.SetActive(false);
        openDoor.SetActive(true);
        AlreadyInterated = true;
        otherSideDialogInteractable.SetActive(false);
        Save();
        gameObject.SetActive(false);
    }
    public override void Load(InteractableData interactableData)
    {
        base.Load(interactableData);
        if(AlreadyInterated)OpenDoor();
    }

}

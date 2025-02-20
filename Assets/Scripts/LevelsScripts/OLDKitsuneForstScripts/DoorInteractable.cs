using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorInteractable : Interactable
{
    [SerializeField]GameObject closedDoor;
    [SerializeField]GameObject openDoor;
    [SerializeField]TextMeshPro wrongSideText;
    [SerializeField]TextMeshPro keyIndicationText;
    bool inRange;
    PlayerInput playerInput;
    protected override void Start()
    {
        base.Start();
        playerInput = new PlayerInput();
        playerInput.Gameplay.Enable();
        playerInput.Gameplay.Interact.performed+=Interact;
    }
    protected override void OnTriggerEnter(Collider collider)
    {
        //Debug.Log("Entrei numa porta");
        if(collider.CompareTag("Player")){
            if(AlreadyInterated)return;
            if(collider.transform.position.x>transform.position.x){//Lado onde não abre
                wrongSideText.gameObject.SetActive(true);
            }
            else{
                keyIndicationText.gameObject.SetActive(true);
                inRange=true;
            }
        }
    }
    void OnTriggerExit(Collider collider){
        //Debug.Log("Sai duma porta");
        if(collider.CompareTag("Player")){
            inRange = false;
            keyIndicationText.gameObject.SetActive(false);
            wrongSideText.gameObject.SetActive(false);
        }
    }
    void Interact(InputAction.CallbackContext context){
        if(!AlreadyInterated){
            if(inRange){
                OpenDoor();
            }
        }
    }
    void OpenDoor(){
        closedDoor.SetActive(false);
        openDoor.SetActive(true);
        AlreadyInterated=true;
        Save();
    }
    public override void Load(InteractableData interactableData)
    {
        base.Load(interactableData);
        if(AlreadyInterated)OpenDoor();
    }

}

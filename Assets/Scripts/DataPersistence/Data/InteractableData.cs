using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InteractableData{
    public bool canInteract;
    public bool alreadyInterated;
    public bool active;
    public InteractableData(){
        canInteract=true;
        alreadyInterated=false;
        active = true;
    }
    public InteractableData(Interactable interactable){
        canInteract=interactable.CanInteract;
        alreadyInterated=interactable.AlreadyInterated;
        active=interactable.Active;
    }
}

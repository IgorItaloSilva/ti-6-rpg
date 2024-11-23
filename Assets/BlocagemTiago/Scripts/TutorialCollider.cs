using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCollider : ColliderActivateEvent
{
    bool alreadyApeared;
    [SerializeField]string tutorialTitle;
    protected override void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player")){
            if(!alreadyApeared){
                GameEventsManager.instance.uiEvents.OpenTutorial(tutorialTitle);
                alreadyApeared=true;
            }
        }
    }

    
}

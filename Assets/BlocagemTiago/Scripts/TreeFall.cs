using Unity.VisualScripting;
using UnityEngine;

public class TreeFall : ColliderActivateEvent
{
    bool isActive;
    void OnEnable(){
        GameEventsManager.instance.mapaTiagoEvents.onCutScenePlayed+=Activate;
    }
    void OnDisable(){
        GameEventsManager.instance.mapaTiagoEvents.onCutScenePlayed-=Activate;
    }
    protected override void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player")){
            if(isActive){
                myEvent.Invoke();
                GameEventsManager.instance.uiEvents.OpenTutorial();
                isActive=false;
            }
        }
    }
    void Activate(){
        isActive=true;
    }
}

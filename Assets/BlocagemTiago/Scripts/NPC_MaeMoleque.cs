using UnityEngine;

public class NPC_MaeMoleque : ColliderActivateEvent
{
    bool alreadyGaveQuest;
    void OnEnable(){
        GameEventsManager.instance.mapaTiagoEvents.onCutScenePlayed+=Die;
    }
    void OnDisable(){
        GameEventsManager.instance.mapaTiagoEvents.onCutScenePlayed-=Die;
    }
    void Die(){
        transform.position = new Vector3(transform.position.x,0.2f,transform.position.z);
        transform.rotation = new Quaternion(-0.659181058f,0.255891263f,-0.255891263f,0.659181058f);
    }

    protected override void OnTriggerEnter(Collider collider)
    {
        if(!alreadyGaveQuest){
            GameEventsManager.instance.uiEvents.DialogOpen();
            alreadyGaveQuest = true;
            GameEventsManager.instance.mapaTiagoEvents.DeactivateQuest(0);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarInteractabe : Interactable
{
    [SerializeField]int spawnId;
    [SerializeField]GameObject effect;
    void OnEnable(){
        GameEventsManager.instance.levelEvents.onKitsuneDeath+=KitsuneDied;
    }
    void OnDisable(){
        GameEventsManager.instance.levelEvents.onKitsuneDeath-=KitsuneDied;
    }
    override protected void Start()
    {
        base.Start();
        if(effect!=null)effect.SetActive(false);
    }
    void KitsuneDied(int kitsuneId){
        if(kitsuneId==spawnId&&!AlreadyInterated){
            ActivatePillar();
        }
    }
    
    void ActivatePillar(){
        GameEventsManager.instance.levelEvents.PillarActivated();
        AlreadyInterated = true;
        if(effect!=null)effect.SetActive(true);
        Save();
    }
    
    public override void Load(InteractableData interactableData)
    {
        base.Load(interactableData);
        if(AlreadyInterated){
            ActivatePillar();
        }
    }
}

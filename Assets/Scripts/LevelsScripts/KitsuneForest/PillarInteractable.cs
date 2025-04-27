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
        Debug.Log("Rodei o start do pilar");
        base.Start();
        if(effect!=null&&!AlreadyInterated)effect.SetActive(false);
    }
    void KitsuneDied(int kitsuneId){
        if(kitsuneId==spawnId&&!AlreadyInterated){
            ActivatePillar();
        }
    }
    
    void ActivatePillar(){
        Debug.Log("Entrei no activate pilar");
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

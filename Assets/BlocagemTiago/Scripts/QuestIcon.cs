using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestIcon : MonoBehaviour
{
    [SerializeField]int QuestId;
    Transform iconParent;
    void OnEnable(){
        GameEventsManager.instance.mapaTiagoEvents.OnActivateQuest+=Activate;
        GameEventsManager.instance.mapaTiagoEvents.OnDeactivateQuest+=Deactivate;
    }
    void OnDisable(){
        GameEventsManager.instance.mapaTiagoEvents.OnActivateQuest-=Activate;
        GameEventsManager.instance.mapaTiagoEvents.OnDeactivateQuest-=Deactivate;
    }
    void Start(){
        iconParent = transform.GetChild(0);
        Debug.Log(iconParent);
        iconParent.gameObject.SetActive(false);
    }
    void Activate(int id){
        if(id==QuestId){
            iconParent.gameObject.SetActive(true);
        }
    }
    void Deactivate(int id){
        if(id==QuestId){
            iconParent.gameObject.SetActive(false);
        }
    }
}

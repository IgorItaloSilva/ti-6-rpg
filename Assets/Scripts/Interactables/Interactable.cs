using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class Interactable : MonoBehaviour
{
    [field:SerializeField]public string saveId{get; protected set;}
    [SerializeField]protected bool ignoreSaveLoad;
    public bool AlreadyInterated{get; protected set;}
    public bool Active{get;protected set;}
    public bool CanInteract {get;protected set;}
    protected SphereCollider sphereCollider;
    [SerializeField] protected UnityEvent myEvent;
    protected virtual void Awake(){
        if(!ignoreSaveLoad){
            if(LevelLoadingManager.instance==null){
                Debug.LogWarning($"O interactable {saveId} está tentando se adicionar na lista de interactables, mas não temos um LevelLoadingManger na cena");
            }
            LevelLoadingManager.instance.interactables.Add(this);
            if(saveId==""){
                //Debug.LogWarning($"O GameObject "+gameObject.name+" está sem id e marcado para salvar, dando o nome do Objeto para ele");
                saveId=gameObject.name;
            }
        }
    }
    protected virtual void Start(){
        sphereCollider=GetComponent<SphereCollider>();
        sphereCollider.isTrigger=true;
    }
    protected virtual void OnTriggerEnter(Collider collider){
        myEvent.Invoke();
    }
    public virtual void Load(InteractableData interactableData){
        if(ignoreSaveLoad)return;
        AlreadyInterated=interactableData.alreadyInterated;
        Active=interactableData.active;
        CanInteract=interactableData.canInteract;
    }
    public virtual void Save(){
        if(ignoreSaveLoad)return;
        if(LevelLoadingManager.instance==null){
            Debug.Log($"O interactable {saveId} está tentando se salvar, mas não temos um LevelLoadingManger na cena");
        }
        //see if we have this data in dictionary
        if(LevelLoadingManager.instance.CurrentLevelData.interactablesData.ContainsKey(saveId)){
            //if so change it
            InteractableData newData = new InteractableData(this);
            LevelLoadingManager.instance.CurrentLevelData.interactablesData[saveId]=newData;
        }
        else{
            //if not add it
            InteractableData newData = new InteractableData(this);
            LevelLoadingManager.instance.CurrentLevelData.interactablesData.Add(saveId,newData);
        }
    }
    
}

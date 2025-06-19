using UnityEngine;

public abstract class ObjectiveInstantiable : MonoBehaviour
{
    public ObjectiveSO objectiveSO;
    public ObjectiveData objectiveData;
    public string displayCompletedMessage;
    void Awake(){
        objectiveData = new();
    }
    virtual public void Settup(ObjectiveSO objectiveSO){
        this.objectiveSO=objectiveSO;
        GameEventsManager.instance.objectiveEvents.OnProgressMade+=Progress;
    }
    virtual public void myDestroy(){
        GameEventsManager.instance.objectiveEvents.OnProgressMade-=Progress;
        Destroy(gameObject);
    }
    virtual public void StartObjective(){
        GameEventsManager.instance?.objectiveEvents.StartObjective(objectiveSO.Id);
        objectiveData.hasStarted=true;
        UpdateDisplayMessage();
        SaveObjective();
        //update Ui
    }
        
    virtual public void CompleteObjective(){
        GameEventsManager.instance?.objectiveEvents.CompleteObjective(objectiveSO.Id);
        GameEventsManager.instance?.playerEvents.PlayerGainExp(objectiveSO.ExpGain);
        objectiveData.hasFinished=true;
        UpdateDisplayMessage();
        //update Ui
        SaveObjective();
        myDestroy();
    }
    abstract public void Progress(string id);
        //progredir a quest
        //Avisar a Ui
        //Salvar
    
    abstract public void LoadObjective(string codedSave);
        //decodificar o save de string pra o que vc usa
        //atrualizar o estado da quest
        //Avisar a Ui
    
    abstract public void SaveObjective();
        /* string encodedData="";
        encode the state into a string
        ObjectiveManager.instance.UpdateQuestData(encodedData); */
    abstract public void UpdateDisplayMessage();
}

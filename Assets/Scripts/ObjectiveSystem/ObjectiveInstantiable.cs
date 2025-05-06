using UnityEngine;

public abstract class ObjectiveInstantiable : MonoBehaviour
{
    public ObjectiveSO objectiveSO;
    public ObjectiveData objectiveData;
    void Awake(){
        objectiveData = new();
    }
    virtual public void Settup(ObjectiveSO objectiveSO){
        this.objectiveSO=objectiveSO;
        GameEventsManager.instance.objectiveEvents.OnProgessMade+=Progress;
    }
    virtual public void myDestroy(){
        GameEventsManager.instance.objectiveEvents.OnProgessMade-=Progress;
        Destroy(gameObject);
    }
    virtual public void StartObjective(){
        objectiveData.hasStarted=true;
        SaveObjective();
    }
        
    virtual public void CompleteObjective(){
        objectiveData.hasFinished=true;
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
    
}

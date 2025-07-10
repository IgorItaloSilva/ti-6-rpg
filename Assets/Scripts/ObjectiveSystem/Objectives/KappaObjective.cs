using UnityEngine;

public class KappaObjective : ObjectiveInstantiable
{
    bool arvoreEncontrada;
    public override void StartObjective(bool shouldStartObjective)
    {
        base.StartObjective(shouldStartObjective);
        UIManager.instance.ObjectiveUpdate(objectiveSO.objectiveTitle,displayCompletedMessage);
    
    }
    void Start(){
        UpdateDisplayMessage();
        UIManager.instance.ObjectiveUpdate(objectiveSO.objectiveTitle,displayCompletedMessage);
    }
    public override void CompleteObjective()
    {
        base.CompleteObjective();
        UIManager.instance.ObjectiveUpdate(objectiveSO.objectiveTitle,displayCompletedMessage);
    }
    public override void LoadObjective(string codedSave)
    {
        if (bool.TryParse(codedSave, out arvoreEncontrada))
        {
            UpdateDisplayMessage();
            UIManager.instance.ObjectiveUpdate(objectiveSO.objectiveTitle, displayCompletedMessage);
        }
        else
        {
            Debug.LogError("Não conseguimos decoficar o save do KappaObjective");
        }
        SaveObjective();
    }

    public override void Progress(string id)
    {
        if(id == objectiveSO.Id){
            arvoreEncontrada=true;
            UpdateDisplayMessage();
            UIManager.instance.ObjectiveUpdate(objectiveSO.objectiveTitle,displayCompletedMessage);
            SaveObjective();
            CompleteObjective();
        }
    }
    public override void SaveObjective()
    {
        string s = arvoreEncontrada.ToString();
        objectiveData.stringData=s;
        objectiveData.displayCompletedMessage=displayCompletedMessage;
        ObjectiveManager.instance.UpdateQuestData(objectiveSO.Id,objectiveData);
    }

    public override void UpdateDisplayMessage()
    {
        string s;
        if (arvoreEncontrada) s = "Sim!";
        else s = "Não!";
        displayCompletedMessage = objectiveSO.objectiveTextProgress+s;
    }
}

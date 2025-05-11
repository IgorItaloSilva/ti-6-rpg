using Codice.CM.Client.Differences.Merge;
using UnityEngine;

public class KitsuneObjective : ObjectiveInstantiable
{
    int ammountKitsunesKilled;
    [SerializeField]int kitsunesToKill = 4;
    public override void StartObjective()
    {
        base.StartObjective();
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
    public override void Progress(string id)
    {
        if(id == objectiveSO.Id){
            ammountKitsunesKilled++;
            UpdateDisplayMessage();
            UIManager.instance.ObjectiveUpdate(objectiveSO.objectiveTitle,displayCompletedMessage);
            SaveObjective();
            if(ammountKitsunesKilled>=4){
                CompleteObjective();
            }
        }
    }
    public override void LoadObjective(string codedSave)
    {
         if(int.TryParse(codedSave, out ammountKitsunesKilled)){
            UpdateDisplayMessage();
            UIManager.instance.ObjectiveUpdate(objectiveSO.objectiveTitle,displayCompletedMessage);
        }
        else{
            Debug.LogError("Não conseguimos decoficar o save do kitsuneObjective");
        }
        SaveObjective(); 
    }

    public override void SaveObjective()
    {
        string s = ammountKitsunesKilled.ToString();
        objectiveData.stringData=s;
        objectiveData.displayCompletedMessage=displayCompletedMessage;
        ObjectiveManager.instance.UpdateQuestData(objectiveSO.Id,objectiveData);
    }
    public override void UpdateDisplayMessage()
    {
        displayCompletedMessage= objectiveSO.objectiveTextProgress+ammountKitsunesKilled.ToString()+"/"+kitsunesToKill.ToString();
    }
}

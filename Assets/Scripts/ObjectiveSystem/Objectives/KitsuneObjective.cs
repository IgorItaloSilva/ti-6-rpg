using Codice.CM.Client.Differences.Merge;
using UnityEngine;

public class KitsuneObjective : ObjectiveInstantiable
{
    int ammountKitsunesKilled;
    [SerializeField]int kitsunesToKill = 4;
    public override void StartObjective()
    {
        base.StartObjective();
        UIManager.instance.ObjectiveUpdate(objectiveSO.objectiveTitle,UpdateString());
    
    }
    void Start(){
        UIManager.instance.ObjectiveUpdate(objectiveSO.objectiveTitle,UpdateString());
    }
    public override void CompleteObjective()
    {
        base.CompleteObjective();
        UIManager.instance.ObjectiveUpdate(objectiveSO.objectiveTitle,UpdateString());
    }
    public override void Progress(string id)
    {
        if(id == objectiveSO.Id){
            ammountKitsunesKilled++;
            UIManager.instance.ObjectiveUpdate(objectiveSO.objectiveTitle,UpdateString());
            SaveObjective();
            if(ammountKitsunesKilled>=4){
                CompleteObjective();
            }
        }
    }
    public override void LoadObjective(string codedSave)
    {
         if(int.TryParse(codedSave, out ammountKitsunesKilled)){
            UIManager.instance.ObjectiveUpdate(objectiveSO.objectiveTitle,UpdateString());
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
        ObjectiveManager.instance.UpdateQuestData(objectiveSO.Id,objectiveData);
    }
    string UpdateString(){
        return objectiveSO.objectiveTextProgress+ammountKitsunesKilled.ToString()+"/"+kitsunesToKill.ToString();
    }
}

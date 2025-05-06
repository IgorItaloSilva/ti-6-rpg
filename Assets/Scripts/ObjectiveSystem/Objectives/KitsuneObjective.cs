using UnityEngine;

public class KitsuneObjective : ObjectiveInstantiable
{
    int ammountKitsunesKilled;
    public override void StartObjective()
    {
        throw new System.NotImplementedException();
    }

    public override void Progress(string id)
    {
        if(id == objectiveSO.Id){
            ammountKitsunesKilled++;
            if(ammountKitsunesKilled>4){
                CompleteObjective();
            }
        }
    }

    public override void LoadObjective(string codedSave)
    {
        
    }

    public override void SaveObjective()
    {
        string s = ammountKitsunesKilled.ToString();
        ObjectiveManager.instance.UpdateQuestData(objectiveSO.Id,objectiveData);
    }

    public override void CompleteObjective()
    {
        throw new System.NotImplementedException();
    }

}

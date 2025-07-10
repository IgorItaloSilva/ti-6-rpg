using UnityEngine;

public class KappaDialogAnswer : DialogAnswer
{
    [SerializeField] ObjectiveSO objectiveSO;
    public override void Option1()
    {
        ObjectiveManager.instance?.StartQuest(objectiveSO);
    }

    public override void Option2()
    {
        ObjectiveManager.instance?.RefuseQuest(objectiveSO.Id);
    }

    public override void Option3()
    {
        
    }
}
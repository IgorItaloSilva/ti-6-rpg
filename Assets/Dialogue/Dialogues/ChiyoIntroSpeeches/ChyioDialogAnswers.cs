using UnityEngine;

public class ChyioDialogAnswers : DialogAnswer
{
    [SerializeField]ObjectiveSO objectiveSO;
    public override void Option1()
    {
        ObjectiveManager.instance?.StartQuest(objectiveSO);
    }

    public override void Option2()
    {
        //algo pq nos recusamos a quest
    }

    public override void Option3()
    {
        throw new System.NotImplementedException();
    }
}

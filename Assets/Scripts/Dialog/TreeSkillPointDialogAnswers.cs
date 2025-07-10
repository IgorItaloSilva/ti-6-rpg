using UnityEngine;

public class TreeSkillPointDialogAnswers :DialogAnswer
{
    [SerializeField] ObjectiveSO objectiveSO;
    public override void Option1()
    {
        SkillTree.instance?.GainMoney((int)Enums.PowerUpType.Dark);
        GameEventsManager.instance?.objectiveEvents.ProgressMade(objectiveSO.Id);
    }

    public override void Option2()
    {
        SkillTree.instance?.GainMoney((int)Enums.PowerUpType.Light);
        GameEventsManager.instance?.objectiveEvents.ProgressMade(objectiveSO.Id);
    }

    public override void Option3()
    {
        //
    }
}

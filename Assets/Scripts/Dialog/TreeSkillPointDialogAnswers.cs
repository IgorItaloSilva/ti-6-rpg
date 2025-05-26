using UnityEngine;

public class TreeSkillPointDialogAnswers :DialogAnswer
{
    public override void Option1()
    {
        SkillTree.instance?.GainMoney((int)Enums.PowerUpType.Dark);
    }

    public override void Option2()
    {
        SkillTree.instance?.GainMoney((int)Enums.PowerUpType.Light);
    }

    public override void Option3()
    {
        //
    }
}

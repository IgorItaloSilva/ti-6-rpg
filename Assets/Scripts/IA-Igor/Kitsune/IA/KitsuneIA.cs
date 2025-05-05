using UnityEngine;

public class KitsuneIA : ASkills
{
    protected override void SetAllSkills()
    {
        allSkills = new EnemyBaseState[1];
        allSkills[0] = new NewKitsuneDash();
        base.SetAllSkills();
    }

}
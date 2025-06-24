using UnityEngine;

public class ChochinSkills : ASkills
{
    protected override void SetAllSkills()
    {
        allSkills = new EnemyBaseState[2];
        allSkills[0] = new ChochinMelee();
        allSkills[1] = new ChochinRange();
        base.SetAllSkills();
        isRangeSkill[0] = false;
        isRangeSkill[1] = false;
    }

}
using UnityEngine;

public class KitsuneMob1 : ASkills
{
    protected override void SetAllSkills()
    {
        allSkills = new EnemyBaseState[2];
        allSkills[0] = new NewKitsuneDash();
        allSkills[1] = new NewKitsuneMagicAttack();
        isRangeSkill = new bool[allSkills.Length];
        base.SetAllSkills();
        isRangeSkill[1] = true;
        
    }
}

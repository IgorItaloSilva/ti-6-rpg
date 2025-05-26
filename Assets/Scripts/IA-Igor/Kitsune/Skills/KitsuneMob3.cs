using UnityEngine;

public class KitsuneMob3 : ASkills
{
    protected override void SetAllSkills()
    {
        allSkills = new EnemyBaseState[2];
        allSkills[0] = new NewKitsuneDash();
        allSkills[1] = new NewKitsuneAttackAOE();
        isRangeSkill = new bool[allSkills.Length];
        base.SetAllSkills();
        
    }
}

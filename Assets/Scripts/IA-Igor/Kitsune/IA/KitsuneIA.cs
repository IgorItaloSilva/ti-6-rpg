using UnityEngine;

public class KitsuneIA : ASkills
{
    protected override void SetAllSkills()
    {
        allSkills = new EnemyBaseState[2];
        allSkills[0] = new NewKitsuneDash();
        allSkills[1] = new NewKitsuneMagicAttack();
        allSkillsCheck = new bool[allSkills.Length];
        isRangeSkill = new bool[allSkills.Length];
        isRangeSkill[1] = true;
        base.SetAllSkills();
        
    }

}
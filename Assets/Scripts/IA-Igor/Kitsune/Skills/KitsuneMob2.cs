using UnityEngine;

public class KitsuneMob2 : ASkills
{
    protected override void SetAllSkills()
    {
        allSkills = new EnemyBaseState[2];
        allSkills[0] = new NewKitsuneMagicAttack();
        allSkills[1] = new NewKitsuneAttackAOE();
        isRangeSkill = new bool[allSkills.Length];
        base.SetAllSkills();
        isRangeSkill[0] = true;
        
    }
}

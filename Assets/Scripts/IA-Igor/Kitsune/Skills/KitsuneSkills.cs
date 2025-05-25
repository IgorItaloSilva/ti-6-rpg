using UnityEngine;

public class KitsuneSkills : ASkills
{
    protected override void SetAllSkills()
    {
        allSkills = new EnemyBaseState[3];
        allSkills[0] = new NewKitsuneDash();
        allSkills[1] = new NewKitsuneMagicAttack();
        allSkills[2] = new NewKitsuneAttackAOE();
        isRangeSkill = new bool[allSkills.Length];
        base.SetAllSkills();
        isRangeSkill[1] = true;
        
    }

}
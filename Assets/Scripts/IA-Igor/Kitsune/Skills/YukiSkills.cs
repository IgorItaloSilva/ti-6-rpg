using UnityEngine;

public class YukiSkills : ASkills
{
    protected override void SetAllSkills()
    {
        allSkills = new EnemyBaseState[3];
        allSkills[0] = new YukiAttack1();
        allSkills[1] = new YukiAttack2();
        allSkills[2] = new YukiAttack3();
        base.SetAllSkills();
        isRangeSkill[2] = true;

    }

}
using UnityEngine;

public class YukiSkills : ASkills
{
    protected override void SetAllSkills()
    {
        allSkills = new EnemyBaseState[1];
        allSkills[0] = new YukiAttack1();
        base.SetAllSkills();
        //isRangeSkill[1] = false;
    }

}
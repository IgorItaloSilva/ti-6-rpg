using UnityEngine;

public class YukiSkills : ASkills
{
    protected override void SetAllSkills()
    {
        allSkills = new EnemyBaseState[2];
        allSkills[0] = new YukiAttack1();
        allSkills[1] = new YukiAttack2();
        base.SetAllSkills();

    }

}
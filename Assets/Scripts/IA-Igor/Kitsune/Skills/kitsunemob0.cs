using UnityEngine;

public class kitsunemob0 : ASkills
{
    protected override void SetAllSkills()
    {
        allSkills = new EnemyBaseState[1];
        allSkills[0] = new NewKitsuneDash();
        isRangeSkill = new bool[allSkills.Length];
        base.SetAllSkills();
        
    }
}


using UnityEngine;

public class KitsuneSkills : ASkills
{
    protected override void SetAllSkills()
    {
        allSkills = new EnemyBaseState[1];
        allSkills[0] = new NewKitsuneDash();
       // allSkills[1] = new NewKitsuneMagicAttack();
        isRangeSkill = new bool[allSkills.Length];
        //isRangeSkill[1] = true;
        base.SetAllSkills();
        
    }

}
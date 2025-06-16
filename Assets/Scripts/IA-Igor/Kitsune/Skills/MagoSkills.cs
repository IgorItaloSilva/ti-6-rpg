using UnityEngine;

public class MagoSkills : ASkills
{
    protected override void SetAllSkills()
    {
        allSkills = new EnemyBaseState[2];
        allSkills[0] = new MagoFireTornado();
        allSkills[1] = new MagoMagicPunch();
        base.SetAllSkills();
        isRangeSkill[0] = true;
        isRangeSkill[1] = true;
    }
}
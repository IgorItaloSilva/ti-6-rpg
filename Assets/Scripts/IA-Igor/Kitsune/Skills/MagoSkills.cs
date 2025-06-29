using UnityEngine;

public class MagoSkills : ASkills
{
    protected override void SetAllSkills()
    {
        allSkills = new EnemyBaseState[5];
        allSkills[0] = new MagoFireTornado();
        allSkills[1] = new MagoMagicPunch();
        allSkills[2] = new MagoDash();
        allSkills[3] = new MagoEarthQuake();
        allSkills[4] = new MagoLazer();
        base.SetAllSkills();
        isRangeSkill[0] = true;
        isRangeSkill[1] = true;
        isRangeSkill[2] = false;
        isRangeSkill[3] = false;
        isRangeSkill[4] = true;
    }

}
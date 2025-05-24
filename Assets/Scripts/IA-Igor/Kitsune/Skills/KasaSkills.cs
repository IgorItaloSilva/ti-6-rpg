public class KasaSkills : ASkills
{
    protected override void SetAllSkills()
    {
        allSkills = new EnemyBaseState[1];
        allSkills[0] = new KasaDash();
        base.SetAllSkills();

    }

}
using UnityEngine;


public abstract class ASkills : MonoBehaviour
{
    protected EnemyBaseState[] allSkills;
    protected bool[] allSkillsCheck;
    [SerializeField] EnemyBaseWeapon[] weapons;
    byte count;
    int indexSkill;
    protected bool[] isRangeSkill;

    void Awake() { SetAllSkills(); }

    protected virtual void SetAllSkills() { allSkillsCheck = new bool[allSkills.Length]; isRangeSkill = new bool[allSkills.Length]; } // Settar todas as skills do inimigo, chamar esse metodo base e definir quais são range

    public EnemyBaseState ChoseSkill()
    {
        count = (byte)allSkillsCheck.Length;
        //Debug.Log("Index atual : " + indexSkill + " || Skill atual : " + allSkills[indexSkill]);
        indexSkill = Random.Range(0, allSkills.Length);
        //Debug.Log("Index Depois : " + indexSkill + " || Skill Depois : " + allSkills[indexSkill]);

        if (!allSkillsCheck[indexSkill])
        { // Não usou skill ?
            SkillSelected();
            return allSkills[indexSkill]; // Retorna skill
        }
        for (int i = 1; i < count; i++)
        { // Pegar outra skill
            int nextIndex = (indexSkill + i) % count;
            if (!allSkillsCheck[nextIndex])
            {
                indexSkill = nextIndex;
                SkillSelected();
                return allSkills[nextIndex];
            }

        }

        for (int i = 0; i < count; i++)
        { // Resetar todas as skills
            allSkillsCheck[i] = false;
        }
        SkillSelected();
        return allSkills[indexSkill];

    }

    void SkillSelected() { allSkillsCheck[indexSkill] = true; }

    public void UseWeapon() { weapons[indexSkill].gameObject.SetActive(true); }

    public bool IsRangeSkill() { return isRangeSkill[indexSkill]; }
    
    public void DisableWeapon(){ weapons[indexSkill].gameObject.SetActive(false); }

}
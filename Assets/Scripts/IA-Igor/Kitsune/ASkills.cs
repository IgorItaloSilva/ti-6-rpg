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

    protected virtual void SetAllSkills(){ allSkillsCheck = new bool[allSkills.Length]; }

    public EnemyBaseState ChoseSkill() {
        count = (byte)allSkillsCheck.Length;
        Debug.Log("Index atual : " + indexSkill + " || Skill atual : " + allSkills[indexSkill]);
        indexSkill = Random.Range(0, allSkills.Length);
        Debug.Log("Index Depois : " + indexSkill + " || Skill Depois : " + allSkills[indexSkill]);

        if(!allSkillsCheck[indexSkill]){ // Não usou skill ?
            SkillSelected();
            return allSkills[indexSkill]; // Retorna skill
        }
        for(int i = 1; i < count; i++){ // Pegar outra skill
            int nextIndex = (indexSkill + i) % count;
            if(!allSkillsCheck[nextIndex]){
                indexSkill = nextIndex;
                SkillSelected();
                return allSkills[nextIndex];
            }

        }

        for(int i = 0; i < count; i++){ // Resetar todas as skills
            allSkillsCheck[i] = false;
        }
        SkillSelected();
        return allSkills[indexSkill];

    }

    void SkillSelected(){ allSkillsCheck[indexSkill] = true; }

    public void UseWeapon() { Debug.Log("Usar : " + weapons[indexSkill].name); weapons[indexSkill].gameObject.SetActive(true); }

    public bool IsRangeSkill(){ return isRangeSkill[indexSkill]; }

}
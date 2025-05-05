using UnityEngine;


public abstract class ASkills : MonoBehaviour
{
    protected EnemyBaseState[] allSkills;
    protected bool[] allSkillsCheck;
    [SerializeField] EnemyBaseWeapon[] weapons;
    byte count;
    int indexSkill;

    void Awake() { SetAllSkills(); }

    protected virtual void SetAllSkills(){
        allSkillsCheck = new bool[allSkills.Length];
        
    }

    public EnemyBaseState ChoseSkill() {
        count = (byte)allSkillsCheck.Length;
        indexSkill = Random.Range(0, allSkills.Length);

        if(!allSkillsCheck[indexSkill]) // Não usou skill ?
            return allSkills[indexSkill]; // Retorna skill

        for(int i = 1; i < count; i++){ // Pegar outra skill
            int nextIndex = (indexSkill + i) % count;
            if(!allSkillsCheck[nextIndex]){
                return allSkills[nextIndex];
            }

        }

        for(int i = 0; i < count; i++){ // Resetar todas as skills
            allSkillsCheck[i] = false;
        }

        return allSkills[indexSkill];

    }

    public void UseWeapon(Transform target) {
            weapons[indexSkill].SetTarget(target);
            weapons[indexSkill].gameObject.SetActive(true); 
        }

}
using UnityEngine;


public abstract class ASkills : MonoBehaviour
{
    protected EnemyBaseState[] allSkills;
    protected bool[] allSkillsCheck;
    byte count;

    void Start() { SetAllSkills(); }
    protected virtual void SetAllSkills(){
        allSkillsCheck = new bool[allSkills.Length];
        
    }

    public EnemyBaseState ChoseSkill() {
        count = (byte)allSkillsCheck.Length;
        int randIndex = Random.Range(0, allSkills.Length);

        if(!allSkillsCheck[randIndex]) // Não usou skill ?
            return allSkills[randIndex]; // Retorna skill

        for(int i = 1; i < count; i++){ // Pegar outra skill
            int nextIndex = (randIndex + i) % count;
            if(!allSkillsCheck[nextIndex]){
                return allSkills[nextIndex];
            }

        }

        for(int i = 0; i < count; i++){ // Resetar todas as skills
            allSkillsCheck[i] = false;
        }

        return allSkills[randIndex];

    }
}
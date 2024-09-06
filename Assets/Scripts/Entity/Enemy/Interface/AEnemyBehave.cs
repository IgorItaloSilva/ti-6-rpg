using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


public abstract class AEnemyBehave : MonoBehaviour
{
    protected EnemyController enemyController; // Controlador deste personagem
    protected AEnemyAction[] allActions; // Todas as ações possiveis do personagem
    protected float[] coolDown;
    protected float[] coolUp;


    int startSkills;
    int indexMelee;

    public void StartBehave(EnemyController enemyController)
    {
        this.enemyController = enemyController; // Define o controlador deste personagem
        SetActions();
    }

    protected abstract void SetActions(); // Define todas as ações deste personagem

    public AEnemyAction GetAction(int index)
    {

        return allActions[index];
    }

    public bool SkillReady()
    {
        if (coolUp[3] <= 0)
        {
            return true;
        }
        return false;
    }

    public void SkillUsed(int index)
    {
        coolUp[index] = coolDown[index];
    }

    public void CoolDownTimer()
    {
        for(int i = startSkills; i < coolUp.Length; i++)
        {
            coolUp[i] = Mathf.Max(0, coolUp[i] - Time.fixedDeltaTime);
        }
    }

}

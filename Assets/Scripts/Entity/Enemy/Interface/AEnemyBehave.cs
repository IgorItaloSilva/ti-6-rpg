using System.Collections.Generic;
using UnityEngine;


public abstract class AEnemyBehave : MonoBehaviour
{
    protected OldEnemyController enemyController; // Controlador deste personagem
    protected List<AEnemyAction> actionList = new List<AEnemyAction>();
    protected List<AEnemyAction> actionsCanUse = new List<AEnemyAction>();

    protected int startSkills;
    protected bool haveToRest;


    public void StartBehave(OldEnemyController enemyController, out AEnemyAction action) // Definir Corpo do cerebro
    {
        this.enemyController = enemyController; // Define o controlador deste personagem
        SetActions();
        action = actionList[0];
    }

    protected abstract void SetActions(); // Define todas as ações deste personagem


    //public abstract AEnemyAction GetAction();

    public abstract void Think(out AEnemyAction action, bool haveToRest);

    protected abstract void HaveToRest(bool haveToRest);

}

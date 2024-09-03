using System.Collections;
using UnityEngine;


public class KitsuneDash : AEnemyAction
{
    public override void StartAction(IEnemyBehave _enemyBehave)
    {
        base.StartAction(_enemyBehave);
        Debug.Log("Dash");
    }

    public override void UpdateAction()
    {
        //throw new System.NotImplementedException();
    }

    public override void ExitAction(AEnemyAction _nextAction)
    {
        enemyBehaviour.CheckDistance();
    }
}
using System.Collections;
using UnityEngine;

public class KitsuneAttack : AEnemyAction
{
    public override void Antecipation()
    {
        Debug.Log("Attack!");
    }
    public override void ExitAction(AEnemyAction _nextAction)
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateAction()
    {
        //
    }
}


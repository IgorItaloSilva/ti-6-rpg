using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestKitsuneController : TestEnemyController
{
    protected override void StartEnemy()
    {
        movementActions.Add(new TestKitsuneIdle());
        movementActions.Add(new TestKitsuneRun());
        attackActions.Add(new TestKitsuneHeadButt());
        ShuffleAttacks();
        currentAction = new TestKitsuneIdle();
        currentAction.StartAction(this);

    }

}

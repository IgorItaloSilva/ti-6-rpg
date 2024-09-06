using UnityEngine;


public class BotTestBehave : AEnemyBehave
{
    protected override void SetActions()
    {
        allActions = new AEnemyAction[]
        {
            new BotTestIdle(this), // 0
            new BotTestApproach(this), // 1
            new BotTestFlee(this), // 2
            new BotTestAttack(this), // 3
            new BotTestTurn(this) // 4
        };
        coolDown = new float[allActions.Length];
        coolUp = new float[allActions.Length];
        coolDown[3] = 5f;
    }
}

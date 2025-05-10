using UnityEngine;

public class NewKitsuneBehave : EnemyBehaviour
{
    protected override void OneExecution()
    {
        idleState = new NewKitsuneIdle();
        currentState = idleState;
        currentState.StateStart(this);
    }
}

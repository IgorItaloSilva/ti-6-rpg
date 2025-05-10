using UnityEngine;

public class KasaBehave : EnemyBehaviour
{
    protected override void OneExecution()
    {
        idleState = new KasaIdle();
        currentState = idleState;
        currentState.StateStart(this);
    }
}
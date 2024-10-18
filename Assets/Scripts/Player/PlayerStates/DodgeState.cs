using UnityEngine.InputSystem;

public class DodgeState : PlayerState
{
    public override PlayerMovement.moveTypes moveType => PlayerMovement.moveTypes.dodging;
    public override float MoveSpeed => 70f;
    public override float TurnTime => 0.05f;
    
    public override void Update()
    {
        
    }
}

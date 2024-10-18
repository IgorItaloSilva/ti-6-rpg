using UnityEngine.InputSystem;

public class SprintState : PlayerState
{
    public override PlayerMovement.moveTypes moveType => PlayerMovement.moveTypes.sprinting;
    public override float MoveSpeed => 18f;
    public override float TurnTime => 0.125f;

    public override void Update()
    {
        if (!PlayerMovement.instance.IsGrounded) return;
        
        if (PlayerMovement.instance.jumpAction.triggered) PlayerMovement.instance.Jump();
        if (PlayerMovement.instance.dodgeAction.triggered) PlayerMovement.instance.DodgeAsync();
    }
    
}

using UnityEngine.InputSystem;

public class WalkState : PlayerState
{
    public override PlayerMovement.moveTypes moveType => PlayerMovement.moveTypes.walking;
    public override float MoveSpeed => 12f;
    public override float TurnTime => 0.1f;
    
    public override void Update()
    {
        if (!PlayerMovement.instance.IsGrounded) return;
        
        if (PlayerMovement.instance.sprintAction.triggered) PlayerMovement.instance.SprintAsync();
        if (PlayerMovement.instance.jumpAction.triggered) PlayerMovement.instance.Jump();
        if (PlayerMovement.instance.dodgeAction.triggered) PlayerMovement.instance.DodgeAsync();
    }
}

using UnityEngine.InputSystem;

public class LandState : PlayerState
{
    public override PlayerMovement.moveTypes moveType => PlayerMovement.moveTypes.landing;
    public override float MoveSpeed => 6f;
    public override float TurnTime => 0.15f;

    public override void Enter()
    {
        PlayerMovement.instance.SwitchMovements(toCC: true);
    }

    public override void Update()
    {
        if (!PlayerMovement.instance.IsGrounded) return;
    }
}
using UnityEngine;

public class PlayerInAirState : PlayerBaseState
{
    protected const float MoveSpeed = 8f, SprintSpeed = 12f;

    public PlayerInAirState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
    }

    public override void EnterState()
    {
        Debug.Log("In Air");
        _ctx.TurnTime = _ctx.BaseTurnTime * _ctx.SlowTurnTimeModifier;
    }

    public override void UpdateState()
    {
        HandleGravity();
        HandleAirMove();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        if (_ctx.CC.isGrounded)
            SwitchState(_factory.Grounded());
    }

    private void HandleGravity()
    {
        var previousYVelocity = _ctx.CurrentMovementY;
        _ctx.CurrentMovementY += (_ctx.Gravity * Time.deltaTime);
        _ctx.AppliedMovementY = ((previousYVelocity + _ctx.CurrentMovementY));
    }

    private void HandleAirMove()
    {
        _ctx.AppliedMovement = new Vector3(_ctx.transform.forward.x, _ctx.AppliedMovementY, _ctx.transform.forward.z);

        _ctx.CC.Move(_ctx.AppliedMovement * ((_ctx.IsSprintPressed ? SprintSpeed : MoveSpeed) * Time.deltaTime));
    }
}
using UnityEngine;

public class PlayerSprintState : PlayerBaseState
{
    protected const float SprintSpeed = 12f;
    public PlayerSprintState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Sprinting");
        _ctx.TurnTime = _ctx.BaseTurnTime * _ctx.SlowTurnTimeModifier;
    }

    public override void UpdateState()
    {
        HandleSprintMove();
        HandleGravity();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        _ctx.TurnTime = _ctx.BaseTurnTime * _ctx.SlowTurnTimeModifier;
    }

    public override void CheckSwitchStates()
    {
        if (_ctx.IsJumpPressed)
        {
            HandleJump();
            SwitchState(_factory.InAir());
        }

        if (!_ctx.CC.isGrounded)
        {
            SwitchState(_factory.InAir());
        }

        if (!_ctx.IsSprintPressed)
        {
            SwitchState(_factory.Grounded());
        }
    }

    private void HandleSprintMove()
    {
        _ctx.AppliedMovement = new Vector3(_ctx.transform.forward.x, _ctx.AppliedMovementY, _ctx.transform.forward.z);
        
        _ctx.CC.Move(_ctx.AppliedMovement * (SprintSpeed * Time.deltaTime));
    }
    private void HandleGravity()
    {
        _ctx.CurrentMovementY = _ctx.BaseGravity;
    }

}
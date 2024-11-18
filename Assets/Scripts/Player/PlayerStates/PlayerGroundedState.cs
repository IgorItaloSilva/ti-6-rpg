using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerGroundedState : PlayerBaseState
{
    private float smoothTime;
    protected const float MoveSpeed = 6f;

    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Grounded");
        _ctx.TurnTime = _ctx.BaseTurnTime;
    }

    public override void UpdateState()
    {
        HandleGravity();
        HandleMove();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        if (!_ctx.CC.isGrounded)
        {
            SwitchState(_factory.InAir());
            return;
        }

        if (_ctx.IsJumpPressed)
        {
            HandleJump();
            SwitchState(_factory.InAir());
        }

        if (_ctx.IsSprintPressed)
        {
            SwitchState(_factory.Sprint());
        }

        if (_ctx.IsDodgePressed)
        {
            SwitchState(_factory.Dodge());
        }

        if (_ctx.IsAttackPressed)
        {
            SwitchState(_factory.Attack());
        }
    }

    private void HandleMove()
    {
        _ctx.AppliedMovement = new Vector3(_ctx.CurrentMovement.x, _ctx.AppliedMovementY, _ctx.CurrentMovement.z);
        _ctx.CC.Move(_ctx.AppliedMovement * (MoveSpeed * Time.deltaTime));
    }

    private void HandleGravity()
    {
        _ctx.CurrentMovementY = _ctx.BaseGravity;
    }
}
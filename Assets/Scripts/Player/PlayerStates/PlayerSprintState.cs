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
        HandleAnimatorParameters();
        Debug.Log("Sprinting");
        _turnTime = _ctx.BaseTurnTime * _ctx.SlowTurnTimeModifier;
    }


    public override void HandleAnimatorParameters()
    {
        _ctx.Animator.SetBool(_ctx.IsWalkingHash, true);
        _ctx.Animator.SetBool(_ctx.IsRunningHash, true);
        _ctx.Animator.SetBool(_ctx.IsGroundedHash, true);
    }

    public override void UpdateState()
    {
        HandleRotation();
        HandleSprintMove();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
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

    private void HandleJump()
    {
        _ctx.Animator.SetBool(_ctx.HasJumpedHash, true);
        _ctx.CanJump = false;
        _ctx.CurrentMovementY = _ctx.InitialJumpVelocity;
        _ctx.AppliedMovementY = _ctx.InitialJumpVelocity;
    }

    private void HandleSprintMove()
    {
        _ctx.AppliedMovement = new Vector3(_ctx.transform.forward.x * SprintSpeed, _ctx.BaseGravity,
            _ctx.transform.forward.z * SprintSpeed);

        _ctx.CC.Move(_ctx.AppliedMovement * Time.deltaTime);
    }
}
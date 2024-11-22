using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerGroundedState : PlayerBaseState
{
    private float smoothTime;
    protected const float MoveSpeed = 6f;

    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        HandleAnimatorParameters();
    }

    public override void EnterState()
    {
        Debug.Log("Grounded");
        _turnTime = _ctx.BaseTurnTime;
    }

    public sealed override void HandleAnimatorParameters()
    {
        _ctx.Animator.SetBool(_ctx.IsGroundedHash, true);
        _ctx.Animator.SetBool(_ctx.IsRunningHash, false);
        _ctx.Animator.SetBool(_ctx.IsWalkingHash, _ctx.IsMovementPressed);
    }


    public override void UpdateState()
    {
        HandleRotation();
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

        if (_ctx.IsClimbing && _ctx.CanMount)
            SwitchState(_factory.Climb());
    }

    public void HandleJump()
    {
        _ctx.Animator.ResetTrigger(_ctx.HasJumpedHash);
        _ctx.Animator.SetTrigger(_ctx.HasJumpedHash);
        _ctx.CanJump = false;
        if (!_ctx.IsMovementPressed)
            _ctx.AppliedMovement = Vector3.zero;
        _ctx.CurrentMovementY = _ctx.InitialJumpVelocity;
        _ctx.AppliedMovementY = _ctx.InitialJumpVelocity;
    }

    private void HandleMove()
    {
        _ctx.AppliedMovement = new Vector3(_ctx.CurrentMovement.x, _ctx.BaseGravity, _ctx.CurrentMovement.z);
        _ctx.CC.Move(_ctx.AppliedMovement * (MoveSpeed * Time.deltaTime));
    }

}
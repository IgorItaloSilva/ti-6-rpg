using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerGroundedState : PlayerBaseState
{
    private Vector3 _appliedMovement;

    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        HandleAnimatorParameters();
    }

    public override void EnterState()
    {
        if(_ctx.ShowDebugLogs) Debug.Log("Grounded");
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

        if (_ctx.IsSprintPressed && _ctx.IsMovementPressed)
        {
            SwitchState(_factory.Sprint());
        }

        if (_ctx.IsDodgePressed && _ctx.IsMovementPressed)
        {
            SwitchState(_factory.Dodge());
        }

        if (_ctx.IsAttackPressed)
        {
            SwitchState(_factory.Attack());
            AudioPlayer.instance.PlaySFX("Cut");
        }

        if (_ctx.IsClimbing && _ctx.CanMount)
            SwitchState(_factory.Climb());
    }

}
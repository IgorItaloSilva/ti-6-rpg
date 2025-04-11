using Unity.VisualScripting;
using UnityEditor;
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
        _turnTime = _ctx.BaseTurnTime * 2;
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
    
    protected override void HandleMove()
    {
        _ctx.AppliedMovement = new Vector3(_ctx.transform.forward.x * _ctx.BaseMoveSpeed * _ctx.Acceleration, _ctx.BaseGravity,
            _ctx.transform.forward.z * _ctx.BaseMoveSpeed * _ctx.Acceleration);

        _ctx.CC.Move(_ctx.AppliedMovement * Time.deltaTime);
    }

    public override void CheckSwitchStates()
    {
        // if(_ctx.IsOnTarget)
        // {
        //     SwitchState(_factory.Combat());
        //     return;
        // }
        //
        if (!_ctx.CC.isGrounded)
        {
            
            _ctx.Animator.SetBool(_ctx.InCombatHash, false);
            SwitchState(_factory.InAir());
            return;
        }

        if (_ctx.IsJumpPressed)
        {
            
            _ctx.Animator.SetBool(_ctx.InCombatHash, false);
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
        }

        if (_ctx.IsClimbing && _ctx.CanMount)
        {
            _ctx.Animator.SetBool(_ctx.InCombatHash, false);
            SwitchState(_factory.Climb());
        }
    }

}
using UnityEngine;

public class PlayerSprintState : PlayerBaseState
{

    public PlayerSprintState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        _maxAcceleration = 2.5f;
    }

    public override void EnterState()
    {
        HandleAnimatorParameters();
        if(_ctx.ShowDebugLogs) Debug.Log("Sprinting");
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
        HandleMove();
        HandlePotion();
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

        if (!_ctx.IsSprintPressed || !_ctx.IsMovementPressed)
        {
            if(_ctx.InCombat)
                SwitchState(_factory.Combat());
            else
                SwitchState(_factory.Grounded());
        }
        if(_ctx.IsAttackPressed)
        {
            SwitchState(_factory.Attack());
        }
        
        if (_ctx.IsClimbing && _ctx.CanMount)
        {
            SwitchState(_factory.Climb());
        }
    }

    protected override void HandleMove()
    {
        _ctx.AppliedMovement = new Vector3(_ctx.transform.forward.x * _ctx.BaseMoveSpeed * _ctx.Acceleration, _ctx.BaseGravity,
            _ctx.transform.forward.z * _ctx.BaseMoveSpeed * _ctx.Acceleration);

        _ctx.CC.Move(_ctx.AppliedMovement * Time.deltaTime);
    }
}
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
        _maxAcceleration = 2.5f;
        _turnTime = _ctx.BaseTurnTime;
    }

    public override void EnterState()
    {
        HandleAnimatorParameters();
        if(_ctx.ShowDebugLogs) Debug.Log("Grounded");
    }

    public override void HandleAnimatorParameters()
    {
        _ctx.Animator.SetBool(_ctx.IsGroundedHash, true);
        _ctx.Animator.SetBool(_ctx.InCombatHash, _ctx.EnemyDetector.targetEnemy);
        _ctx.Animator.SetBool(_ctx.IsRunningHash, false);
        _ctx.Animator.SetBool(_ctx.IsWalkingHash, _ctx.IsMovementPressed);
    }
    
    public override void UpdateState()
    {
        HandleRotation();
        HandleForwardMove();
        HandlePotion();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        if (_ctx.InCombat && !_ctx.IsSprintPressed)
        {
            SwitchState(_factory.Combat());
            return;
        }
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

        if (_ctx.IsDodgePressed)
        {
            SwitchState(_factory.Dodge());
        }

        if (_ctx.IsAttackPressed)
        {
            SwitchState(_factory.Attack());
        }
        
        if (_ctx.IsClimbing)
        {
            _ctx.Animator.SetBool(_ctx.InCombatHash, false);
            SwitchState(_factory.Climb());
        }
        
    }

}
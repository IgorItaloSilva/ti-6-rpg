using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerCombatState : PlayerGroundedState
{
    public PlayerCombatState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        _ctx.AppliedMovementX = 0f;
        _ctx.AppliedMovementZ = 0f;
        _ctx.AppliedMovementY = _ctx.BaseGravity;
    }
    
    public sealed override void HandleAnimatorParameters()
    {
        _ctx.Animator.SetBool(_ctx.IsGroundedHash, true);
        _ctx.Animator.SetBool(_ctx.InCombatHash, true);
        _ctx.Animator.SetBool(_ctx.IsRunningHash, false);
        _ctx.Animator.SetBool(_ctx.IsWalkingHash, _ctx.IsMovementPressed);
    }
    public override void EnterState()
    {
        HandleAnimatorParameters();
        if(_ctx.ShowDebugLogs) Debug.Log("Combat");
        _turnTime = _ctx.BaseTurnTime * 2;
    }
    
    public override void UpdateState()
    {
        if(_ctx.EnemyDetector.targetEnemy)
        {
            HandleTargetedMove();
            HandleTargetedRotation();
        }
        else
        {
            HandleRotation();
            HandleForwardMove();
        }
        HandlePotion();
        CheckSwitchStates();
    }
    
    public override void CheckSwitchStates()
    {
        if (!_ctx.InCombat)
        {
            SwitchState(_factory.Grounded());
            return;
        }
        if (!_ctx.CC.isGrounded)
        {
            
            _ctx.Animator.SetBool(_ctx.InCombatHash, false);
            SwitchState(_factory.InAir());
            return;
        }
        if(_ctx.IsBlockPressed)
        {
            _ctx.Animator.SetBool(_ctx.InCombatHash, true);
            _ctx.Animator.SetBool(_ctx.IsBlockingHash, true);
            SwitchState(_factory.Block());
            return;
        }
        if (_ctx.IsJumpPressed)
        {
            _ctx.Animator.SetBool(_ctx.InCombatHash, false);
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

        if (_ctx.IsClimbing)
        {
            _ctx.Animator.SetBool(_ctx.InCombatHash, false);
            SwitchState(_factory.Climb());
        }
        
    }

}
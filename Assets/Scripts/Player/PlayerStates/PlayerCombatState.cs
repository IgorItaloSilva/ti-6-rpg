using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerCombatState : PlayerGroundedState
{
    private const byte CombatCooldownMs = 255;
    public PlayerCombatState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        _maxAcceleration = 1.5f;
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
        HandleTargetedMove();
        HandleTargetedRotation();
        HandlePotion();
        CheckSwitchStates();
    }
    
    public override void CheckSwitchStates()
    {
        if (!_ctx.InCombat)
        {
            _ctx.CanEnterCombat = false;
            HandleCombatCooldownAsync();
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
            SwitchState(_factory.Grounded());
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
    
    private async void HandleCombatCooldownAsync()
    {
        await Task.Delay(CombatCooldownMs);
        _ctx.CanEnterCombat = true;
    }

}
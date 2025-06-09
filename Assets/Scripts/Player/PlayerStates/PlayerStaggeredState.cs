using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerStaggeredState : PlayerCombatState
{
    public PlayerStaggeredState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        _maxAcceleration = 1.5f;
        _ctx.AppliedMovementY = _ctx.BaseGravity;
        _ctx.CurrentMovement = _ctx.CurrentMovementInput;
        _ctx.CurrentMovementZ = _ctx.CurrentMovementInput.y;
    }

    public override void EnterState()
    {
        HandleAnimatorParameters();
        if (_ctx.ShowDebugLogs) Debug.Log("Staggered");
        _turnTime = _ctx.BaseTurnTime * 5;
    }

    public override void ExitState()
    {
        base.ExitState();
            _ctx.Animator.SetBool(_ctx.IsBlockingHash, _ctx.IsBlockPressed);
    }

    public override void UpdateState()
    {
        Knockback();
        CheckSwitchStates();
        
        if (!_ctx.IsBlockPressed)
        {
            _ctx.Animator.SetBool(_ctx.IsBlockingHash, false);
        }
    }

    private void Knockback()
    {
        _ctx.AppliedMovementX = -_ctx.transform.forward.x * _ctx.BaseMoveSpeed * 2;
        _ctx.AppliedMovementY = _ctx.BaseGravity;
        _ctx.AppliedMovementZ = -_ctx.transform.forward.z * _ctx.BaseMoveSpeed * 2;

        _ctx.CC.Move(_ctx.AppliedMovement * Time.deltaTime);

        _ctx.Animator.SetFloat(_ctx.PlayerVelocityYHash, -1.5f, 0.1f, Time.deltaTime);
        _ctx.Animator.SetFloat(_ctx.PlayerVelocityXHash, 0f, 0.1f, Time.deltaTime);
    }

    public override void CheckSwitchStates()
    {
        if (!_ctx.CC.isGrounded)
        {
            _ctx.Animator.SetBool(_ctx.InCombatHash, false);
            SwitchState(_factory.InAir());
        }
    }
}
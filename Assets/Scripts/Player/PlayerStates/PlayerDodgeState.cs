using System.Threading.Tasks;
using UnityEngine;

public class PlayerDodgeState : PlayerBaseState
{
    private const byte DodgeDurationMs = 200;
    private new const byte DecelerationSpeed = 6;
    private const int DodgeCooldownMs = 500;

    public PlayerDodgeState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        HandleAnimatorParameters();
        _ctx.Acceleration = _ctx.IsMovementPressed ? 4f : -4f;
        if (_ctx.ShowDebugLogs) Debug.Log("Dodging");
        _ctx.IsDodging = true;
        _turnTime = DodgeDurationMs / 2000f;
        _ctx.CanDodge = false;
        HandleDodgeDurationAsync();
    }

    public override void EnterState()
    {
    }

    public sealed override void HandleAnimatorParameters()
    {
        _ctx.Animator.ResetTrigger(_ctx.HasDodgedHash);
        _ctx.Animator.SetTrigger(_ctx.HasDodgedHash);
        _ctx.Animator.SetBool(_ctx.IsGroundedHash, true);
    }

    public override void UpdateState()
    {
        if (_ctx.EnemyDetector.targetEnemy)
        {
            HandleTargetedRotation();
            HandleTargetedMove();
        }
        else
        {
            HandleRotation();
            HandleMove();
        }
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        _ctx.IsDodging = false;
    }

    public override void CheckSwitchStates()
    {
        if (_ctx.IsAttackPressed)
        {
            SwitchState(_factory.Attack());
        }

        if (!_ctx.CC.isGrounded)
        {
            SwitchState(_factory.InAir());
        }
    }

    private async void HandleDodgeDurationAsync()
    {
        HandleDodgeCooldownAsync();
        await Task.Delay(DodgeDurationMs);
        if (!_ctx.Animator.GetBool(_ctx.Attack1Hash))
        {
            SwitchState(_ctx.IsSprintPressed ? _factory.Sprint() : _factory.Grounded());
        }
    }

    private async void HandleDodgeCooldownAsync()
    {
        await Task.Delay(DodgeDurationMs + DodgeCooldownMs);
        while (_ctx.IsDodgePressed)
        {
            await Task.Yield();
        }

        _ctx.CanDodge = true;
    }

    protected override void HandleAcceleration()
    {
        _ctx.Acceleration -= (Time.fixedDeltaTime * DecelerationSpeed);
        _ctx.Animator.SetFloat(_ctx.PlayerVelocityYHash, _ctx.Acceleration);
    }
}
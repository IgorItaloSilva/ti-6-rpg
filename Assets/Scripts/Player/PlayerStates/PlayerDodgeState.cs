using System.Threading.Tasks;
using Player.PlayerStates;
using UnityEngine;

public class PlayerDodgeState : PlayerCombatState
{
    private const int DodgeCooldownMs = 1000, DodgeDurationMs = 300;

    public PlayerDodgeState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        _decelerationSpeed = 40;
        HandleAnimatorParameters();
        _ctx.Acceleration = 8f;
        if (_ctx.ShowDebugLogs) Debug.Log("Dodging");
        _ctx.IsDodging = true;
        _turnTime = DodgeDurationMs / 2000f;
        _ctx.CanDodge = false;
        HandleDodgeDurationAsync();
        _ctx.ResetAttacks();
        _ctx.Animator.speed = 1.75f;
        _ctx.SwordTrail.emitting = true;
    }

    public override void EnterState()
    {
    }
    
    protected override void HandleAcceleration()
    {
        _lowestAccelerationSpeed = Mathf.Min(_ctx.Acceleration, _lowestAccelerationSpeed);

        _ctx.Acceleration -= Time.fixedDeltaTime * _decelerationSpeed;

        if (_lowestAccelerationSpeed < _maxAcceleration)
        {
            _ctx.Acceleration = Mathf.Clamp(_ctx.Acceleration, 0, _maxAcceleration);
        }
    }

    public sealed override void HandleAnimatorParameters()
    {
        _ctx.Animator.ResetTrigger(_ctx.HasDodgedHash);
        _ctx.Animator.SetTrigger(_ctx.HasDodgedHash);
        _ctx.Animator.SetBool(_ctx.IsGroundedHash, true);
        _ctx.Animator.SetFloat(_ctx.PlayerVelocityXHash, 0f);
    }


    public override void ExitState()
    {
        _decelerationSpeed = 10;
        _ctx.IsDodging = false;
        _ctx.Animator.speed = 1f;
        _ctx.SwordTrail.emitting = false;
        HandleDodgeCooldownAsync();
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
        await Task.Delay(DodgeDurationMs);
        if(_ctx.InCombat)
            SwitchState(_factory.Combat());
        else
            SwitchState(_factory.Grounded());
    }

    private async void HandleDodgeCooldownAsync()
    {
        await Task.Delay(DodgeCooldownMs);
        while (_ctx.IsDodgePressed)
        {
            await Task.Yield();
        }

        _ctx.CanDodge = true;
    }
}
using System.Threading.Tasks;
using UnityEngine;

public class PlayerDodgeState : PlayerBaseState
{
    protected const float DodgeSpeed = 20f;
    private int dodgeDurationMs = 300;
    
    public PlayerDodgeState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        HandleAnimatorParameters();
    }

    public override void EnterState()
    {
        _turnTime = 0f;
        HandleRotation();
        _ctx.CanDodge = false;
        Debug.Log("Dodging");
        HandleDodgeDurationAsync();
    }
    

    public sealed override void HandleAnimatorParameters()
    {
        _ctx.Animator.ResetTrigger(_ctx.HasDodgedHash);
        _ctx.Animator.SetTrigger(_ctx.HasDodgedHash);
        _ctx.Animator.SetBool(_ctx.IsGroundedHash, true);
    }

    public override void UpdateState()
    {
        HandleDodgeMove();
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
        }
    }

    private async void HandleDodgeDurationAsync()
    {
        HandleDodgeCooldownAsync();
        await Task.Delay(dodgeDurationMs);
        SwitchState(_ctx.IsSprintPressed ? _factory.Sprint() : _factory.Grounded());
    }
    
    private async void HandleDodgeCooldownAsync()
    {
        await Task.Delay(dodgeDurationMs + _ctx.DodgeCooldownMs);
        while (_ctx.IsDodgePressed)
        {
            await Task.Yield();
        }

        _ctx.CanDodge = true;
    }

    private void HandleDodgeMove()
    {
        _ctx.AppliedMovement = new Vector3(_ctx.transform.forward.x * DodgeSpeed, _ctx.BaseGravity, _ctx.transform.forward.z * DodgeSpeed);
        
        _ctx.CC.Move(_ctx.AppliedMovement * Time.deltaTime);
    }
}
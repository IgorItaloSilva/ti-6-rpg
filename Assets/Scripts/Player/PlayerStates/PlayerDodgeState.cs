using System.Threading.Tasks;
using UnityEngine;

public class PlayerDodgeState : PlayerBaseState
{
    protected const float DodgeSpeed = 30f;
    private int dodgeDurationMs = 200;
    public PlayerDodgeState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
    }

    public override void EnterState()
    {
        _ctx.CanDodge = false;
        Debug.Log("Dodging");
        _ctx.TurnTime = float.MaxValue;
        HandleDodgeDurationAsync();
    }

    public override void UpdateState()
    {
        HandleDodgeMove();
        HandleGravity();
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
        SwitchState(_factory.Grounded());
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
        _ctx.AppliedMovement = new Vector3(_ctx.transform.forward.x, _ctx.AppliedMovementY, _ctx.transform.forward.z);
        
        _ctx.CC.Move(_ctx.AppliedMovement * (DodgeSpeed * Time.deltaTime));
    }
    private void HandleGravity()
    {
        _ctx.CurrentMovementY = _ctx.BaseGravity;
    }

    public override void InitializeSubState()
    {
        
    }
}
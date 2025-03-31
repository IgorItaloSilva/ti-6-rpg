using System.Threading.Tasks;
using UnityEngine;

public class PlayerLockedState : PlayerBaseState
{
    private readonly int _lockDurationMs;
    
    public PlayerLockedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory, int lockDurationMs = 1667) : base(
        currentContext, playerStateFactory)
    {
        this._lockDurationMs = lockDurationMs;
        _ctx.ShouldLock = false;
    }

    public override void EnterState()
    {
        if(_ctx.ShowDebugLogs) Debug.Log("Locked");
        _turnTime = int.MaxValue;
        HandleLockDurationAsync();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {

    }

    private async void HandleLockDurationAsync()
    {
        await Task.Delay(_lockDurationMs);
        SwitchState(_factory.Grounded());
    }
}
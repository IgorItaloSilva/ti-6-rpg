using System.Threading.Tasks;
using UnityEngine;

public class PlayerLockedState : PlayerBaseState
{
    
    public PlayerLockedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory, int lockDurationMs) : base(
        currentContext, playerStateFactory)
    {
        HandleLockDurationAsync(lockDurationMs);
    }

    public override void EnterState()
    {
        if(_ctx.ShowDebugLogs) Debug.Log("Locked");
        _turnTime = int.MaxValue;
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

    private async void HandleLockDurationAsync(int _lockDurationMs)
    {
        await Task.Delay(_lockDurationMs);
        SwitchState(_factory.Grounded());
        _ctx.IsLocked = false;
    }
}
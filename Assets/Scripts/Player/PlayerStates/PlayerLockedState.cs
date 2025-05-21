using System.Threading.Tasks;
using UnityEngine;

public class PlayerLockedState : PlayerBaseState
{
    
    public PlayerLockedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
    }

    public override void EnterState()
    {
        if(_ctx.ShowDebugLogs) Debug.Log("Locked");
        _turnTime = int.MaxValue;
        _ctx.Acceleration = 0f;
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
}
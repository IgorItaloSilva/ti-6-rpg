using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private const byte AttackTurnTimeModifier = 2;
    private new const byte DecelerationSpeed = 10;
    
    public PlayerAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        _turnTime = _ctx.BaseTurnTime * 2;
    }

    public override void EnterState()
    {
        if(_ctx.ShowDebugLogs) Debug.Log("Attacking!");
        _ctx.HandleAttack();
    }
    
    public override void UpdateState()
    {
        
        HandleRotation();
        HandleMove();
        if (_ctx.IsAttackPressed)
        {
            _ctx.HandleAttack();
            return;
        }
        
        CheckSwitchStates();
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        if (_ctx.AttackCount is 0)
        {
            if (_ctx.IsSprintPressed)
                SwitchState(_factory.Sprint());
            else
                SwitchState(_factory.Grounded());
        }
    }

    protected override void HandleAcceleration()
    {
        _ctx.Acceleration -= (Time.fixedDeltaTime * DecelerationSpeed);
        _ctx.Acceleration = Mathf.Clamp(_ctx.Acceleration, 0, float.MaxValue);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    
    public PlayerAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Attacking!");
        _ctx.HandleAttack();
    }

    public override void UpdateState()
    {
        if(_ctx.IsAttackPressed)
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
        if(_ctx.AttackCount == 0)
        {
            SwitchState(_factory.Grounded());
        }
    }
}
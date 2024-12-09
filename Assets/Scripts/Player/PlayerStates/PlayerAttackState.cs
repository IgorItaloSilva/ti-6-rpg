using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private Transform targetTransform;

    public PlayerAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        _turnTime = 0f;
        HandleRotation();
        _turnTime = _ctx.BaseTurnTime * _ctx.AttackTurnTimeModifier;
    }

    public override void EnterState()
    {
        Debug.Log("Attacking!");
        _ctx.HandleAttack();
    }

    public sealed override void HandleAnimatorParameters()
    {
    }


    public override void UpdateState()
    {
        HandleRotation();
        if (_ctx.IsAttackPressed)
        {
            _ctx.HandleAttack();
            return;
        }

        if (targetTransform) HandleRotation();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        if (_ctx.AttackCount == 0)
        {
            if (_ctx.IsSprintPressed)
                SwitchState(_factory.Sprint());
            else
                SwitchState(_factory.Grounded());
        }
    }
}

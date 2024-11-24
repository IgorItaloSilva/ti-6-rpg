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
        if (Physics.SphereCast(_ctx.transform.position, 5f, _ctx.transform.forward, out var hit))
        {
            Debug.Log(hit.collider.gameObject.name);
        }
    }

    public override void EnterState()
    {
        Debug.Log("Attacking!");
        _turnTime = 0f;
        _ctx.HandleAttack();
    }

    public sealed override void HandleAnimatorParameters()
    {
    }


    public override void UpdateState()
    {
        if (_ctx.IsAttackPressed)
        {
            _ctx.HandleAttack();
            return;
        }

        if (targetTransform) HandleRotation();
        CheckSwitchStates();
    }

    private new void HandleRotation()
    {
        // Calcular direção resultante do input do player e rotacionar ele na direção para onde está indo.
        var turnOrientation = Mathf.Atan2(targetTransform.position.x, targetTransform.position.y) * Mathf.Rad2Deg;
        var smoothedTurnOrientation =
            Mathf.SmoothDampAngle(_ctx.transform.eulerAngles.y, turnOrientation, ref _turnSmoothSpeed, _turnTime);

        if (!_ctx.IsMovementPressed) return;

        // Aplicar movimentação baseado na rotação do 
        var groundedMovement = Quaternion.Euler(0f, turnOrientation, 0f) * Vector3.forward;

        _ctx.CurrentMovement = new Vector3(groundedMovement.x, _ctx.CurrentMovement.y, groundedMovement.z);

        // Rotacionar a direção do player
        _ctx.transform.rotation = Quaternion.Euler(0f, smoothedTurnOrientation, 0f);
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
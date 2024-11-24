using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerBaseState
{
    protected PlayerStateMachine _ctx;
    protected PlayerStateFactory _factory;
    protected float _turnTime, _turnSmoothSpeed;

    public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    {
        _ctx = currentContext;
        _factory = playerStateFactory;
    }

    public abstract void HandleAnimatorParameters();
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();

    protected void HandleRotation()
    {
        // Calcular direção resultante do input do player e rotacionar ele na direção para onde está indo.
        var turnOrientation = Mathf.Atan2(_ctx.CurrentMovementInput.x, _ctx.CurrentMovementInput.y) * Mathf.Rad2Deg +
                              _ctx.MainCam.transform.eulerAngles.y;
        var smoothedTurnOrientation =
            Mathf.SmoothDampAngle(_ctx.transform.eulerAngles.y, turnOrientation, ref _turnSmoothSpeed, _turnTime);

        if (!_ctx.IsMovementPressed) return;

        // Aplicar movimentação baseado na rotação do 
        var groundedMovement = Quaternion.Euler(0f, turnOrientation, 0f) * Vector3.forward;

        _ctx.CurrentMovement = new Vector3(groundedMovement.x, _ctx.CurrentMovement.y, groundedMovement.z);

        // Rotacionar a direção do player
        _ctx.transform.rotation = Quaternion.Euler(0f, smoothedTurnOrientation, 0f);
    }

    protected void SwitchState(PlayerBaseState newState)
    {
        _ctx.CurrentState.ExitState();
        newState.EnterState();

        _ctx.CurrentState = newState;
    }
}
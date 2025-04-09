using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerBaseState
{
    protected readonly PlayerStateMachine _ctx;
    protected readonly PlayerStateFactory _factory;
    protected float _turnTime, _turnSmoothSpeed, _lowestAccelerationSpeed = float.MaxValue;
    protected readonly byte AccelerationSpeed = 3, DecelerationSpeed = 10;
    private Vector3 _appliedMovement;

    protected PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    {
        _ctx = currentContext;
        _factory = playerStateFactory;
    }

    public virtual void HandleAnimatorParameters(){}
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();

    public virtual void FixedUpdateState()
    {
        HandleAcceleration();
        if (_ctx.InCombat)
        {
            _ctx.Animator.SetBool(_ctx.InCombatHash, true);
        }
        else
        {
            _ctx.Animator.SetBool(_ctx.InCombatHash, false);
        }
    }
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
    protected virtual void HandleJump(float jumpForceOverride = 1f)
    {
        _ctx.Animator.ResetTrigger(_ctx.HasJumpedHash);
        _ctx.Animator.SetTrigger(_ctx.HasJumpedHash);
        _ctx.CanJump = false;
        if (!_ctx.IsMovementPressed)
            _ctx.AppliedMovement = Vector3.zero;
        _ctx.CurrentMovementY = _ctx.InitialJumpVelocity * jumpForceOverride;
        _ctx.AppliedMovementY = _ctx.InitialJumpVelocity * jumpForceOverride;
    }
    protected virtual void HandleAcceleration()
    {
        _lowestAccelerationSpeed = Mathf.Min(_ctx.Acceleration, _lowestAccelerationSpeed);
        
        if (_ctx.IsMovementPressed && _ctx.Acceleration <= 1)
        {
            _ctx.Acceleration += Time.fixedDeltaTime * AccelerationSpeed;
        }
        else
        {
            _ctx.Acceleration -= Time.fixedDeltaTime * DecelerationSpeed;
        }

        if (_lowestAccelerationSpeed < 1)
        {
            _ctx.Acceleration = Mathf.Clamp(_ctx.Acceleration, 0, 1);
        }

        _ctx.Animator.SetFloat(_ctx.PlayerVelocityYHash, _ctx.Acceleration);
        _ctx.Animator.SetFloat(_ctx.PlayerVelocityXHash, _ctx.Acceleration);
    }
    
    protected void HandleMove()
    {
        _appliedMovement.x = _ctx.CurrentMovement.x * _ctx.Acceleration;
        _appliedMovement.y = _ctx.BaseGravity;
        _appliedMovement.z = _ctx.CurrentMovement.z * _ctx.Acceleration;

        _ctx.AppliedMovement = _appliedMovement;
        _ctx.CC.Move(_ctx.AppliedMovement * (_ctx.BaseMoveSpeed * Time.deltaTime ));
    }

    protected void SwitchState(PlayerBaseState newState)
    {
        _ctx.CurrentState.ExitState();
        newState.EnterState();

        _ctx.CurrentState = newState;
    }

    public void LockPlayer(int durationMs)
    {
        SwitchState(_factory.Locked(durationMs));
    }
}
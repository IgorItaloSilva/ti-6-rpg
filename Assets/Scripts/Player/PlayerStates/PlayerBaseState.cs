using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerBaseState
{
    protected readonly PlayerStateMachine _ctx;
    protected readonly PlayerStateFactory _factory;
    protected float _turnTime, _turnSmoothSpeed, _lowestAccelerationSpeed = float.MaxValue;
    protected float _maxAcceleration;
    private const byte RotationSpeed = 5;
    protected byte _accelerationSpeed = 3, _decelerationSpeed = 10;
    private Vector3 _appliedMovement;
    protected Vector3 _cameraForward, _cameraRight;
    protected Vector3 _targetDirection;

    protected PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    {
        _maxAcceleration = 2f;
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
        
        if ((_ctx.IsMovementPressed && _ctx.Acceleration <= _maxAcceleration) || _ctx.Acceleration < 0f)
        {
            _ctx.Acceleration += Time.fixedDeltaTime * _accelerationSpeed;
        }
        else
        {
            _ctx.Acceleration -= Time.fixedDeltaTime * _decelerationSpeed;
        }

        if (_lowestAccelerationSpeed < _maxAcceleration)
        {
            _ctx.Acceleration = Mathf.Clamp(_ctx.Acceleration, 0, _maxAcceleration);
        }

    }
    protected void HandleTargetedRotation()
    {
        if (_ctx.InCombat)
            _targetDirection = _ctx.EnemyDetector.targetEnemy.transform.position - _ctx.transform.position;
        else
            _targetDirection = _ctx.MainCam.transform.forward;
        
        _targetDirection.y = 0; // Keep rotation only on the Y-axis if needed
    
        _ctx.transform.rotation = Quaternion.Slerp(_ctx.transform.rotation, Quaternion.LookRotation(_targetDirection),
            Time.deltaTime * RotationSpeed);
    }
    
    protected void HandleRotation()
    {
        // Calculate the resulting direction from player input and rotate the player towards it.
        float turnOrientation = Mathf.Atan2(_ctx.CurrentMovementInput.x, _ctx.CurrentMovementInput.y) * Mathf.Rad2Deg + _ctx.MainCam.transform.eulerAngles.y;
        float smoothedTurnOrientation =
            Mathf.SmoothDampAngle(_ctx.transform.eulerAngles.y, turnOrientation, ref _turnSmoothSpeed, _turnTime);
    
        if (!_ctx.IsMovementPressed) return;
    
        // Apply movement based on the player's rotation
        var groundedMovement = Quaternion.Euler(0f, turnOrientation, 0f) * Vector3.forward;
    
        // Ensure consistent movement direction when switching between rotation modes
        float forward = groundedMovement.z;
        float right = groundedMovement.x;
        _ctx.CurrentMovement = new Vector3(right, _ctx.CurrentMovement.y, forward);
    
        // Rotate the player's direction
        _ctx.transform.rotation = Quaternion.Euler(0f, smoothedTurnOrientation, 0f);
    }
    
    protected void HandlePotion()
    {
        if (_ctx.IsPotionPressed)
        {
            _ctx.CanHeal = false;
            _ctx.Animator.ResetTrigger(_ctx.HasHealedHash);
            _ctx.Animator.SetTrigger(_ctx.HasHealedHash);
        }
    }
    
    protected virtual void HandleMove()
    {
        _appliedMovement.x = _ctx.CurrentMovement.x * _ctx.Acceleration;
        _appliedMovement.y = _ctx.BaseGravity;
        _appliedMovement.z = _ctx.CurrentMovement.z * _ctx.Acceleration;

        _ctx.AppliedMovement = _appliedMovement;
        _ctx.CC.Move(_ctx.AppliedMovement * (_ctx.BaseMoveSpeed * Time.deltaTime ));
        
        _ctx.Animator.SetFloat(_ctx.PlayerVelocityYHash, _ctx.Acceleration * _ctx.CurrentMovementInput.magnitude, 0.1f, Time.deltaTime);
        _ctx.Animator.SetFloat(_ctx.PlayerVelocityXHash, 0f);
    }
    
protected virtual void HandleTargetedMove()
{
    _cameraForward = _ctx.MainCam.transform.forward;
    _cameraRight = _ctx.MainCam.transform.right;

    // Flatten the camera directions to ignore vertical movement
    _cameraForward.y = 0;
    _cameraRight.y = 0;
    _cameraForward.Normalize();
    _cameraRight.Normalize();

    // Calculate desired movement in world space
    Vector3 desiredMovement = (_cameraForward * _ctx.CurrentMovement.z + _cameraRight * _ctx.CurrentMovement.x) * _ctx.Acceleration;
    desiredMovement.y = _ctx.BaseGravity;

    _appliedMovement = Vector3.Lerp(_appliedMovement, desiredMovement, .15f);

    _ctx.AppliedMovement = _appliedMovement;
    _ctx.CC.Move(_ctx.AppliedMovement * (_ctx.BaseMoveSpeed * Time.deltaTime));

    _ctx.Animator.SetFloat(_ctx.PlayerVelocityXHash, _ctx.transform.InverseTransformDirection(_appliedMovement).x, 0.15f, Time.deltaTime);
    _ctx.Animator.SetFloat(_ctx.PlayerVelocityYHash, _ctx.transform.InverseTransformDirection(_appliedMovement).z, 0.15f, Time.deltaTime);
}
    protected virtual void HandleForwardMove()
    {
        _cameraForward = _ctx.MainCam.transform.forward;
        _cameraRight = _ctx.MainCam.transform.right;
        
        // Flatten the camera directions to ignore vertical movement
        _cameraForward.y = 0;
        _cameraRight.y = 0;
        _cameraForward.Normalize();
        _cameraRight.Normalize();
        
        _ctx.AppliedMovementX = _ctx.transform.forward.x * _ctx.BaseMoveSpeed * _ctx.Acceleration;
        _ctx.AppliedMovementY = _ctx.BaseGravity;
        _ctx.AppliedMovementZ = _ctx.transform.forward.z * _ctx.BaseMoveSpeed * _ctx.Acceleration;

        _ctx.CC.Move(_ctx.AppliedMovement * Time.deltaTime);
        
        _ctx.Animator.SetFloat(_ctx.PlayerVelocityYHash, _ctx.Acceleration * _ctx.CurrentMovementInput.magnitude, 0.1f, Time.deltaTime);
        _ctx.Animator.SetFloat(_ctx.PlayerVelocityXHash, 0f, 0.1f, Time.deltaTime);
    }

    protected void SwitchState(PlayerBaseState newState)
    {
        _ctx.CurrentState.ExitState();
        newState.EnterState();

        _ctx.CurrentState = newState;
    }

    public void LockPlayer()
    {
        SwitchState(_factory.Locked());
    }
    
    public void UnlockPlayer()
    {
        SwitchState(_ctx.InCombat? _factory.Combat() : _factory.Grounded());
    }
    
}
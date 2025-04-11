using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerCombatState : PlayerGroundedState
{
    private Vector3 _appliedMovement;
    public Vector3 _lookDirection;
    private const byte RotationSpeed = 3;

    public PlayerCombatState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        
    }
    
    public override void EnterState()
    {
        base.EnterState();
    }
    
    public override void UpdateState()
    {
        HandleRotation();
        HandleMove();
        CheckSwitchStates();
    }
    
    protected override void HandleMove()
    {
        Vector3 cameraForward = _ctx.MainCam.transform.forward;
        Vector3 cameraRight = _ctx.MainCam.transform.right;
        
        // Flatten the vectors to ignore vertical movement
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();
        
        _appliedMovement = (cameraForward * _ctx.CurrentMovement.z + cameraRight * _ctx.CurrentMovement.x) * _ctx.Acceleration;
        _appliedMovement.y = _ctx.BaseGravity;
        
        _ctx.AppliedMovement = _appliedMovement;
        _ctx.CC.Move(_ctx.AppliedMovement * (_ctx.BaseMoveSpeed * Time.deltaTime));
    }
    
    protected new void HandleRotation()
    {
        _lookDirection = _ctx.MainCam.transform.forward;
        _lookDirection.y = 0;
        
        _ctx.transform.rotation = Quaternion.Slerp(_ctx.transform.rotation, Quaternion.LookRotation(_lookDirection),
            Time.deltaTime * RotationSpeed);
    }
    
    protected override void HandleAcceleration()
    {
        _lowestAccelerationSpeed = Mathf.Min(_ctx.Acceleration, _lowestAccelerationSpeed);
        
        if (_ctx.IsMovementPressed && _ctx.Acceleration <= 1f)
        {
            _ctx.Acceleration += Time.fixedDeltaTime * AccelerationSpeed;
        }
        else
        {
            _ctx.Acceleration -= Time.fixedDeltaTime * DecelerationSpeed;
        }

        if (_lowestAccelerationSpeed < 1)
        {
            _ctx.Acceleration = Mathf.Clamp(_ctx.Acceleration, 0, 1f);
        }

        _ctx.Animator.SetFloat(_ctx.PlayerVelocityXHash, _ctx.Acceleration * _ctx.CurrentMovement.x);
        _ctx.Animator.SetFloat(_ctx.PlayerVelocityYHash, _ctx.Acceleration * _ctx.CurrentMovement.y);
    }

    public override void CheckSwitchStates()
    {
        if (!_ctx.IsOnTarget)
        {
            SwitchState(_factory.Grounded());
        }
        
        if (!_ctx.CC.isGrounded)
        {
            _ctx.Animator.SetBool(_ctx.InCombatHash, false);
            SwitchState(_factory.InAir());
            return;
        }

        if (_ctx.IsJumpPressed)
        {
            _ctx.Animator.SetBool(_ctx.InCombatHash, false);
            HandleJump();
            SwitchState(_factory.InAir());
        }

        if (_ctx.IsSprintPressed && _ctx.IsMovementPressed)
        {
            SwitchState(_factory.Sprint());
        }

        if (_ctx.IsDodgePressed && _ctx.IsMovementPressed)
        {
            SwitchState(_factory.Dodge());
        }

        if (_ctx.IsAttackPressed)
        {
            SwitchState(_factory.Attack());
        }

        if (_ctx.IsClimbing && _ctx.CanMount)
        {
            _ctx.Animator.SetBool(_ctx.InCombatHash, false);
            SwitchState(_factory.Climb());
        }
    }

}
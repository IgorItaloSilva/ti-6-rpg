using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerAttackState : PlayerBaseState
{
    private const byte DecelerationSpeed = 2;
    private bool _inSeekRange;

    public PlayerAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        _turnTime = _ctx.BaseTurnTime * 2;
        _maxAcceleration = 1f;
    }

    public override void EnterState()
    {
        if (_ctx.ShowDebugLogs) Debug.Log("Attacking!");
        CheckAttackTargetDistance();
    }

    public override void UpdateState()
    {
        if (!_inSeekRange)
            HandleRotation();
        
        HandleForwardMove();
        if (_ctx.IsAttackPressed)
        {
            _ctx.HandleAttack();
            return;
        }

        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {
        CheckAttackTargetDistance();

        HandleAcceleration();

        HandleTargetedRotation();
    }

    public override void ExitState()
    {
        
    }

    private void CheckAttackTargetDistance()
    {
        _inSeekRange = ((_ctx.InCombat) && Vector3.Distance(_ctx.transform.position, _ctx.EnemyDetector.targetEnemy.transform.position) <= 3f);
    }

    protected override void HandleAcceleration()
    {
        if (_ctx.Acceleration > 0) _ctx.Acceleration -= Time.fixedDeltaTime * DecelerationSpeed;
        else _ctx.Acceleration = 0;

        _ctx.Animator.SetFloat(_ctx.PlayerVelocityYHash, Mathf.Max(_ctx.Acceleration,1f), 0.1f, Time.deltaTime);
    }
    
    protected override void HandleForwardMove()
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
        
    }

    public override void CheckSwitchStates()
    {
        if (_ctx.IsDodgePressed)
        {
            _ctx.ResetAttacks();
            SwitchState(_factory.Dodge());
        }

        if (_ctx.AttackCount == 0)
        {
            SwitchState(_factory.Grounded());
        }
    }
}
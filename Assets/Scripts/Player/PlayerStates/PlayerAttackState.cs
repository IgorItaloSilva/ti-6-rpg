using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerAttackState : PlayerBaseState
{
    private new const byte DecelerationSpeed = 5;
    private bool _inSeekRange;

    public PlayerAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        _turnTime = _ctx.BaseTurnTime * 2;
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
        
        _ctx.Animator.SetFloat(_ctx.PlayerVelocityYHash, _ctx.Acceleration * 5);
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

        if (_ctx.IsBlocking && _ctx.SwordWeaponManager.DamageCollider.enabled == false)
        {
            _ctx.ResetAttacks();
            SwitchState(_factory.Block());
        }
        
        if (_ctx.AttackCount is 0 || !_ctx.InCombat)
        {
            SwitchState(_ctx.InCombat ? _factory.Combat() : _factory.Grounded());
        }
    }
}
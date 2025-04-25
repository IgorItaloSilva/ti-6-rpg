using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private new const byte DecelerationSpeed = 5;
    public Vector3 _attackDirection;
    private const byte RotationSpeed = 5;
    private bool _hasTarget;

    public PlayerAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        _turnTime = _ctx.BaseTurnTime * 2;
    }

    public override void EnterState()
    {
        _ctx.Animator.SetBool(_ctx.InCombatHash, true);
        _ctx.InCombat = true;
        if (_ctx.ShowDebugLogs) Debug.Log("Attacking!");
        CheckTarget();
    }

    public override void UpdateState()
    {
        if (!_hasTarget)
            HandleRotation();

        HandleMove();
        if (_ctx.IsAttackPressed)
        {
            _ctx.HandleAttack();
            return;
        }

        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {
        CheckTarget();

        HandleAcceleration();

        if (_hasTarget)
            HandleAttackRotation();
    }

    public override void ExitState()
    {
        if (!_ctx.EnemyDetector.targetEnemy)
            _ctx.InCombat = false;
    }

    private void HandleAttackRotation()
    {
        _attackDirection = _ctx.EnemyDetector.targetEnemy.transform.position - _ctx.transform.position;
        _attackDirection.y = 0; // Keep rotation only on the Y-axis if needed

        _ctx.transform.rotation = Quaternion.Slerp(_ctx.transform.rotation, Quaternion.LookRotation(_attackDirection),
            Time.deltaTime * RotationSpeed);
    }
    
    protected override void HandleMove()
    {
        _ctx.AppliedMovement = new Vector3(_ctx.transform.forward.x * _ctx.BaseMoveSpeed * _ctx.Acceleration, _ctx.BaseGravity,
            _ctx.transform.forward.z * _ctx.BaseMoveSpeed * _ctx.Acceleration);

        _ctx.CC.Move(_ctx.AppliedMovement * Time.deltaTime);
    }

    private void CheckTarget()
    {
        _hasTarget = ((_ctx.IsOnTarget || _ctx.EnemyDetector.targetEnemy) &&
                      Vector3.Distance(_ctx.transform.position, _ctx.EnemyDetector.targetEnemy.transform.position) <=
                      3f);
    }

    protected override void HandleAcceleration()
    { 
        if(_ctx.Acceleration > 0)
            _ctx.Acceleration -= Time.fixedDeltaTime * DecelerationSpeed;
        else
            _ctx.Acceleration = 0;
        
        _ctx.Animator.SetFloat(_ctx.PlayerVelocityYHash, _ctx.Acceleration);
        //_ctx.Animator.SetFloat(_ctx.PlayerVelocityXHash, _ctx.Acceleration);
    }

    public override void CheckSwitchStates()
    {
        if (_ctx.AttackCount is 0 || !_ctx.InCombat)
        {
            if (_ctx.IsSprintPressed)
                SwitchState(_factory.Sprint());
            else
                SwitchState(_factory.Grounded());
        }

        if (_ctx.IsDodgePressed)
        {
            _ctx.ResetAttacks();
            SwitchState(_factory.Dodge());
        }
    }
}
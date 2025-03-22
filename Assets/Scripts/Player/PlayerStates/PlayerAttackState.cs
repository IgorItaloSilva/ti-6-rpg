using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private const byte AttackTurnTimeModifier = 2;
    private Vector3 _appliedMovement, _cameraForward, _cameraRight, _attackDirection;
    private const byte RotationSpeed = 3;
    private bool _hasTarget;
    
    public PlayerAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        _turnTime = _ctx.BaseTurnTime * AttackTurnTimeModifier;
    }

    public override void EnterState()
    {
        if(_ctx.ShowDebugLogs) Debug.Log("Attacking!");
        CheckTarget();
        _ctx.HandleAttack();
    }
    
    public override void UpdateState()
    {
        if(_hasTarget)
            HandleAttackRotation();
        else
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
    }

    public override void ExitState()
    {
        
    }

    private void HandleAttackRotation()
    {
        _attackDirection = _ctx.EnemyDetector.targetEnemy.transform.position - _ctx.transform.position;
        _attackDirection.y = 0; // Keep rotation only on the Y-axis if needed
        
        _ctx.transform.rotation = Quaternion.Slerp(_ctx.transform.rotation, Quaternion.LookRotation(_attackDirection), Time.deltaTime * RotationSpeed);
    }

    private void CheckTarget()
    {
        _hasTarget = ((_ctx.IsOnTarget || _ctx.EnemyDetector.targetEnemy) &&
                     Vector3.Distance(_ctx.transform.position, _ctx.EnemyDetector.targetEnemy.transform.position) <=
                     3f);
    }

    public override void CheckSwitchStates()
    {
        if (_ctx.AttackCount is 0)
        {
            if (_ctx.IsSprintPressed)
                SwitchState(_factory.Sprint());
            else if(_hasTarget)
                SwitchState(_factory.Combat());
            else
                SwitchState(_factory.Grounded());
        }
    }
    
    protected override void HandleMove()
    {
        _ctx.AppliedMovement = new Vector3(_ctx.transform.forward.x * _ctx.BaseMoveSpeed * _ctx.Acceleration, _ctx.BaseGravity,
            _ctx.transform.forward.z * _ctx.BaseMoveSpeed * _ctx.Acceleration);

        _ctx.CC.Move(_ctx.AppliedMovement * Time.deltaTime);
    }
}

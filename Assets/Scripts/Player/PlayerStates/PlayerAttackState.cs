using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private const byte AttackTurnTimeModifier = 2;
    private new const byte DecelerationSpeed = 10, DodgeDecelerationSpeed = 5;
    public Vector3 _attackDirection;
    private const byte RotationSpeed = 3;
    private bool _hasTarget;
    private readonly bool _dodgeAttack;
    
    public PlayerAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory, bool dodgeAttack = false) : base(
        currentContext, playerStateFactory)
    {
        _turnTime = _ctx.BaseTurnTime * 2;
        //_dodgeAttack = dodgeAttack;
    }

    public override void EnterState()
    {
        if(_ctx.ShowDebugLogs) Debug.Log("Attacking!");
        CheckTarget();
        if(_dodgeAttack) _ctx.HandleAttack(_dodgeAttack);
    }
    
    public override void UpdateState()
    {
        if(!_hasTarget)
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
        
        if(_hasTarget) 
            HandleAttackRotation();
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
            else
                SwitchState(_factory.Grounded());
        }

        if (_ctx.IsDodgePressed && !_dodgeAttack)
        {
            _ctx.ResetAttacks();
            SwitchState(_factory.Dodge());
        }
    }
}

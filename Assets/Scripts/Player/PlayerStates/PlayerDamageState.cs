using System.Threading.Tasks;
using Player.PlayerStates;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerDamageState : PlayerCombatState
{
    private readonly Vector3 _knockbackDir;
    public PlayerDamageState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        _maxAcceleration = 1.5f;
        _ctx.Acceleration = _ctx.IsBlocking ? 0.75f : 1.5f;
        _ctx.AppliedMovementY = _ctx.BaseGravity;
        _ctx.CurrentMovement = _ctx.CurrentMovementInput;
        _ctx.CurrentMovementZ = _ctx.CurrentMovementInput.y;
        _turnTime = _ctx.BaseTurnTime * 5;
        if (_ctx.EnemyDetector.targetEnemy)
            _knockbackDir = (_ctx.transform.position - _ctx.EnemyDetector.targetEnemy.transform.position).normalized;
        else
            _knockbackDir = -_ctx.transform.forward;
    }

    public override void EnterState()
    {
        HandleAnimatorParameters();
        if (_ctx.ShowDebugLogs) Debug.Log("Staggered");
    }

    public override void ExitState()
    {
        base.ExitState();
            _ctx.Animator.SetBool(_ctx.IsBlockingHash, _ctx.IsBlockPressed);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        
        if (!_ctx.IsBlockPressed)
        {
            _ctx.Animator.SetBool(_ctx.IsBlockingHash, false);
        }
    }

    public override void FixedUpdateState()
    {
        Knockback();
        HandleAcceleration();
    }

    protected override void HandleAcceleration()
    {
        if (_ctx.Acceleration > 0f)
            _ctx.Acceleration -= Time.fixedDeltaTime * 3;
        else
            _ctx.Acceleration = 0f;
    }

    private void Knockback()
    {
        _ctx.AppliedMovementX = _knockbackDir.x * _ctx.BaseMoveSpeed * _ctx.Acceleration;
        _ctx.AppliedMovementY = _ctx.BaseGravity;
        _ctx.AppliedMovementZ = _knockbackDir.z * _ctx.BaseMoveSpeed * _ctx.Acceleration;

        _ctx.CC.Move(_ctx.AppliedMovement * Time.fixedDeltaTime);

        _ctx.Animator.SetFloat(_ctx.PlayerVelocityXHash, _knockbackDir.x * 2, 0.1f, Time.fixedDeltaTime);
        _ctx.Animator.SetFloat(_ctx.PlayerVelocityYHash, _knockbackDir.z * 2, 0.1f, Time.fixedDeltaTime);
    }

    public override void CheckSwitchStates()
    {
        if (!_ctx.CC.isGrounded)
        {
            _ctx.Animator.SetBool(_ctx.InCombatHash, false);
            SwitchState(_factory.InAir());
        }
    }
}
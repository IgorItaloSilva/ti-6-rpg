using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerSlowGroundedState : PlayerGroundedState
{
    private new const float MaxAcceleration = 1f;

    public PlayerSlowGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
    }
    
    public override void EnterState()
    {
        if(_ctx.ShowDebugLogs) Debug.Log("SlowGrounded");
        _turnTime = _ctx.BaseTurnTime * 2;
    }
    
    
    protected override void HandleAcceleration()
    {
        _lowestAccelerationSpeed = Mathf.Min(_ctx.Acceleration, _lowestAccelerationSpeed);
        
        if (_ctx.IsMovementPressed && _ctx.Acceleration <= MaxAcceleration)
        {
            _ctx.Acceleration += Time.fixedDeltaTime * AccelerationSpeed;
        }
        else
        {
            _ctx.Acceleration -= Time.fixedDeltaTime * DecelerationSpeed;
        }

        if (_lowestAccelerationSpeed < 1)
        {
            _ctx.Acceleration = Mathf.Clamp(_ctx.Acceleration, 0, MaxAcceleration);
        }

        _ctx.Animator.SetFloat(_ctx.PlayerVelocityYHash, _ctx.Acceleration);
        _ctx.Animator.SetFloat(_ctx.PlayerVelocityXHash, _ctx.Acceleration);
    }

    public override void CheckSwitchStates()
    {
        if (!_ctx.CC.isGrounded)
        {
            _ctx.Animator.SetBool(_ctx.InCombatHash, false);
            SwitchState(_factory.InAir());
            return;
        }
        
        if(!_ctx.IsBlocking)
        {
            _ctx.Animator.SetBool(_ctx.IsBlockingHash, false);
            SwitchState(_factory.Grounded());
            return;
        }
    }

}

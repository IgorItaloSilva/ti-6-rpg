using UnityEngine;

public class PlayerInAirState : PlayerBaseState
{
    private readonly bool _shouldRotate;
    private const byte AirSpeed = 8;

    public PlayerInAirState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory, bool shouldRotate) : base(
        currentContext, playerStateFactory)
    {        
        HandleAirGravity();
        HandleAnimatorParameters();
        _shouldRotate = shouldRotate;
    }

    public override void EnterState()
    {
        if(_ctx.ShowDebugLogs) Debug.Log("In Air");
        _turnTime = _ctx.BaseTurnTime * _ctx.SlowTurnTimeModifier;
    }

    public sealed override void HandleAnimatorParameters()
    {
        _ctx.Animator.SetBool(_ctx.IsGroundedHash, false);
    }

    public override void UpdateState()
    {
        if(_shouldRotate)
            HandleRotation();
        
        HandleAirMove();
        HandleAirGravity();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        if (_ctx.CC.isGrounded)
        {
            SwitchState(_ctx.IsSprintPressed ? _factory.Sprint() : _factory.Grounded());
        }

        if (_ctx.IsClimbing && _ctx.CanMount)
        {
            SwitchState(_factory.Climb());
        }
            
    }

    private void HandleAirGravity()
    {
        var previousYVelocity = _ctx.CurrentMovement.y;
        _ctx.CurrentMovementY += (_ctx.Gravity * Time.deltaTime);
        _ctx.AppliedMovementY = previousYVelocity + _ctx.CurrentMovement.y;
    }

    private void HandleAirMove()
    {
        _ctx.AppliedMovement = new Vector3(
            _ctx.transform.forward.x * AirSpeed * _ctx.Acceleration,
            _ctx.AppliedMovementY,
            _ctx.transform.forward.z * AirSpeed * _ctx.Acceleration);

        _ctx.CC.Move(_ctx.AppliedMovement * Time.deltaTime);
    }
}
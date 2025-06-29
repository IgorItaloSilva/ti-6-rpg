using UnityEngine;

public class PlayerInAirState : PlayerBaseState
{
    private readonly bool _shouldRotate;
    private const byte AirSpeed = 8;
    private readonly float MaxAcceleration;

    public PlayerInAirState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory, bool shouldRotate) : base(
        currentContext, playerStateFactory)
    {        
        HandleAirGravity();
        HandleAnimatorParameters();
        _shouldRotate = shouldRotate;
        MaxAcceleration = Mathf.Min(_ctx.Acceleration,1.25f);
    }

    public override void EnterState()
    {
        if(_ctx.ShowDebugLogs) Debug.Log("In Air");
        _turnTime = 1f;
    }

    public sealed override void HandleAnimatorParameters()
    {
        if(Time.timeScale > 0f)
            _ctx.Animator.SetBool(_ctx.IsGroundedHash, false);
    }

    public override void UpdateState()
    {
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
            SwitchState(_factory.Grounded());
        }

        if (_ctx.IsClimbing && _ctx.CanMount)
        {
            SwitchState(_factory.Climb());
        }
            
    }
    protected override void HandleAcceleration()
    {
        _ctx.Acceleration = Mathf.Clamp(_ctx.Acceleration, 0, MaxAcceleration);
        
        _ctx.Animator.SetFloat(_ctx.PlayerVelocityYHash, _ctx.Acceleration * _ctx.CurrentMovementInput.magnitude * 1.5f);
        //_ctx.Animator.SetFloat(_ctx.PlayerVelocityXHash, _ctx.Acceleration);
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
            _ctx.AppliedMovement.y,
            _ctx.transform.forward.z * AirSpeed * _ctx.Acceleration);

        _ctx.CC.Move(_ctx.AppliedMovement * Time.deltaTime);
    }
}
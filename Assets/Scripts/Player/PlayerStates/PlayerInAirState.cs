using UnityEngine;

public class PlayerInAirState : PlayerBaseState
{
    protected const float MoveSpeed = 8f, SprintSpeed = 12f;
    private readonly float _currentMoveSpeed;

    public PlayerInAirState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory, float airMoveSpeedOverride) : base(
        currentContext, playerStateFactory)
    {
        HandleAnimatorParameters();
        if (airMoveSpeedOverride != 0)
            _currentMoveSpeed = airMoveSpeedOverride;
        else
            _currentMoveSpeed = _ctx.IsSprintPressed ? SprintSpeed : MoveSpeed;
    }

    public override void EnterState()
    {
        Debug.Log("In Air");
        _turnTime = _ctx.BaseTurnTime * _ctx.SlowTurnTimeModifier;
    }

    public sealed override void HandleAnimatorParameters()
    {
        _ctx.Animator.SetBool(_ctx.IsGroundedHash, false);
    }

    public override void UpdateState()
    {
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
        var previousYVelocity = _ctx.CurrentMovementY;
        _ctx.CurrentMovementY += (_ctx.Gravity * Time.deltaTime);
        _ctx.AppliedMovementY = previousYVelocity + _ctx.CurrentMovementY;
    }

    private void HandleAirMove()
    {
        _ctx.AppliedMovement = new Vector3(_ctx.transform.forward.x * _currentMoveSpeed * _ctx.CurrentMovementInput.magnitude, _ctx.AppliedMovementY * MoveSpeed, _ctx.transform.forward.z * _currentMoveSpeed * _ctx.CurrentMovementInput.magnitude);

        _ctx.CC.Move(_ctx.AppliedMovement * Time.deltaTime);
    }
}
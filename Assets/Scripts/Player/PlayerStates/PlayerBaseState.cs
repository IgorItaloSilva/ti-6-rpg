using UnityEngine.InputSystem;

public abstract class PlayerBaseState
{

    protected PlayerStateMachine _ctx;
    protected PlayerStateFactory _factory;

    public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    {
        _ctx = currentContext;
        _factory = playerStateFactory;
    }
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();

    protected void SwitchState(PlayerBaseState newState)
    {
        _ctx.CurrentState.ExitState();
        newState.EnterState();

        _ctx.CurrentState = newState;
    }
    
    protected void HandleJump()
    {
        _ctx.CanJump = false;
        _ctx.CurrentMovementY = _ctx.InitialJumpVelocity;
        _ctx.AppliedMovementY = _ctx.InitialJumpVelocity;
    }
}
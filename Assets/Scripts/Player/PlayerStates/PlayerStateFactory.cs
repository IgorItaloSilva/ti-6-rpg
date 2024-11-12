public class PlayerStateFactory
{
    private PlayerStateMachine _context;

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
    }

    public PlayerBaseState Idle()
    {
        return new PlayerIdleState(_context, this);
    }

    public PlayerBaseState Walk()
    {
        return new PlayerWalkState(_context, this);
    }

    public PlayerBaseState Sprint()
    {
        return new PlayerSprintState(_context, this);
    }
    
    public PlayerBaseState Dodge()
    {
        return new PlayerDodgeState(_context, this);
    }
    
    public PlayerBaseState Attack()
    {
        return new PlayerAttackState(_context, this);
    }

    public PlayerBaseState Grounded()
    {
        return new PlayerGroundedState(_context, this);
    }
    public PlayerBaseState InAir()
    {
        return new PlayerInAirState(_context, this);
    }

}
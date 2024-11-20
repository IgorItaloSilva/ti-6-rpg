using System.Numerics;
using Player.PlayerStates;

public class PlayerStateFactory
{
    private PlayerStateMachine _context;

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
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

    public PlayerBaseState Climb()
    {
        return new PlayerClimbState(_context, this);
    }

}
using System.Numerics;
using Player.PlayerStates;
using UnityEngine;

public class PlayerStateFactory
{
    private PlayerStateMachine _context;
    public Camera mainCam;

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

    public PlayerBaseState Dead()
    {
        return new PlayerDeadState(_context, this);
    }

    public PlayerBaseState Grounded()
    {
        return new PlayerGroundedState(_context, this);
    }
    public PlayerBaseState InAir(float airMoveSpeedOverride = 0f)
    {
        return new PlayerInAirState(_context, this, airMoveSpeedOverride);
    }

    public PlayerBaseState Climb()
    {
        return new PlayerClimbState(_context, this);
    }

}
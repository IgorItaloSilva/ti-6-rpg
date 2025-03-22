using System.Numerics;
using Player.PlayerStates;
using UnityEngine;
using UnityEngine.UIElements;

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
    public PlayerBaseState Combat()
    {
        return new PlayerCombatState(_context, this);
    }
    public PlayerBaseState InAir(bool shouldRotate = true)
    {
        return new PlayerInAirState(_context, this, shouldRotate);
    }

    public PlayerBaseState Climb()
    {
        return new PlayerClimbState(_context, this);
    }

}
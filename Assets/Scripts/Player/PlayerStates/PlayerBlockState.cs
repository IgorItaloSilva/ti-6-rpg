using UnityEngine;
using Debug = UnityEngine.Debug;
using System.Threading.Tasks;
using Player.PlayerStates;
using Task = System.Threading.Tasks.Task;

public class PlayerBlockState : PlayerCombatState
{
    private const float MaxAcceleration = 1f;
    private const byte ParryDurationMs = 150, ParryCooldownMs = 150;

    public PlayerBlockState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        _ctx.ShouldParry = true;
        HandleParryDurationAsync();
    }
    
    public override void EnterState()
    {
        if(_ctx.ShowDebugLogs) Debug.Log("Parrying");
        _turnTime = _ctx.BaseTurnTime * 2;
    }

    public override void ExitState()
    {

    }

    private async void HandleParryDurationAsync()
    {
        await Task.Delay(ParryDurationMs);
        _ctx.ShouldParry = false;
    }

    public override void CheckSwitchStates()
    {
        if (_ctx.PlayerStats.PlayerIsDead)
        {
            _ctx.Animator.SetBool(_ctx.IsBlockingHash, false);
            SwitchState(_factory.Dead());
            return;
        }

        if (!_ctx.InCombat)
        {
            _ctx.Animator.SetBool(_ctx.IsBlockingHash, false);
            SwitchState(_factory.Grounded());
        }
        
        if (!_ctx.CC.isGrounded)
        {
            _ctx.Animator.SetBool(_ctx.IsBlockingHash, false);
            SwitchState(_factory.InAir());
            return;
        }
        
        if(!_ctx.IsBlocking)
        {
            _ctx.Animator.SetBool(_ctx.IsBlockingHash, false);
            if(_ctx.InCombat)
                SwitchState(_factory.Combat());
            else
                SwitchState(_factory.Grounded());
            return;
        }
    }

}

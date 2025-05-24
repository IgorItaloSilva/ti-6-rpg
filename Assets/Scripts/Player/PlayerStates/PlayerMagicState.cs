using Debug = UnityEngine.Debug;
using Task = System.Threading.Tasks.Task;

public class PlayerMagicState : PlayerCombatState
{
    private readonly int TimeBetweenDamagesMs;
    public PlayerMagicState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        TimeBetweenDamagesMs = 500;
    }
    
    public override void EnterState()
    {
        HandleMagicDamageRateAsync();
        if(_ctx.ShowDebugLogs) Debug.Log("Casting Magic");
        _turnTime = _ctx.BaseTurnTime * 2;
    }

    public override void CheckSwitchStates()
    {
        if (!_ctx.CC.isGrounded)
        {
            _ctx.Animator.SetBool(_ctx.InCombatHash, false);
            _ctx.Animator.SetBool(_ctx.IsCastingMagicHash, false);
            SwitchState(_factory.InAir());
            return;
        }
        
        if(!_ctx.IsCastingMagic)
        {
            _ctx.Animator.SetBool(_ctx.IsCastingMagicHash, false);
            if(_ctx.InCombat)
                SwitchState(_factory.Combat());
            else
                SwitchState(_factory.Grounded());
        }
    }

    public async void HandleMagicDamageRateAsync()
    {
        while (_ctx.IsCastingMagic)
        {
            if(_ctx.ShowDebugLogs) Debug.Log("Magic Damage Tick");
            _ctx.MagicWeaponManager.EnableCollider();
            await Task.Delay(TimeBetweenDamagesMs);
        }
    }

}

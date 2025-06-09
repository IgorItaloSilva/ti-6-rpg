using PlasticGui;
using Debug = UnityEngine.Debug;
using Task = System.Threading.Tasks.Task;

public class PlayerMagicState : PlayerCombatState
{
    private readonly int TimeBetweenDamagesMs;

    public PlayerMagicState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        TimeBetweenDamagesMs = 500;
        _ctx.CanCastMagic = false;
        _ctx.Animator.SetBool(_ctx.IsCastingMagicHash, true);
    }

    public override void EnterState()
    {
        HandleMagicDamageRateAsync();
        if (_ctx.ShowDebugLogs) Debug.Log("Casting Magic");
        _turnTime = _ctx.BaseTurnTime * 2;
    }

    public override void ExitState()
    {
        _ctx.CanCastMagic = false;
        _ctx.MagicWeaponManager.DisableCollider();
        _ctx.Animator.SetBool(_ctx.IsCastingMagicHash, false);
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

        if (!_ctx.IsMagicPressed)
        {
            _ctx.Animator.SetBool(_ctx.IsCastingMagicHash, false);
            if (_ctx.InCombat)
                SwitchState(_factory.Combat());
            else
                SwitchState(_factory.Grounded());
        }
    }

    public async void HandleMagicDamageRateAsync()
    {
        await Task.Delay(TimeBetweenDamagesMs);
        while (_ctx.IsMagicPressed && _ctx.CurrentState is PlayerMagicState)
        {
            if (_ctx.ShowDebugLogs) Debug.Log("Magic Damage Tick");
            _ctx.MagicWeaponManager.EnableCollider();
            await Task.Delay(TimeBetweenDamagesMs);
        }
        _ctx.CanCastMagic = false;
        _ctx.MagicWeaponManager.DisableCollider();
        await Task.Delay(TimeBetweenDamagesMs);
        _ctx.CanCastMagic = true;
    }
}
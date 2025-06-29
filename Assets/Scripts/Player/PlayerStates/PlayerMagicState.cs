using System.Collections;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

public class PlayerMagicState : PlayerCombatState
{
    private readonly float _timeBetweenDamagesMs;
    private float _damageTimer;
    private Light _fireLight;

    public PlayerMagicState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        _timeBetweenDamagesMs = 0.5f;
        _ctx.CanCastMagic = false;
        _ctx.Animator.SetBool(_ctx.IsCastingMagicHash, true);
        _fireLight = _ctx.MagicVFX.transform.GetChild(0).GetComponent<Light>();
        _fireLight.enabled = true;
    }

    public override void EnterState()
    {
        _ctx.MagicVFX.Play();
        _ctx.StartSpendManaCoroutine();
        _ctx.StartSpendManaCoroutine();
        if (_ctx.ShowDebugLogs) Debug.Log("Casting Magic");
        _turnTime = _ctx.BaseTurnTime * 2;
    }

    public override void ExitState()
    {
        _ctx.CanCastMagic = true;
        _ctx.EndSpendManaCoroutine();
        _fireLight.enabled = false;
        _ctx.MagicWeaponManager.DisableCollider();
        _ctx.Animator.SetBool(_ctx.IsCastingMagicHash, false);
        _ctx.MagicVFX.Stop();
        AudioPlayer.instance.StopSFX("Fire");
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
        _damageTimer += Time.fixedDeltaTime;
        if (_damageTimer >= _timeBetweenDamagesMs)
        {
            _ctx.MagicWeaponManager.EnableCollider();
            _damageTimer = 0f;
        }
    }

    public override void CheckSwitchStates()
    {
        if (!_ctx.CC.isGrounded)
        {
            _ctx.Animator.SetBool(_ctx.InCombatHash, false);
            SwitchState(_factory.InAir());
            return;
        }

        if (!_ctx.IsMagicPressed || !_ctx.InCombat || !_ctx.HasMana())
        {
            if (_ctx.InCombat)
                SwitchState(_factory.Combat());
            else
                SwitchState(_factory.Grounded());
        }
    }

}
using System.Collections;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

public class PlayerMagicState : PlayerCombatState
{
    private readonly int TimeBetweenDamagesMs;
    private Light _fireLight;
    private float _lightIntensity;

    public PlayerMagicState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        TimeBetweenDamagesMs = 500;
        _ctx.CanCastMagic = false;
        _ctx.Animator.SetBool(_ctx.IsCastingMagicHash, true);
        _fireLight = _ctx.MagicVFX.transform.GetChild(0).GetComponent<Light>();
        _lightIntensity = _fireLight.intensity;
        _fireLight.intensity = 0f;
        _fireLight.enabled = true;
    }

    public override void EnterState()
    {
        _ctx.MagicVFX.Play();
        _ctx.StartSpendManaCoroutine();
        _ctx.StartSpendManaCoroutine();
        HandleMagicDamageRateAsync();
        if (_ctx.ShowDebugLogs) Debug.Log("Casting Magic");
        _turnTime = _ctx.BaseTurnTime * 2;
    }

    public override void ExitState()
    {
        _ctx.EndSpendManaCoroutine();
        SmoothDisableLightAsync(_lightIntensity);
        _ctx.CanCastMagic = false;
        _ctx.MagicWeaponManager.DisableCollider();
        _ctx.Animator.SetBool(_ctx.IsCastingMagicHash, false);
        _ctx.MagicVFX.Stop();
        AudioPlayer.instance.StopSFX("Fire");
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

    public async void HandleMagicDamageRateAsync()
    {
        await Task.Delay(TimeBetweenDamagesMs);
        AudioPlayer.instance.PlaySFX("Fire");
        SmoothEnableLightAsync();
        while (_ctx.IsMagicPressed && _ctx.CurrentState is PlayerMagicState)
        {
            _ctx.MagicWeaponManager.EnableCollider();
            await Task.Delay(TimeBetweenDamagesMs);
        }
        _ctx.CanCastMagic = false;
        _ctx.MagicWeaponManager.DisableCollider();
        await Task.Delay(TimeBetweenDamagesMs);
        _ctx.CanCastMagic = true;
    }

    private async void SmoothDisableLightAsync(float initialLightIntensity)
    {
        while (_fireLight.intensity > 0)
        {
            _fireLight.intensity -= Time.fixedDeltaTime * 200;
            await Task.Delay((int)(Time.fixedDeltaTime * 1000));
        }

        _fireLight.enabled = false;
        _fireLight.intensity = initialLightIntensity;
    }

    private async void SmoothEnableLightAsync()
    {
        _fireLight.enabled = true;
        while (_fireLight.intensity < _lightIntensity)
        {
            _fireLight.intensity += Time.fixedDeltaTime * 50;
            await Task.Delay((int)(Time.fixedDeltaTime * 1000));
        }
        _fireLight.intensity = _lightIntensity;
    }
    
}
using System.Threading.Tasks;
using Debug = UnityEngine.Debug;

public class PlayerCombatState : PlayerGroundedState
{
    private const byte CombatCooldownMs = 255;

    public PlayerCombatState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        _maxAcceleration = 1.5f;
        _ctx.AppliedMovementY = _ctx.BaseGravity;
        _ctx.CurrentMovement = _ctx.CurrentMovementInput;
        _ctx.CurrentMovementZ = _ctx.CurrentMovementInput.y;
    }

    public override void HandleAnimatorParameters()
    {
        _ctx.Animator.SetBool(_ctx.IsGroundedHash, true);
        _ctx.Animator.SetBool(_ctx.InCombatHash, true);
        _ctx.Animator.SetBool(_ctx.IsWalkingHash, _ctx.IsMovementPressed);
    }

    public override void EnterState()
    {
        HandleAnimatorParameters();
        if (_ctx.ShowDebugLogs) Debug.Log("Combat");
        _turnTime = _ctx.BaseTurnTime * 2;
        _ctx.Animator.SetFloat(_ctx.PlayerIdleAnimationHash, 0f);
    }

    public override void UpdateState()
    {
        HandleTargetedMove();
        HandleTargetedRotation();
        HandlePotion();
        CheckSwitchStates();
    }

    public override void CheckSwitchStates()
    {
        if (!_ctx.InCombat)
        {
            _ctx.CanEnterCombat = false;
            HandleCombatCooldownAsync();
            SwitchState(_factory.Grounded());
            return;
        }

        if (!_ctx.CC.isGrounded)
        {
            _ctx.Animator.SetBool(_ctx.InCombatHash, false);
            SwitchState(_factory.InAir());
            return;
        }

        if (_ctx.IsBlockPressed)
        {
            _ctx.Animator.SetBool(_ctx.IsBlockingHash, true);
            SwitchState(_factory.Block());
            return;
        }

        if (_ctx.IsCastingMagic)
        {
            _ctx.Animator.SetBool(_ctx.IsCastingMagicHash, true);
            SwitchState(_factory.Magic());
            return;
        }

        if (_ctx.IsJumpPressed)
        {
            HandleJump();
            SwitchState(_factory.InAir());
            return;
        }

        if (_ctx.IsSprintPressed)
        {
            SwitchState(_factory.Grounded());
            return;
        }

        if (_ctx.IsDodgePressed)
        {
            SwitchState(_factory.Dodge());
            return;
        }

        if (_ctx.IsAttackPressed)
        {
            SwitchState(_factory.Attack());
            return;
        }

        if (_ctx.IsSpecial1Pressed)
        {
            _ctx.Animator.ResetTrigger(_ctx.Special1Hash);
            _ctx.Animator.SetTrigger(_ctx.Special1Hash);
            _ctx.AttackCount = 5;
            _ctx.CanSpecial1 = false;
            _ctx.StartSpecialCooldown(1);
            SwitchState(_factory.Attack());
            return;
        }

        if (_ctx.IsSpecial2Pressed)
        {
            _ctx.Animator.ResetTrigger(_ctx.Special2Hash);
            _ctx.Animator.SetTrigger(_ctx.Special2Hash);
            _ctx.AttackCount = 6;
            _ctx.CanSpecial2 = false;
            _ctx.StartSpecialCooldown(2);
            SwitchState(_factory.Attack());
            return;
        }

        if (_ctx.IsSpecial3Pressed)
        {
            _ctx.Animator.ResetTrigger(_ctx.Special3Hash);
            _ctx.Animator.SetTrigger(_ctx.Special3Hash);
            _ctx.AttackCount = 7;
            _ctx.CanSpecial3 = false;
            _ctx.StartSpecialCooldown(3);
            HandleJump();
            SwitchState(_factory.InAir());
            return;
        }

        if (_ctx.IsSpecial4Pressed)
        {
            _ctx.Animator.ResetTrigger(_ctx.Special4Hash);
            _ctx.Animator.SetTrigger(_ctx.Special4Hash);
            _ctx.AttackCount = 8;
            _ctx.CanSpecial4 = false;
            _ctx.StartSpecialCooldown(4);
            SwitchState(_factory.Attack());
            return;
        }

        if (_ctx.PlayerStats.PlayerIsDead)
        {
            SwitchState(_factory.Dead());
        }
    }

    private async void HandleCombatCooldownAsync()
    {
        await Task.Delay(CombatCooldownMs);
        _ctx.CanEnterCombat = true;
    }
}
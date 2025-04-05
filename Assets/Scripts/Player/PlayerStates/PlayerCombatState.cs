using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerCombatState : PlayerGroundedState
{
    private Vector3 _appliedMovement, _cameraForward, _cameraRight;

    public PlayerCombatState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        HandleAnimatorParameters();
    }

    public override void EnterState()
    {
        if (_ctx.ShowDebugLogs) Debug.Log("Combat");
        _turnTime = float.MaxValue;
    }

    public sealed override void HandleAnimatorParameters()
    {
        _ctx.Animator.SetLayerWeight(1, 0);
        _ctx.Animator.SetBool(_ctx.IsGroundedHash, true);
        _ctx.Animator.SetBool(_ctx.IsRunningHash, false);
        _ctx.Animator.SetBool(_ctx.IsWalkingHash, _ctx.IsMovementPressed);
    }

    protected override void HandleRotation()
    {
        var lookPos = _ctx.MainCam.transform.forward;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        _ctx.transform.rotation = Quaternion.Slerp(_ctx.transform.rotation, rotation, Time.deltaTime * 5f);
    }
    
    protected override void HandleMove()
    {
        _cameraForward = _ctx.MainCam.transform.forward;
        _cameraRight = _ctx.MainCam.transform.right;
    
        _cameraForward.y = 0;
        _cameraRight.y = 0;
    
        _cameraForward.Normalize();
        _cameraRight.Normalize();
    
        _appliedMovement = _cameraForward * (_ctx.CurrentMovement.z * _ctx.Acceleration) + 
                           _cameraRight * (_ctx.CurrentMovement.x * _ctx.Acceleration);
        _appliedMovement.y = _ctx.BaseGravity;
    
        _ctx.AppliedMovement = _appliedMovement;
        _ctx.CC.Move(_ctx.AppliedMovement * (_ctx.BaseMoveSpeed * Time.deltaTime));
    }

    protected override void HandleAnimatorVelocity()
    {
        _ctx.Animator.SetFloat(_ctx.PlayerVelocityYHash, Mathf.Lerp(_ctx.Animator.GetFloat(_ctx.PlayerVelocityYHash), _ctx.Acceleration * Mathf.Abs(_ctx.CurrentMovementInput.y), Time.fixedDeltaTime * 10f));
        _ctx.Animator.SetFloat(_ctx.PlayerVelocityXHash, Mathf.Lerp(_ctx.Animator.GetFloat(_ctx.PlayerVelocityXHash), _ctx.Acceleration * Mathf.Abs(_ctx.CurrentMovementInput.x), Time.fixedDeltaTime * 10f));
    }
}
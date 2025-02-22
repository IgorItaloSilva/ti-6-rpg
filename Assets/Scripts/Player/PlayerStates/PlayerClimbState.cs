using System.Runtime.CompilerServices;
using UnityEngine;

namespace Player.PlayerStates
{
    public class PlayerClimbState : PlayerBaseState
    {
        protected const float ClimbSpeed = 4f;
        private Vector3 _localMovement;
        
        public PlayerClimbState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
            currentContext, playerStateFactory)
        {
            HandleAnimatorParameters();
        }

        public override void EnterState()
        {
            _ctx.Acceleration = 0;
            _ctx.CanMount = false;
            if(_ctx.ShowDebugLogs) Debug.Log("Climbing");
        }

        public sealed override void HandleAnimatorParameters()
        {
            _ctx.Animator.SetBool(_ctx.IsClimbingHash, true);
            _ctx.Animator.SetBool(_ctx.IsGroundedHash, false);
        }

        public override void UpdateState()
        {
            HandleClimb();
            CheckSwitchStates();
        }

        private void HandleClimb()
        {
            _ctx.Animator.SetFloat(_ctx.PlayerVelocityHash, Mathf.Abs(_ctx.CurrentMovementInput.x) > Mathf.Abs(_ctx.CurrentMovementInput.y) ? _ctx.CurrentMovementInput.x : _ctx.CurrentMovementInput.y);
            _localMovement = ((_ctx.transform.right * _ctx.CurrentMovementInput.x) + (Vector3.up * _ctx.CurrentMovementInput.y)) * _ctx.Acceleration;
            _ctx.CC.Move(_localMovement * (ClimbSpeed * Time.deltaTime));
        }

        public override void ExitState()
        {
            _ctx.Animator.SetBool(_ctx.IsClimbingHash, false);
        }

        public override void CheckSwitchStates()
        {
            if (!_ctx.IsClimbing || (_ctx.CC.isGrounded && _ctx.CC.velocity.y < 0f) || _ctx.IsJumpPressed)
            {
                if (_ctx.CC.isGrounded)
                {
                    SwitchState(_factory.Grounded());
                }
                else
                {
                    if (_ctx.IsJumpPressed)
                    {
                        if(_ctx.IsMovementPressed)
                        {
                            _ctx.transform.Rotate(0f, 180f, 0f);
                            HandleJump();
                            SwitchState(_factory.InAir(shouldRotate: false));
                            return;
                        }
                        SwitchState(_factory.InAir());
                    }
                    else
                    {
                        HandleJump();
                        SwitchState(_factory.InAir());
                    }

                    
                }
            }
        }
    }
}
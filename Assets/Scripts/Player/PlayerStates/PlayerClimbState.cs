using System.Runtime.CompilerServices;
using UnityEngine;

namespace Player.PlayerStates
{
    public class PlayerClimbState : PlayerBaseState
    {
        protected const float ClimbSpeed = 4f;

        public PlayerClimbState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
            currentContext, playerStateFactory)
        {
            HandleAnimatorParameters();
        }

        public override void EnterState()
        {
            _ctx.CanMount = false;
            Debug.Log("Climbing");
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
            _ctx.Animator.SetFloat(_ctx.PlayerVelocity, _ctx.CurrentMovementInput.y);
            _ctx.AppliedMovement = new Vector3(0f, _ctx.CurrentMovementInput.y, 0f);
            _ctx.CC.Move(_ctx.AppliedMovement * (ClimbSpeed * Time.deltaTime));
        }

        public void HandleJump(float jumpStrength = 1f)
        {
            _ctx.CanJump = false;
            if (!_ctx.IsMovementPressed)
                _ctx.AppliedMovement = Vector3.zero;
            _ctx.CurrentMovementY = _ctx.InitialJumpVelocity * jumpStrength;
            _ctx.AppliedMovementY = _ctx.InitialJumpVelocity * jumpStrength;
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
                        _ctx.transform.Rotate(0f, 180f, 0f);
                        if(_ctx.IsMovementPressed)
                        {
                            HandleJump();
                            SwitchState(_factory.InAir(airMoveSpeedOverride: 15f));
                        }
                        SwitchState(_factory.InAir());
                    }
                    else
                    {
                        HandleJump(0.75f);
                        SwitchState(_factory.InAir());
                    }

                    
                }
            }
        }
    }
}
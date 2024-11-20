using System.Runtime.CompilerServices;
using UnityEngine;

namespace Player.PlayerStates
{
    public class PlayerClimbState : PlayerBaseState
    {
        protected const float ClimbSpeed = 2f;
        private readonly Transform _colliderTransform;

        public PlayerClimbState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
            currentContext, playerStateFactory)
        {
            HandleAnimatorParameters();
            _ctx.CanMount = false;
            RaycastHit hit;
            if(Physics.Raycast(_ctx.transform.position, _ctx.transform.forward, out hit))
            {
                _colliderTransform = hit.collider.gameObject.transform;
                _ctx.TurnTime = float.MaxValue;
                _ctx.transform.rotation = _colliderTransform.rotation;
            }
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
            HandleClimbJump();
            HandleClimb();
            CheckSwitchStates();
        }

        private void HandleClimb()
        {
            _ctx.Animator.SetFloat(_ctx.PlayerVelocity, _ctx.CurrentMovementInput.y);
            _ctx.AppliedMovement = new Vector3(0f, _ctx.CurrentMovementInput.y, 0f);
            _ctx.CC.Move(_ctx.AppliedMovement * (ClimbSpeed * Time.deltaTime));
        }

        private void HandleClimbJump()
        {
            if (_ctx.IsJumpPressed)
            {
                _ctx.transform.Rotate(0f,180f,0f);
                SwitchState(_factory.InAir());
            }
        }

        public override void ExitState()
        {
            _ctx.Animator.SetBool(_ctx.IsClimbingHash, false);
        }

        public override void CheckSwitchStates()
        {
            if (!_ctx.IsClimbing || (_ctx.CC.isGrounded && _ctx.CC.velocity.y < 0f))
            {
                if(_ctx.CC.isGrounded)
                {
                    _ctx.transform.Rotate(0f,180f,0f);
                    SwitchState(_factory.Grounded());
                }
                else
                    SwitchState(_factory.InAir());
            }
            
        }
    }
}
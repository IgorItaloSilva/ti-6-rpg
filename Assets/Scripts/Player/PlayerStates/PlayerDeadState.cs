using UnityEngine;

namespace Player.PlayerStates
{
    public class PlayerDeadState : PlayerBaseState
    {
        public PlayerDeadState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
        {
        }

        public override void HandleAnimatorParameters()
        {
            
        }

        public override void EnterState()
        {
            _ctx.Animator.ResetTrigger(_ctx.HasDiedHash);
            _ctx.Animator.SetTrigger(_ctx.HasDiedHash);
        }

        public override void UpdateState()
        {
            ApplyGravity();
        }

        private void ApplyGravity()
        {
            _ctx.CC.Move(new Vector3(0f, _ctx.BaseGravity, 0f));
        }

        public override void ExitState()
        {
            
        }

        public override void CheckSwitchStates()
        {
           
        }
    }
}
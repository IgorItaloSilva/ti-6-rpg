public class SprintState : PlayerState
{
    public override PlayerMovement.moveStateTypes MoveState => PlayerMovement.moveStateTypes.sprinting;
    public override float MoveSpeed => 18f;
    public override float TurnTime => 0.125f;
}

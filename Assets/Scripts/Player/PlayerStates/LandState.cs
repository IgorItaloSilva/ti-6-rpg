public class LandState : PlayerState
{
    public override PlayerMovement.moveStateTypes MoveState => PlayerMovement.moveStateTypes.landing;
    public override float MoveSpeed => 8f;
    public override float TurnTime => 0.15f;
}

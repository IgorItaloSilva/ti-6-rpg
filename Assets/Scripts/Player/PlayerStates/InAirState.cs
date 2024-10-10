public class InAirState : PlayerState
{
    public override PlayerMovement.moveStateTypes MoveState => PlayerMovement.moveStateTypes.inAir;
    public override float MoveSpeed => 150f;
    public override float TurnTime => 3f;
}

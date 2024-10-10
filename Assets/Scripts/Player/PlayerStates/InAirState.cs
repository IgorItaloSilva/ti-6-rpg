public class InAirState : PlayerState
{
    public override PlayerMovement.moveStateTypes MoveState => PlayerMovement.moveStateTypes.inAir;
    public override float MoveSpeed => 200f;
    public override float TurnTime => 1f;
}

public class InAirState : PlayerState
{
    public override PlayerMovement.moveTypes moveType => PlayerMovement.moveTypes.inAir;
    public override float MoveSpeed => 150f;
    public override float TurnTime => 3f;
}

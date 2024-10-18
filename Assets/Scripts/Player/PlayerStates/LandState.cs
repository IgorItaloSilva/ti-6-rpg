public class LandState : PlayerState
{
    public override float MoveSpeed => 8f;
    public override PlayerMovement.moveTypes moveType => PlayerMovement.moveTypes.landing;
    public override float TurnTime => 0.15f;
}

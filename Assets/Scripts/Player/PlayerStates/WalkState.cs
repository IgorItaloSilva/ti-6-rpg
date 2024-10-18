public class WalkState : PlayerState
{
    public override PlayerMovement.moveTypes moveType => PlayerMovement.moveTypes.walking;
    public override float MoveSpeed => 12f;
    public override float TurnTime => 0.1f;
}

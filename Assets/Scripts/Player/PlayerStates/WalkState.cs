public class WalkState : PlayerState
{
    public override PlayerMovement.moveStateTypes MoveState => PlayerMovement.moveStateTypes.walking;
    public override float MoveSpeed => 12f;
    public override float TurnTime => 0.1f;
}

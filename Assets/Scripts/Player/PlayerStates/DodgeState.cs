public class DodgeState : PlayerState
{
    public override PlayerMovement.moveStateTypes MoveState => PlayerMovement.moveStateTypes.dodging;
    public override float MoveSpeed => 70f;
    public override float TurnTime => 0.05f;
}

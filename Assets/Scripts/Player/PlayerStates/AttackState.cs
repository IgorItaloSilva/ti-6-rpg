public class AttackState : PlayerState
{
    public override PlayerMovement.moveStateTypes MoveState => PlayerMovement.moveStateTypes.attacking;
    public override float MoveSpeed => 3f;
    public override float TurnTime => 0.1f;
}

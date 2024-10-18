public class SprintState : PlayerState
{
    public override PlayerMovement.moveTypes moveType => PlayerMovement.moveTypes.sprinting;
    public override float MoveSpeed => 18f;
    public override float TurnTime => 0.125f;
}

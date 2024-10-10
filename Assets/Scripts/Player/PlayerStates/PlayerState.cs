public abstract class PlayerState
{
    public abstract PlayerMovement.moveStateTypes MoveState { get; }
    public abstract float MoveSpeed { get; }
    public abstract float TurnTime { get; }
}
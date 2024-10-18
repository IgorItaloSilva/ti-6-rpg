public abstract class PlayerState
{
    public abstract PlayerMovement.moveTypes moveType { get; }
    public abstract float MoveSpeed { get; }
    public abstract float TurnTime { get; }
}
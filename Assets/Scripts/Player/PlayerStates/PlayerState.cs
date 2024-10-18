using UnityEngine.InputSystem;

public abstract class PlayerState
{
    public abstract PlayerMovement.moveTypes moveType { get; }
    public abstract float MoveSpeed { get; }
    public abstract float TurnTime { get; }

    public virtual void Enter(){}
    public abstract void Update();
    public virtual void FixedUpdate()
    {
        // Aplicar movimentação do player no FixedUpdate para não causar os travamentos de antes
        if ((PlayerMovement.instance.MoveInput.magnitude > 0.01f)) PlayerMovement.instance.Move();
    }
}
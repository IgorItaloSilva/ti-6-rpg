using UnityEngine.InputSystem;

public class AttackState : PlayerState
{
    public override PlayerMovement.moveTypes moveType => PlayerMovement.moveTypes.attacking;
    public override float MoveSpeed => 3f;
    public override float TurnTime => 0.1f;
    
    public override void Update()
    {
        
    }
}

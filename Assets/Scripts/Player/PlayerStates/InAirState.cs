using UnityEngine.InputSystem;

public class InAirState : PlayerState
{
    public override PlayerMovement.moveTypes moveType => PlayerMovement.moveTypes.inAir;
    public override float MoveSpeed => 150f;
    public override float TurnTime => 1f;

    public override void Enter()
    {
        PlayerMovement.instance.LandAsync();
    }
    public override void Update()
    {
        
    }
    public override void FixedUpdate(){}
}

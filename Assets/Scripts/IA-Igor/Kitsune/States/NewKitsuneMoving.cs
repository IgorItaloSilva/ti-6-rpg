using UnityEngine;

public class NewKitsuneMoving : EnemyBaseState
{
    protected override void OneExecution()
    {
        animator.CrossFade("Run", 0.25f);
        steeringForce = 0.1f;
    }

    // Update is called once per frame
    public override void StateUpdate()
    {
        if(GetTargetAngle(charControl.transform, enemyBehave.GetTarget()) > 90){
            enemyBehave.currentState = new NewKitsuneTurn();
            StateExit();
        }
        
        if(GetPlayerDistance() < minDistPlayer){
            enemyBehave.currentState = new NewKitsuneIdle();
            StateExit();
        }

    }

    public override void StateFixedUpdate()
    {
        float newSpeed = speed;
        float newSteering = steeringForce;
        if(enemyBehave.GetTarget()) {
            newSpeed *= 0.5f;
            newSteering *= 0.5f; 

            // Rotation
            Vector3 directionToPlayer = (enemyBehave.GetTarget().position - charControl.transform.position).normalized;
            directionToPlayer.y = 0;
            Quaternion rotationDesired = Quaternion.LookRotation(directionToPlayer);
            charControl.transform.rotation = Quaternion.Slerp(charControl.transform.rotation, rotationDesired, lookTime);
            if(lookTime < 1)
                lookTime += newSteering * Time.fixedDeltaTime;
            
            // Moviment
            charControl.Move(charControl.transform.forward * newSpeed + Vector3.up * ApplyGravity());

        }
        
    }

}
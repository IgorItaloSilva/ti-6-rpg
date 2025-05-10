using UnityEngine;

public class KasaMoving : EnemyBaseState
{
    protected override void OneExecution()
    {
        animator.CrossFade("Moving", 0.25f);
        steeringForce = 0.1f;
        minDistPlayer = 2f;
    }

    public override void StateUpdate()
    {
        if(GetPlayerDistance() < minDistPlayer){
            enemyBehave.currentState = new KasaIdle();
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
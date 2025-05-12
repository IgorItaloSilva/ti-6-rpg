using System.ComponentModel;
using UnityEngine;


public class NewKitsuneTurn : EnemyBaseState
{
    Quaternion startRotation;
    Quaternion desiredRotation;

    protected override void OneExecution()
    {
        steeringForce = 0.5f;
        Vector3 directionToPlayer = (enemyBehave.GetTarget().position - charControl.transform.position).normalized;
        directionToPlayer.y = 0;
        Debug.Log("Test : " + Vector3.Cross(charControl.transform.forward, directionToPlayer).y);
        if(Vector3.Cross(charControl.transform.forward, directionToPlayer).y > 0){
            animator.Play("Turn Mirror", -1, 0.25f);
        }else
            animator.Play("Turn", -1, 0.25f);
        
    }

    public override void StateUpdate()
    {
        if(lookTime >= 0.5f){
            enemyBehave.currentState = new NewKitsuneIdle();
            StateExit();
        }
    }

    public override void StateFixedUpdate(){
        
        // Rotation
            Vector3 directionToPlayer = (enemyBehave.GetTarget().position - charControl.transform.position).normalized;
            directionToPlayer.y = 0;
            desiredRotation = Quaternion.LookRotation(directionToPlayer);
            charControl.transform.rotation = Quaternion.Slerp(charControl.transform.rotation, desiredRotation, lookTime);
            if(lookTime < 1)
                lookTime += steeringForce * Time.fixedDeltaTime;

    }

}
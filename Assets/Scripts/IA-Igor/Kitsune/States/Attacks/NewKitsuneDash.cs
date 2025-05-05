using UnityEngine;

public class NewKitsuneDash : EnemyBaseState
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void OneExecution()
    {
        restTime = 2f;
        animator.Play("Dash", -1, 0f);
    }

    public override void StateUpdate()
    {
        lookTime += Time.deltaTime;

        if(lookTime > 1f){ // acabou o ataque
            enemyBehave.SetRest(restTime);
            enemyBehave.currentState = new NewKitsuneIdle();
            enemyBehave.DisableWeapon();
            StateExit();
        }

    }

    public override void StateFixedUpdate()
    {
        if(lookTime < 0.5f){
            // Rotation
            Vector3 directionToPlayer = (enemyBehave.GetTarget().position - charControl.transform.position).normalized;
            directionToPlayer.y = 0;
            Quaternion rotationDesired = Quaternion.LookRotation(directionToPlayer);
            charControl.transform.rotation = Quaternion.Slerp(charControl.transform.rotation, rotationDesired, lookTime);
            if(lookTime < 1)
                lookTime += steeringForce * Time.fixedDeltaTime * 2;
        
        }else if(lookTime < 0.8f){
            enemyBehave.EnableWeapon();
            charControl.Move(charControl.transform.forward * speed * 2 + charControl.transform.up * ApplyGravity());
        }
        
    }

}
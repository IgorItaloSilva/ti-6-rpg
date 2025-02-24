using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitsuneBasicAttack : EnemyActions
{
    float time;
    public override void EnterAction()
    {
        actualEnemyController.animator.CrossFade("Fox_Attack1",0.1f);
        time=0;
        actualEnemyController.rb.constraints=RigidbodyConstraints.FreezeAll;
    }

    public override void ExitAction()
    {
        actualEnemyController.rb.constraints=RigidbodyConstraints.FreezeRotation;
    }

    public override void UpdateAction()
    {
        time+=Time.fixedDeltaTime;
        ISteeringAgent steeringAgent = actualEnemyController.target;
        if(steeringAgent==null){
            actualEnemyController.ChangeAction(new nullAction());
        }
        else{
            //actualEnemyController.steeringManager.LookAtTarget();
            /*Vector3 target = steeringAgent.GetPosition();
            target.y=actualEnemyController.transform.position.y;
            actualEnemyController.transform.LookAt(target); */
            if(time>animationDuration){
                actualEnemyController.ChangeAction(new nullAction());
            }
        }
    }
    public KitsuneBasicAttack(float attackTime,float minDistToAttack,ActualEnemyController actualEnemyController){
        animationDuration=attackTime;
        distToAttack=minDistToAttack;
        this.actualEnemyController=actualEnemyController;
    }
}

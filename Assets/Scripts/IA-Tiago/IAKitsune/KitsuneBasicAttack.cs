using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitsuneBasicAttack : EnemyActions
{
    float time;
    KitsuneController kitsuneController;
    public override void EnterAction()
    {
        kitsuneController.animator.SetTrigger("isAttacking");
        kitsuneController.isAttacking=true;
        time=0;
        kitsuneController.rb.constraints=RigidbodyConstraints.FreezeAll;
    }

    public override void ExitAction()
    {
        kitsuneController.rb.constraints=RigidbodyConstraints.FreezeRotation;
    }

    public override void UpdateAction()
    {
        time+=Time.fixedDeltaTime;
        ISteeringAgent steeringAgent = kitsuneController.target;
        if(steeringAgent==null){
            kitsuneController.ChangeAction(new nullAction());
        }
        else{
            //kitsuneController.steeringManager.LookAtTarget();
            /*Vector3 target = steeringAgent.GetPosition();
            target.y=kitsuneController.transform.position.y;
            kitsuneController.transform.LookAt(target); */
            if(time>animationDuration){
                kitsuneController.ChangeAction(new nullAction());
            }
        }
    }
    public KitsuneBasicAttack(float attackTime,float minDistToAttack,KitsuneController kitsuneController){
        animationDuration=attackTime;
        distToAttack=minDistToAttack;
        this.kitsuneController=kitsuneController;
    }
}

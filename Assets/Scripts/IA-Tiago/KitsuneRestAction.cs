using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitsuneRestAction : EnemyActions
{
    float restTime;
    float time;
    public override void EnterAction()
    {
        actualEnemyController.animator.CrossFade("Fox|Idle",0.1f);
        actualEnemyController.rb.constraints=RigidbodyConstraints.FreezePosition;
        time=0;
    }

    public override void ExitAction()
    {
        actualEnemyController.animator.CrossFade("Fox_Run",0.1f);
        actualEnemyController.rb.constraints=RigidbodyConstraints.FreezeRotation;
    }

    public override void UpdateAction()
    {
        time+=Time.fixedDeltaTime;
        if(time>restTime)
            actualEnemyController.ChangeAction(new nullAction());
    }
    public KitsuneRestAction(float restTime,ActualEnemyController actualEnemyController){
        this.restTime=restTime;
        this.actualEnemyController=actualEnemyController;
    }
}

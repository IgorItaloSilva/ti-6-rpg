using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitsuneTakeDamageAction : EnemyActions
{
    float time;
    public KitsuneTakeDamageAction(float animationDuration,ActualEnemyController actualEnemyController){
        this.animationDuration=animationDuration;
        this.actualEnemyController=actualEnemyController;
    }

    public override void EnterAction()
    {
        time=0;
        actualEnemyController.animator.CrossFade("Fox_Damage",0.1f);
    }

    public override void ExitAction()
    {
        
    }

    public override void UpdateAction()
    {
        time+=Time.fixedDeltaTime;
        if(time>animationDuration)
            actualEnemyController.ChangeAction(new nullAction());
    }
}

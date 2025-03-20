using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitsuneRestAction : EnemyActions
{
    float restTime;
    float time;
    KitsuneController kitsuneController;
    public override void EnterAction()
    {
        kitsuneController.animator.SetBool("isResting",true);
        kitsuneController.isResting=true;
        kitsuneController.rb.constraints=RigidbodyConstraints.FreezeAll;
        time=0;
    }

    public override void ExitAction()
    {
        kitsuneController.animator.SetBool("isResting",false);
        kitsuneController.rb.constraints=RigidbodyConstraints.FreezeRotation;
        kitsuneController.ResetSpecialAttacksCharges();
    }

    public override void UpdateAction()
    {
        time+=Time.fixedDeltaTime;
        if(time>restTime)
            kitsuneController.ChangeAction(new nullAction());
    }
    public KitsuneRestAction(float restTime,KitsuneController kitsuneController){
        this.restTime=restTime;
        this.kitsuneController=kitsuneController;
    }
}

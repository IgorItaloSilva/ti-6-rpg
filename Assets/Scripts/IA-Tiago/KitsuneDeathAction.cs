using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitsuneDeathAction : EnemyActions
{
    KitsuneController kitsuneController;
    float time;
    public override void EnterAction()
    {
        time=0f;
        kitsuneController.animator.SetBool("isDeadBool",true);
        kitsuneController.animator.ResetTrigger("isDead");
        kitsuneController.animator.SetTrigger("isDead");
        kitsuneController.rb.constraints=RigidbodyConstraints.FreezeAll;
    }

    public override void ExitAction()
    {
        kitsuneController.ActualDeath();
        kitsuneController.rb.constraints=RigidbodyConstraints.FreezeRotation;
    }

    public override void UpdateAction()
    {
        time+=Time.deltaTime;
        if(time>=animationDuration){
            kitsuneController.ChangeAction(new nullAction());
        }
    }

    public KitsuneDeathAction(float animationDuration,KitsuneController kitsuneController){
        this.animationDuration=animationDuration;
        this.kitsuneController=kitsuneController;
    }
}

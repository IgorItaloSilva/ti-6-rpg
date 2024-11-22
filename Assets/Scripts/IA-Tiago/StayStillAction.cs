using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayStillAction : EnemyActions
{
    public override void EnterAction()
    {
        actualEnemyController.rb.constraints=RigidbodyConstraints.FreezeAll;
        Debug.Log("Entrei na action stay still");
    }

    public override void ExitAction()
    {
        actualEnemyController.rb.constraints=RigidbodyConstraints.FreezeRotation;
        Debug.Log("Sai da action stay still");
    }

    public override void UpdateAction()
    {
        
    }
    public StayStillAction(ActualEnemyController actualEnemyController){
        this.actualEnemyController=actualEnemyController;
    }
}

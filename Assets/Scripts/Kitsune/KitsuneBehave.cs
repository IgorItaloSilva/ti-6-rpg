using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitsuneBehave : AEnemyBehave
{
    protected override void SetActions()
    {
        actionList.Add(new KitsuneIdle()); // idle
        actionList.Add(new KitsuneApproach()); // Correr 
        actionList.Add(new KitsuneIdle()); 
    }

    public override void Think(out AEnemyAction action, bool haveToRest)
    {
        HaveToRest(haveToRest);
        Vector3 thisForward = enemyController.transform.forward;
        Vector3 dir = (enemyController.GetTarget().position - enemyController.transform.position).normalized;
        thisForward.y = 0;
        dir.y = 0;
        Debug.Log("Teste Rest: " + haveToRest);
        if (haveToRest)
        {
            action = actionList[0];
            action.SetRestTime(UnityEngine.Random.Range(1.5f, 2.5f));
        }
        else if (Vector3.Angle(enemyController.transform.forward, dir) > 130f) // Virar personagem para o jogador
            action = actionList[2];
        else if (Vector3.Distance(transform.position, enemyController.GetTarget().position) > actionsCanUse[0].GetMinDistanceSkill()) // Checar distancia entre inimigo e jogador
        {
            actionList[1].SetDistance(actionsCanUse[0].GetMinDistanceSkill());
            action = actionList[1];
        }
        else // Aplica idle
            action = actionsCanUse[0];
    }

    protected override void HaveToRest(bool haveToRest)
    {
        throw new System.NotImplementedException();
    }

}

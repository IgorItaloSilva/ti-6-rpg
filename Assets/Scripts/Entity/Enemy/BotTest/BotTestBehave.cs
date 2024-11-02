using System;
using UnityEngine;
public class BotTestBehave : AEnemyBehave
{

    protected override void SetActions()
    {
        actionList.Add(new BotTestIdle()); // 0
        actionList.Add(new BotTestApproach()); // 1
        actionList.Add(new BotTestTurn()); // 2
        actionList.Add(new BotTestKickCombo()); // 3
        //actionList.Add(new BotTestMagicAttack()); // 4
        startSkills = 3;
        for(int i = startSkills; i < actionList.Count; i++)
            actionsCanUse.Add(actionList[i]);
        actionsCanUse.Sort();

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
        else if (Vector3.Distance(transform.position, enemyController.GetTarget().position) > actionsCanUse[0].GetMinDistanceSkill())
        {
            actionList[1].SetDistance(actionsCanUse[0].GetMinDistanceSkill());
            action = actionList[1];
        }
        else
            action = actionsCanUse[0];
    }


    //public override AEnemyAction GetAction()
    //{
    //    return actionsCanUse[0];
    //}

    protected override void HaveToRest(bool haveToRest)
    {
        this.haveToRest = haveToRest;
    }

}

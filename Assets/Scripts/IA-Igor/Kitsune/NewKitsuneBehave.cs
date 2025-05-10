using System;
using UnityEngine;

public class NewKitsuneBehave : EnemyBehaviour
{
    protected override void OneExecution()
    {
        attackState = allSkills.ChoseSkill();
        currentState = new NewKitsuneIdle();
    }
}

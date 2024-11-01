using UnityEngine;


public class TestKitsuneHeadButt : TestEnemyActions
{
    bool canUpdate;

    public TestKitsuneHeadButt()
    {
        minRange = 3.1f;
    }

    protected override void AdditionalStart()
    {
        restTime = 1f;
        canUpdate = true;
        animator.SetBool("isAttacking", true);
    }

    public override void UpdateAction()
    {
        TrackTarget(trackSpeed:0.1f);
        if (InRestTime())
            return;
        if(canUpdate)
        {
            rb.AddForce(rb.transform.forward * 5f, ForceMode.Impulse);
            canUpdate = false;
        }
        if (restTime < -0.5f)
            ExitAction(enemyController.GetMoveActions(0));
    }

    public override void ExitAction(TestEnemyActions enemyAction)
    {
        enemyController.EnemyAttacked();
        animator.SetBool("isAttacking", false);
        enemyController.SetMovement(enemyAction);
    }
}

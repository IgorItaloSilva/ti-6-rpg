using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitSuneRangedAttack : EnemyActions
{
    float rangedAttackTime;
    float time;
    float auxSpawn;
    GameObject rangedAttackPrefab;
    public override void EnterAction()
    {
        //actualEnemyController.animator.CrossFade("Fox_Attack1",0.1f);
        time=0;
        //actualEnemyController.animator.applyRootMotion=true;
        actualEnemyController.rb.constraints=RigidbodyConstraints.FreezeAll;
        auxSpawn=1;
    }

    public override void ExitAction()
    {
        actualEnemyController.rb.constraints=RigidbodyConstraints.FreezeRotation;
    }

    public override void UpdateAction()
    {
        time+=Time.fixedDeltaTime;
        if(time>auxSpawn){
            Vector3 spawnPoint = actualEnemyController.transform.position + new Vector3(0,4,0);
            GameObject newAttack = GameObject.Instantiate(rangedAttackPrefab,spawnPoint,Quaternion.identity);
            AttackRangedKitsuneBoss attackRangedKitsuneBoss = newAttack.GetComponent<AttackRangedKitsuneBoss>();
            attackRangedKitsuneBoss.target=actualEnemyController.target;
            auxSpawn+=1f;
        }
        Vector3 target = actualEnemyController.target.GetPosition();
        target.y=actualEnemyController.transform.position.y;
        actualEnemyController.transform.LookAt(target);
        if(time>rangedAttackTime){
            actualEnemyController.ChangeAction(new nullAction());
        } 
    }
    public KitSuneRangedAttack(float rangedAttackTime,GameObject prefabRangedAttack,ActualEnemyController actualEnemyController){
        this.actualEnemyController=actualEnemyController;
        this.rangedAttackTime=rangedAttackTime;
        this.rangedAttackPrefab=prefabRangedAttack;
    }

}

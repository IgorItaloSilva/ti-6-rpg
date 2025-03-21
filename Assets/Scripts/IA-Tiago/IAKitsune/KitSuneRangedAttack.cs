using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitSuneRangedAttack : EnemyActions
{
    float spawnDelay = .3f;
    float time;
    float spawnTime;
    float releaseTime=2f;
    GameObject rangedAttackPrefab;
    KitsuneController kitsuneController;
    int nBallsToSpawn;
    int vectorIndex;
    GameObject[]bolas;
    bool hasReleased;
    
    public override void EnterAction()
    {
        Debug.Log("Entrei numa ação de magia");
        kitsuneController.animator.SetTrigger("isCasting");
        kitsuneController.rb.constraints=RigidbodyConstraints.FreezeAll;
        kitsuneController.isCasting = true;
        hasReleased=false;
        time=0;
        spawnTime=spawnDelay;
        vectorIndex=0;
        nBallsToSpawn=kitsuneController.rangedAttackPos.Length;
    }

    public override void ExitAction()
    {
        Debug.Log("Sai duma ação de magia");
        kitsuneController.rb.constraints=RigidbodyConstraints.FreezeRotation;
    }

    public override void UpdateAction()
    {
        time+=Time.fixedDeltaTime;
        if(nBallsToSpawn>0){
            if(time>spawnTime){
                GameObject clone = GameObject.Instantiate(rangedAttackPrefab,  kitsuneController.rangedAttackPos[vectorIndex].position,
                                                             kitsuneController.rangedAttackPos[vectorIndex].rotation);
                bolas[vectorIndex]=clone;
                vectorIndex++;
                spawnTime=spawnTime+spawnDelay;
                nBallsToSpawn--;
            }
        }
        if(time>releaseTime&&!hasReleased){
            for(int i=0;i<kitsuneController.rangedAttackPos.Length;i++){
                bolas[i].GetComponent<AttackRangedKitsuneBoss>().SetTargetAndGo(kitsuneController.target);
            }
            hasReleased=true;
        }
        if(time>animationDuration){
            kitsuneController.ChangeAction(new nullAction());
        } 
    }
    public KitSuneRangedAttack(float rangedAttackTime,GameObject prefabRangedAttack,KitsuneController kitsuneController){
        this.kitsuneController=kitsuneController;
        this.animationDuration=rangedAttackTime;
        this.rangedAttackPrefab=prefabRangedAttack;
        bolas = new GameObject[kitsuneController.rangedAttackPos.Length];
    }

}

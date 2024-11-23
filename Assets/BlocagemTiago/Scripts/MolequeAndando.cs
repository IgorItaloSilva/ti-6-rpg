using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolequeAndando : ActualEnemyController
{
    bool isDead;
    [SerializeField]GameObject[] path;
    [SerializeField]int pointInPath;
    [SerializeField]int direction;
    Vector3 destination;
    void OnEnable(){
        GameEventsManager.instance.mapaTiagoEvents.onCutScenePlayed+=Dies;
    }
    void OnDisable(){
        GameEventsManager.instance.mapaTiagoEvents.onCutScenePlayed-=Dies;
    }
    protected override void AdditionalStart()
    {
        pointInPath = 0;
        direction=1;
    }

    protected override void CreateActions()
    {
        
    }
    void Dies(){
        transform.position = new Vector3(transform.position.x,-3.89f,transform.position.z);
        transform.rotation = new Quaternion(-0.659181058f,0.255891263f,-0.255891263f,0.659181058f);
        rb.constraints=RigidbodyConstraints.FreezeAll;
        isDead=true;
        steeringManager=null;
    }
    protected override void SetSteeringTargetAndCurrentAction(){
        if(isDead)return;
        if(target!=null){
            return;
        }
        destination = path[pointInPath+direction].transform.position;
        steeringManager.Seek(destination);
        if(Vector3.SqrMagnitude(transform.position-destination)<2){
            pointInPath+=direction;
            if((pointInPath+direction)>=path.Length||(pointInPath+direction)<0){
                direction=-direction;
            }
        }
    }
}

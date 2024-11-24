using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolequeAndando : ActualEnemyController
{
    [SerializeField]GameObject[] path;
    [SerializeField]int pointInPath;
    [SerializeField]int direction;
    Vector3 destination;
    protected override void AdditionalStart()
    {
        pointInPath = 0;
        direction=1;
    }

    protected override void CreateActions()
    {
        
    }
    protected override void SetSteeringTargetAndCurrentAction(){
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

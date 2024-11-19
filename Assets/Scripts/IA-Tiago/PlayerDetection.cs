
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(SphereCollider))]
public class PlayerDetection : MonoBehaviour
{
    ActualEnemyController actualEnemyController;
    bool alreadyFoundPlayer;

    public void Start(){
        actualEnemyController = GetComponentInParent<ActualEnemyController>();
        if(!actualEnemyController){
            Debug.LogWarning("Um playerDetection Collider não achou o enemyController");
        }
    }
    public void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            ISteeringAgent steeringAgent = other.GetComponent<ISteeringAgent>();
            if(steeringAgent!=null){
                actualEnemyController?.SetTarget(steeringAgent);
                alreadyFoundPlayer=true;
            }
        }
    }
    public void OnTriggerStay(Collider other){
        if(alreadyFoundPlayer)return;
        if(other.CompareTag("Player")){
            ISteeringAgent steeringAgent = other.GetComponent<ISteeringAgent>();
            if(steeringAgent!=null){
                actualEnemyController?.SetTarget(steeringAgent);
                alreadyFoundPlayer=true;
            }
        }
    }
    public void OnTriggerExit(Collider other){
        if(other.CompareTag("Player")){
            actualEnemyController?.SetTarget(null);
            alreadyFoundPlayer=false;
        }
    }
}

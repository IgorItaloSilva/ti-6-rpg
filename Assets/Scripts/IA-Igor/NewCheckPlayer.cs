using UnityEngine;

public class NewCheckPlayer : MonoBehaviour
{
    [SerializeField] EnemyBehaviour enemyBehave;


    public void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            enemyBehave.SetTarget(other.transform);
        }
    }  

    public void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")) {
            enemyBehave.SetTarget(this.transform);
        }
    }

}
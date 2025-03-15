using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    private SphereCollider _collider;
    public GameObject targetEnemy;
    
    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            targetEnemy = other.gameObject;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            targetEnemy = null;
            PlayerStateMachine.Instance.CameraTargetUnlock();
        }
    }
}

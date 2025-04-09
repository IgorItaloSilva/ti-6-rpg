using System;
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
        if (other.CompareTag("Enemy") && other.gameObject.activeInHierarchy)
        {
            PlayerStateMachine.Instance.InCombat = true;
            targetEnemy = other.gameObject;
        }
    }

    private void FixedUpdate()
    {
        if (targetEnemy && !targetEnemy.activeInHierarchy)
        {
            PlayerStateMachine.Instance.InCombat = false;
            targetEnemy = null;
            PlayerStateMachine.Instance.CameraTargetUnlock();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            PlayerStateMachine.Instance.InCombat = false;
            targetEnemy = null;
            PlayerStateMachine.Instance.CameraTargetUnlock();
        }
    }
}

using System;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    private SphereCollider _collider;
    public GameObject targetEnemy;
    private bool _inLineOfSight;
    private Vector3 _raycastDirection;
    private RaycastHit _raycastHit;
    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && other.gameObject.activeInHierarchy)
        {
            targetEnemy = other.gameObject;
            RaycastEnemyAsync();
            Debug.Log("In enemy range, looking for line of sight");
        }
    }

    private void FixedUpdate()
    {
        if ((targetEnemy) && !targetEnemy.activeInHierarchy)
        {
            ForgetEnemy();
        }
        if(!targetEnemy && _inLineOfSight)
        {
            ForgetEnemy();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            ForgetEnemy();
        }
    }

    private async void RaycastEnemyAsync()
    {
        while (targetEnemy && !_inLineOfSight)
        {
            _raycastDirection = targetEnemy.transform.position - transform.position;
            if (Physics.Raycast(transform.position, _raycastDirection, out _raycastHit))
            {
                Debug.Log("In enemy line of sight");
                if (_raycastHit.collider.CompareTag("Enemy"))
                {
                    _inLineOfSight = true;
                    PlayerStateMachine.Instance.InCombat = true;
                    return;
                }
            }
            await Task.Delay(250);
        }
    }

    private void ForgetEnemy()
    {
        PlayerStateMachine.Instance.InCombat = false;
        targetEnemy = null;
        _inLineOfSight = false;
        PlayerStateMachine.Instance.CameraTargetUnlock();
    }
}

using System;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    private SphereCollider _collider;
    public GameObject targetEnemy;
    private bool _inLineOfSight;
    private Vector3 _raycastDirection, _raycastOffset;
    private RaycastHit _raycastHit;
    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        _raycastOffset = new Vector3(0, 0.5f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && other.gameObject.activeInHierarchy)
        {
            targetEnemy = other.gameObject;
            RaycastEnemyAsync();
            Debug.Log("In enemy range, looking for line of sight");
            other.GetComponent<EnemyBehaviour>().SetTarget(transform.parent);
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
            _raycastDirection = targetEnemy.transform.position - transform.position + _raycastOffset;
            if (Physics.Raycast(transform.position, _raycastDirection, out _raycastHit))
            {
                if (_raycastHit.collider.CompareTag("Enemy"))
                {
                    Debug.Log("In enemy line of sight");
                    _inLineOfSight = true;
                    PlayerStateMachine.Instance.InCombat = true;
                    return;
                }
            }
            await Task.Delay(250);
        }
    }

    public void ForgetEnemy()
    {
        PlayerStateMachine.Instance.InCombat = false;
        targetEnemy = null;
        _inLineOfSight = false;
        PlayerStateMachine.Instance.CameraTargetUnlock();
    }
}

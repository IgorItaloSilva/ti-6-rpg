using System.Threading.Tasks;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    private SphereCollider _collider;
    public GameObject targetEnemy;
    private bool _inLineOfSight;
    private Vector3 _raycastDirection, _raycastOffset;
    private RaycastHit _raycastHit;
    [SerializeField] private LayerMask raycastLayerMask;
    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        _raycastOffset = new Vector3(0, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && other.gameObject.activeInHierarchy)
        {
            targetEnemy = other.gameObject;
            RaycastEnemyAsync();
            if(PlayerStateMachine.Instance.ShowDebugLogs) Debug.Log("In enemy range, looking for line of sight");
            other.GetComponent<EnemyBehaviour>().SetTarget(transform.parent);
            other.GetComponent<EnemyBehaviour>().DisplayBossInfoIfBoss();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            ForgetEnemy();
            targetEnemy = other.gameObject;
            other.GetComponent<EnemyBehaviour>().HideBossInfo();
        }
    }

    private void OnDrawGizmos()
    {
        if (targetEnemy && !_inLineOfSight)
        {
            Gizmos.color = Color.red;
            Vector3 raycastDirection = targetEnemy.transform.position - transform.position + _raycastOffset;
            Gizmos.DrawLine(transform.position, transform.position + raycastDirection);
        }
    }
    
    private async void RaycastEnemyAsync()
    {
        while (targetEnemy && !_inLineOfSight)
        {
            if(targetEnemy) _raycastDirection = targetEnemy.transform.position - transform.position + _raycastOffset;
            if (Physics.Raycast(transform.position, _raycastDirection, out _raycastHit, 25f,raycastLayerMask))
            {
                if (_raycastHit.collider.CompareTag("Enemy"))
                {
                    if(PlayerStateMachine.Instance.ShowDebugLogs)
                    {
                        Debug.Log("In enemy line of sight");
                    }
                    _inLineOfSight = true;
                    AudioPlayer.instance.PlayMusic("CombatMusic");
                    PlayerStateMachine.Instance.InCombat = true;
                    return;
                }
            }
            await Task.Delay(100);
        }
    }

    public void ForgetEnemy()
    {
        
        AudioPlayer.instance.PlayMusic("MainTheme");
        PlayerStateMachine.Instance.InCombat = false;
        PlayerStateMachine.Instance.Animator.SetBool(PlayerStateMachine.Instance.InCombatHash, false);
        targetEnemy.GetComponent<EnemyBehaviour>()?.SetTarget(null);
        targetEnemy = null;
        _inLineOfSight = false;
        PlayerStateMachine.Instance.CameraTargetUnlock();
    }
}

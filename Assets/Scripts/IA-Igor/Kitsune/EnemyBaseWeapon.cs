using System.Collections;
using UnityEngine;

public abstract class EnemyBaseWeapon : MonoBehaviour
{
    protected Coroutine coroutine;
    protected Transform target;

    [SerializeField] protected WeaponManager weaponManager;

    protected void Start()
    {
        target = PlayerStateMachine.Instance.transform;
        weaponManager = GetComponentInChildren<WeaponManager>();
        CustomStart();
    }

    protected virtual void CustomStart() {  }

    void OnEnable() {
        OneExecution();
        coroutine = StartCoroutine(MultipleExecution());
    }

    protected virtual void OneExecution(){  }

    protected virtual IEnumerator MultipleExecution() { yield return null; }


}
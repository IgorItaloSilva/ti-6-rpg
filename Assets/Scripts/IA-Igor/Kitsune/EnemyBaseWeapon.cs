using System.Collections;
using UnityEngine;

public abstract class EnemyBaseWeapon : MonoBehaviour
{
    protected Coroutine coroutine;
    protected Transform target;

    void Start()
    {
        target = PlayerStateMachine.Instance.transform;
    }

    void OnEnable() {
        OneExecution();
        coroutine = StartCoroutine(MultipleExecution());
    }

    protected virtual void OneExecution(){  }

    protected virtual IEnumerator MultipleExecution() { yield return new WaitForSeconds(0); }


}
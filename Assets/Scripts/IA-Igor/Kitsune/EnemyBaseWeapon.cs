using System.Collections;
using UnityEngine;

public abstract class EnemyBaseWeapon : MonoBehaviour
{
    Coroutine coroutine;
    Transform target;

    void OnEnable() {
        OneExecution();
        coroutine = StartCoroutine(MultipleExecution());
    }

    protected virtual void OneExecution(){ transform.LookAt(target); }

    protected virtual IEnumerator MultipleExecution() { yield return new WaitForSeconds(0); }

    public void SetTarget(Transform target){ this.target = target; }

}
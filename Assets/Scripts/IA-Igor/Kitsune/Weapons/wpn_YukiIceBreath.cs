using System.Collections;
using UnityEngine;


public class wpn_YukiIceBreath : EnemyBaseWeapon
{
    Coroutine thisCoroutine, damageCoroutine;
    [SerializeField] BoxCollider boxCollider;
    [SerializeField] Vector3[] colliderCenter, colliderSizes;
    float timer;
    [SerializeField] float disableSeconds;
    [SerializeField] bool halfAttack;


    protected override void CustomStart()
    {
        timer = 0;
    }

    void OnEnable()
    {
        halfAttack = false;
        timer = 0;
        thisCoroutine = StartCoroutine(Disable());
        damageCoroutine = StartCoroutine(DamageEnable());
    }

    void Update()
    {
        if (!halfAttack)
        {
            boxCollider.center = Vector3.Lerp(colliderCenter[0], colliderCenter[1], timer);
            boxCollider.size = Vector3.Lerp(colliderSizes[0], colliderSizes[1], timer);
        }
        else
        {
            boxCollider.center = Vector3.Lerp(colliderCenter[1], colliderCenter[2], timer);
            boxCollider.size = Vector3.Lerp(colliderSizes[1], colliderSizes[2], timer);
        }

        if (timer < 1)
            timer += Time.deltaTime * 1.1f;
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(disableSeconds / 2);
        timer = 0;
        halfAttack = true;
        yield return new WaitForSeconds(disableSeconds / 2);
        StopCoroutine(DamageEnable());
        gameObject.SetActive(false);

    }

    IEnumerator DamageEnable()
    {
        while (true) {
            yield return new WaitForSeconds(0.5f);
            weaponManager.EnableCollider();
        }
    }

}
using UnityEngine;
using System.Threading.Tasks;

public class wpn_KitsuneAoe : EnemyBaseWeapon
{
    Coroutine coroutine;
    [SerializeField] SphereCollider collider;
    [SerializeField] float sizeSpeed;
    sbyte time;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        collider.radius = 1.5f;
        Wait();
    }

    // Update is called once per frame
    void Update()
    {
        if (collider.enabled)
        {
            collider.radius += sizeSpeed * Time.deltaTime;
        }
    }

    async void Wait()
    {
        await Task.Delay(2500);
        time = 2;
        do {
            collider.enabled = true;
            await Task.Delay(400);
            collider.radius = 1.5f;
            time--;
        } while (time >= 0);
        await Task.Delay(400);
        collider.enabled = false;
        gameObject.SetActive(false);

    }
    
}
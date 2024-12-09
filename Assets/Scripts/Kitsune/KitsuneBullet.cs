using UnityEngine;


public class KitsuneBullet : MonoBehaviour
{
    Vector3 player;
    [SerializeField] Rigidbody rb;
    [SerializeField]float damage;

    float timer = 3f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * 10f;
        Destroy(gameObject,5f);
    }

    void FixedUpdate()
    {
        if(timer <= 0)
        {
            Vector3 dir = Vector3.Slerp(transform.position, player, 0.85f);
            transform.LookAt(dir);
            rb.velocity = transform.forward * 20f;
            return;
        }
        timer -= Time.fixedDeltaTime;
    }

    public void SetPlayer(Vector3 _player)
    {
        player = _player;
    }
    public void OnTriggerEnter(Collider collider){
        if(collider.CompareTag("Player")){
            IDamagable damagable = collider.GetComponent<IDamagable>();
            if(damagable==null){
                Debug.Log("a bullet n conseguiu dar dano no player");
            }
            else{
                damagable.TakeDamage(damage,Enums.DamageType.Magic);
                Destroy(gameObject);
            }
        }
    }

}
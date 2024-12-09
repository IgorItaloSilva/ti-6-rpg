using UnityEngine;


public class KitsuneBullet : MonoBehaviour
{
    Transform player;
    [SerializeField] Rigidbody rb;

    float timer = 3f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * 10f;
    }

    void FixedUpdate()
    {
        if(timer <= 0)
        {
            Vector3 dir = Vector3.Slerp(transform.position, player.position, 0.85f);
            transform.LookAt(dir);
            rb.velocity = transform.forward * 7f;
            return;
        }
        timer -= Time.fixedDeltaTime;
    }

    public void SetPlayer(Transform _player)
    {
        player = _player;
    }

}
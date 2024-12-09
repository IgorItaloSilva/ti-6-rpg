using UnityEngine;


public class KitsuneBullet : MonoBehaviour
{
    Vector3 player;
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

}
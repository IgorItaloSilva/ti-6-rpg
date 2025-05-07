using UnityEngine;

public class KitsuneMagicBullet : MonoBehaviour
{
    Transform target;
    Rigidbody rb;

    float speed = 6;


    bool isClose = false;
    Vector3 moviment;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        isClose = false;
    }

    void Update() { 
        if(!isClose) {
            transform.LookAt(target);
            if(Vector3.Distance(target.position, transform.position) < 1)
                isClose = true;
        }
        transform.position += transform.forward * speed * Time.deltaTime;
        
        
    }

    public void SetTarget(Transform target){ this.target = target; }

    void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Enemy")){
            gameObject.SetActive(false);
        }
        
    }

}
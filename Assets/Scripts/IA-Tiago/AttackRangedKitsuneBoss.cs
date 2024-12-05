using UnityEngine;

public class AttackRangedKitsuneBoss : MonoBehaviour
{
    public ISteeringAgent target;
    Vector3 destination;
    bool canMove;
    [SerializeField]float speed;
    [SerializeField]float damage;
    void Start()
    {
        Invoke("SetDestinationAndGo",.5f);
        Invoke("Die",2f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(canMove){
            Vector3 dir = destination-transform.position;
            transform.position+=dir*(speed*Time.fixedDeltaTime);
        }
    }
    void SetDestinationAndGo(){
        destination=target.GetPosition()+new Vector3(0,1,0);
        canMove=true;
    }
    public void OnCollisionEnter(Collision collision){
        Debug.Log("Collidi com algo");
        if(collision.collider.CompareTag("Player")){
            PlayerStats playerStats = collision.collider.GetComponent<PlayerStats>();
            playerStats.TakeDamage(damage,Enums.DamageType.Magic);
        }
        Debug.Log("Colidi com um "+collision.collider.name);
        Destroy(gameObject);
    }
    void Die(){
        Destroy(gameObject);
    }
}

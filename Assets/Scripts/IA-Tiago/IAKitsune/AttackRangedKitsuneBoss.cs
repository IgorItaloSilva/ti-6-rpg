using UnityEngine;

public class AttackRangedKitsuneBoss : MonoBehaviour
{
    public ISteeringAgent target;
    bool canMove;
    [SerializeField]float speed;
    [SerializeField]float damage;
    [SerializeField]float timeToBreak=4f;
    bool stopUpdatingPos = false;
    Vector3 dir;
    void Start()
    {
        Invoke("Die",timeToBreak);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(canMove){
            if(!stopUpdatingPos){
                dir = target.GetPosition()-transform.position;
            }
            if(dir.magnitude<5&&!stopUpdatingPos){
                stopUpdatingPos=true;
            }
            transform.position+=dir.normalized*(speed*Time.fixedDeltaTime);
        }
    }
    public void SetTargetAndGo(ISteeringAgent target){
        this.target=target;
        canMove=true;
    }
    public void OnTriggerEnter(Collider collider){
        if(collider.CompareTag("EnemyDetection"))return;
        Debug.Log("Colidi com um "+collider.name);
        if(collider.CompareTag("Player")){
            PlayerStats playerStats = collider.GetComponent<PlayerStats>();
            playerStats.TakeDamage(damage,Enums.DamageType.Magic,false);
        }
        Destroy(gameObject);
    }
    void Die(){
        Debug.Log("Acabou meu tempo");
        Destroy(gameObject);
    }
}

using UnityEngine;

public class KIllPlayerWhenFallOutMap : MonoBehaviour
{
    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if(playerStats!=null){
                playerStats.TakeDamage(99999,Enums.DamageType.Regular,false);
            }
            
        }
    }
}

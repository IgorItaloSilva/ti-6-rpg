using UnityEngine;

public class KIllPlayerWhenFallOutMap : MonoBehaviour
{
     void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.Die();
            }

        }
    } 
     void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("o player colidiu com a agua");
            PlayerStats playerStats = collision.collider.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.Die();
            }

        }
    }
}

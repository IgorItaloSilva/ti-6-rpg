using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestEnemie : MonoBehaviour
{
    [SerializeField] GameObject pilar;


    // Start is called before the first frame update
    void Start()
    {
        KitsuneForestObserver.instance.AddNumEnemies();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            pilar.SetActive(true);
            KitsuneForestObserver.instance.EnemyDefeated();
        }
    }
}

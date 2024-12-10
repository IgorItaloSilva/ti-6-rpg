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


    private void OnDestroy()
    {
        pilar?.SetActive(true);
        KitsuneForestObserver.instance.EnemyDefeated();
    }
}
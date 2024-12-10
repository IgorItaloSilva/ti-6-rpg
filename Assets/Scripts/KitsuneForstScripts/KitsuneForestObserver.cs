using UnityEngine;


public class KitsuneForestObserver : MonoBehaviour
{
    public static KitsuneForestObserver instance;
    [SerializeField] int numEnemies;
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject statue;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        numEnemies = 0;
    }

    public void EnemyDefeated()
    {
        numEnemies--;
        if(numEnemies == 0)
        {
            statue?.SetActive(false);
            boss?.SetActive(true);
        }
    }

    public void AddNumEnemies()
    {
        numEnemies++;
    }
}

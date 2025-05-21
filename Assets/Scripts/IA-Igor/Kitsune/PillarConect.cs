using UnityEngine;

public class PillarConect : MonoBehaviour
{
    [SerializeField] int pillarID;
    void OnEnable()
    {
        GameEventsManager.instance.levelEvents.OnEnemyDied += ActivatePillar;
    }
    void OnDisable()
    {
        
        GameEventsManager.instance.levelEvents.KitsuneDeath(pillarID);
        GameEventsManager.instance.levelEvents.OnEnemyDied -= ActivatePillar;
    }
    void ActivatePillar(int enemyType)
    {
        /* if (enemyType == (int)EnemyBehaviour.EnemyType.kitsune)
        {
            GameEventsManager.instance.levelEvents.KitsuneDeath(pillarID);
        } */
    }
}

using UnityEngine;

public class KitsuneBossDeathAux : MonoBehaviour
{
    [SerializeField] GameObject portalNextLevel;
    [SerializeField] GameObject dialogInteractableBoss;
    public void Activate(EnemyBehaviour enemyBehaviour)
    {
        portalNextLevel.SetActive(true);
        dialogInteractableBoss.SetActive(true);
    }
}

using UnityEngine;

public class KitsuneBossDeathAux : MonoBehaviour
{
    [SerializeField] GameObject portalNextLevel;
    [SerializeField] GameObject dialogInteractableBoss;
    public void Activate()
    {
        portalNextLevel.SetActive(true);
        dialogInteractableBoss.SetActive(true);
    }
}

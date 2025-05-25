using UnityEngine;

public class ConfiguracaoManager : MonoBehaviour
{
    bool toggle;
    [SerializeField] GameObject painelConfig;
    void Start()
    {
        painelConfig.SetActive(false);
        toggle = false;
    }
    public void ToggleConfig()
    {
        if (toggle)
        {
            toggle = false;
            painelConfig.SetActive(false);
            DataPersistenceManager.instance.SaveGame();
        }
        else
        {
            toggle = true;
            painelConfig.SetActive(true);
        }
    }
}

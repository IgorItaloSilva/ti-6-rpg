using UnityEngine;


[DefaultExecutionOrder(-50)]
public class SetCamTarget : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        GameManager.instance.camTarget = transform;
        if (GameManager.instance.camTarget)
            Destroy(this);
        InvokeRepeating("StartGame", 0.1f, 0.1f);
    }
}

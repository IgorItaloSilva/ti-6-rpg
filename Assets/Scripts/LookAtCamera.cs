using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera mainCam;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(mainCam.transform);
    }
}

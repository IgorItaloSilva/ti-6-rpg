using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenShotTaker : MonoBehaviour
{
    [SerializeField] string fileName = "foto";
    [SerializeField] int superSize = 1;
    string path;
    [SerializeField]PhotoCounter photoCounter;
    void Awake()
    {
        path = "Screenshots/" + fileName+photoCounter.nphotos.ToString()+ ".jpeg";
    }
    void Update()
    {
        if (Keyboard.current.f12Key.wasPressedThisFrame)
        {
            ScreenCapture.CaptureScreenshot(path, superSize);
            photoCounter.nphotos++;
            path = "Screenshots/" + fileName+photoCounter.nphotos.ToString()+ ".jpeg";
        }
    }
}

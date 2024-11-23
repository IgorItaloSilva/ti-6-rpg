using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnviromentController : MonoBehaviour
{
    [SerializeField]Material skyboxBoa;
    [SerializeField]Material skyboxRuim;
    [SerializeField]Light sun;
    [SerializeField]CinemachineFreeLook cinemachineFreeLook;
    [SerializeField]Animation treeAnimation;

    void Start(){
        RenderSettings.skybox = skyboxBoa;
        sun.intensity=1;
        GameEventsManager.instance.mapaTiagoEvents.ActivateQuest(0);
    }
    void Update(){
        if(Keyboard.current.numpad3Key.wasPressedThisFrame){
            PlayCutScene();
        }
    }
    public void PlayAnimation(){
        treeAnimation.Play();
    }
    void PlayCutScene(){
        RenderSettings.skybox = skyboxRuim;
        sun.intensity=.3f;
        RenderSettings.fog=true;
        cinemachineFreeLook.m_Lens.FarClipPlane=50;
        GameEventsManager.instance.mapaTiagoEvents.PlayCutScene();
        GameEventsManager.instance.mapaTiagoEvents.ActivateQuest(2);
    }
}

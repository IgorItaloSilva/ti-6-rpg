using UnityEngine;
using UnityEngine.UI;



public class SetVolumeSlider : MonoBehaviour
{
    [SerializeField] Slider[] volumeSliders;
    [SerializeField] Button btnShake;


    void Start()
    {
        GameManager.instance.audioManager.SetSliders(volumeSliders, btnShake);
        Destroy(this);
    }

}

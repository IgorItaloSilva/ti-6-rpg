using UnityEngine;
using UnityEngine.UI;



public class SetVolumeSlider : MonoBehaviour
{
    [SerializeField] Slider[] volumeSliders;
    [SerializeField] Button btnShake;


    void Start()
    {
        GameManager.gm.audioManager.SetSliders(volumeSliders, btnShake);
        Destroy(this);
    }

}

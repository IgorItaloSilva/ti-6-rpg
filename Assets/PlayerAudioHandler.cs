using UnityEngine;

public class PlayerAudioHandler : MonoBehaviour
{
    public void PlayFootstepSound()
    {
        AudioPlayer.instance?.PlayFootstepSound();
    }

    public void PlayDrawKatanaSound()
    {
        AudioPlayer.instance?.PlaySFX("DrawKatana");
    }

    public void PlaySheathKatanaSound()
    {
        AudioPlayer.instance?.PlaySFX("SheathKatana");
    }
}

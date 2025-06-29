using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndingDisplayer : MonoBehaviour
{
    public Ending ending;
    public Image backgroundImage;
    public TextMeshProUGUI txtEndingTitle;
    public TextMeshProUGUI txtEndingPhrase;

    private void OnEnable()
    {
        if (ending)
        {
            txtEndingTitle.text = ending.endingTitle;
            txtEndingTitle.color = ending.endingColor;
            txtEndingPhrase.text = ending.endingPhrase;
            backgroundImage.sprite = Sprite.Create(ending.backgroundImage,
                new Rect(0, 0, ending.backgroundImage.width, ending.backgroundImage.height), new Vector2(0.5f, 0.5f));
            AudioPlayer.instance.PlayMusic(ending.endingMusicName);
        }
        else
        {
            Debug.Log("Ending is not set in EndingDisplayer.");
        }
    }
}

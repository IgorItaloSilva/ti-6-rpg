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
    public TextMeshProUGUI txtEndingStory;
    public GameObject panelEndingStory;
    public GameObject panelCreditos;
    private byte _currentStoryIndex;

    private void OnEnable()
    {
        PlayerStateMachine.Instance.LockPlayer();
        panelEndingStory.SetActive(false);
        panelCreditos.SetActive(false);
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

    private void Update()
    {
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            if (_currentStoryIndex > ending.endingStory.Length - 1)
            {
                panelCreditos.SetActive(true);
            }
            if(_currentStoryIndex > ending.endingStory.Length)
            {
                // If the story is finished, disable the ending display
                panelEndingStory.SetActive(false);
                panelCreditos.SetActive(false);
                AudioPlayer.instance.PlayMusic("MainTheme");
                gameObject.transform.parent.gameObject.SetActive(false);
                PlayerStateMachine.Instance.UnlockPlayer();
                return;
            }
            
            if(_currentStoryIndex == 0)
                panelEndingStory.SetActive(true);
            
            if(_currentStoryIndex < ending.endingStory.Length)
            {
                txtEndingStory.text = ending.endingStory[_currentStoryIndex];
            }
            _currentStoryIndex++;

        }
    }
}

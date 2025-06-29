using UnityEngine;

[CreateAssetMenu(fileName = "Ending", menuName = "ScriptableObjects/Ending")]
public class Ending : ScriptableObject
{
    [SerializeField] public Texture2D backgroundImage;
    [SerializeField] public string endingTitle;
    [SerializeField] public string endingPhrase;
    [SerializeField] public string[] endingStory;
    public Color endingColor;
    public string endingMusicName;
}

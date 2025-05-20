using UnityEngine;
[CreateAssetMenu(fileName = "TutorialSO", menuName = "ScriptableObjects/TutorialSO")]
public class TutorialSO : ScriptableObject
{
    [field: SerializeField]public Sprite[] Sprites { get; private set; }
    [field: SerializeField]public string Title { get; private set; }
    [field: SerializeField]public string[] Texts { get; private set; }
}

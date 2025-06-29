using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HintDisplayer : MonoBehaviour
{
    public Hint[] hints;
    public TextMeshProUGUI text;

    private void Awake()
    {
        text.text = hints[Random.Range(0, hints.Length)].HintText;
    }
}

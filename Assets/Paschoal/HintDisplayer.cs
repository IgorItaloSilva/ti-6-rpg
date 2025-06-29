using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HintDisplayer : MonoBehaviour
{
    public Hint[] hints;
    public TextMeshProUGUI text;

    private void Awake()
    {
        SceneManager.sceneLoaded += ChangeTip;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= ChangeTip;
    }

    private void ChangeTip(Scene scene, LoadSceneMode mode) 
    {
        text.text = hints[Random.Range(0, hints.Length)].HintText;
    }
    

}

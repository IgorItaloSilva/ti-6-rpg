using UnityEngine;
using UnityEngine.SceneManagement;

public class SingleLevelLoaderDesespero : MonoBehaviour
{
    public void LoadCena(string cena)
    {
        SceneManager.LoadScene(cena);
        SceneManager.LoadSceneAsync("Hud", LoadSceneMode.Additive);
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;



public class InitGame : MonoBehaviour
{
    [SerializeField] string[] scenes; // Lista das cenas inicias do projeto(Menu inicial e hud)


    void Start()
    {
        for(int i = 1; i < scenes.Length; i++)
            LoadScene(scenes[i]);
        StartCoroutine("UnloadScene", scenes[0]);
    }

    private void LoadScene(string sceneName)
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        Debug.Log(asyncOp);
        while (!asyncOp.isDone)
            return;
    }

    private IEnumerator UnloadScene(string sceneName)
    {
        yield return new WaitForSeconds(0.1f);
        AsyncOperation asyncOp = SceneManager.UnloadSceneAsync(sceneName);
        Debug.Log(asyncOp);
        while (!asyncOp.isDone)
            yield return null;
        Destroy(gameObject);
    }
}

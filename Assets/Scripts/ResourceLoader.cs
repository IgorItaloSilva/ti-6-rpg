using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ResourceLoader : MonoBehaviour
{
    [SerializeField] GameObject terrainParent;
    GameObject loadedAsset;
    SphereCollider meuCollider;
    bool alreadyLoaded;
    void Awake()
    {
        meuCollider = gameObject.GetComponent<SphereCollider>();
        meuCollider.radius = 10f;
        meuCollider.isTrigger = true;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            StartCoroutine(UnloadResource());
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&!alreadyLoaded)
        {
            StartCoroutine(LoadTerrainAsync());
            alreadyLoaded = true;
        }
    }
    IEnumerator LoadTerrainAsync()
    {
        ResourceRequest resourceRequest = Resources.LoadAsync<GameObject>("KitsuneMapParts/PaiTerrenoEsquiso");
        while (!resourceRequest.isDone)
        {
            yield return null;
        }
        loadedAsset = Instantiate(resourceRequest.asset, terrainParent.transform) as GameObject;
        
    }
    IEnumerator UnloadResource()
    {
        Destroy(loadedAsset);
        AsyncOperation op = Resources.UnloadUnusedAssets();
        while (!op.isDone)
        {
            yield return null;
        }
        Debug.Log("Terminei de limpar os unusedAssets");
    }
}

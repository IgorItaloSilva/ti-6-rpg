using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ResourceLoader : MonoBehaviour
{
    [SerializeField] GameObject terrainParent; 
    GameObject loadedAsset;
    SphereCollider meuCollider;
    bool alreadyLoaded;
    void Awake() //feito só pra configurar o collider automaticamente
    {
        meuCollider = gameObject.GetComponent<SphereCollider>();
        meuCollider.radius = 10f;
        meuCollider.isTrigger = true;
    }
    void Update() //Debug pra testar se o UnloadFunciona
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
        //Load resource
        ResourceRequest resourceRequest = Resources.LoadAsync<GameObject>("KitsuneMapParts/PaiTerrenoEsquiso");
        while (!resourceRequest.isDone)
        {
            yield return null;
        }
        //Instantiate guardando a referencia para destruir depois
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

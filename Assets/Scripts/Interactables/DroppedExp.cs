using TMPro;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class DroppedExp : MonoBehaviour,IDataPersistence
{
    public static DroppedExp instance;
    int expAmmount;
    Vector3 pos;
    bool isActive;
    [SerializeField]GameObject visuals;
    SphereCollider sphereCollider;
    void OnEnable(){
        GameEventsManager.instance.playerEvents.onPlayerRespawned+=Activate;
    }
    void OnDisable(){
        GameEventsManager.instance.playerEvents.onPlayerRespawned-=Activate;
    }
    void Awake(){//SINGLETON INVERTIDO ONDE O MAIS ANTIGO DESTROI O MAIS NOVO
        if(instance!=null){
            Destroy(instance.gameObject);
        }
        instance=this;
    }
    public void Start(){
        sphereCollider=GetComponent<SphereCollider>();
        visuals.SetActive(isActive);
        sphereCollider.enabled=isActive;
    }
    protected  void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player")){
            GameEventsManager.instance.playerEvents.PlayerGainExp(expAmmount);
            expAmmount=0;
            isActive=false;
            visuals.SetActive(false);
            sphereCollider.enabled=false;
            DataPersistenceManager.instance?.SaveGame();
        }
    }
    public  void LoadData(GameData gameData)
    {
        isActive = gameData.droppedExpData.isActive;
        if(isActive){
            expAmmount= gameData.droppedExpData.expAmmount;
            pos = gameData.droppedExpData.pos;
            transform.position=pos;
        }
    }
    public  void SaveData(GameData gameData)
    {
        gameData.droppedExpData.expAmmount=expAmmount;
        gameData.droppedExpData.pos=pos;
        gameData.droppedExpData.isActive=isActive;
    }
    public void SetVariablesAndPos(int expStored, Vector3 pos){//chamado quando o jogador morre
        expAmmount=expStored;
        if(expAmmount>0){
            this.pos=pos;
            visuals.SetActive(true);
            sphereCollider.enabled=false;
            transform.position=pos;
        }
        else{
            visuals.SetActive(false);
        }
    }
    void Activate(){
        if(expAmmount>0){
            isActive=true;
        }
        else{
            isActive=false;
        }
        sphereCollider.enabled=isActive;
    }
}

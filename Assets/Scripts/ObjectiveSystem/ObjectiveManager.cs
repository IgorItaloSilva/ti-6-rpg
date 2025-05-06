using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour,IDataPersistence
{
    public static ObjectiveManager instance;
    [SerializeField]List<ObjectiveSO> allQuests;
    [SerializeField]GameObject objectiveGO;
    string activeQuestId;
    [SerializeField] SerializableDictionary<string,ObjectiveData>objectivesData;
    ObjectiveData auxObjectiveData;
    [SerializeField] SerializableDictionary<string,ObjectiveSO>allQuestsDictionary;
    void Awake(){
        if(instance==null){
            instance=this;
        }
        else{
            Destroy(gameObject);
        }
        objectivesData = new SerializableDictionary<string, ObjectiveData>();
        allQuestsDictionary=new SerializableDictionary<string, ObjectiveSO>();
        if(allQuests.Count==0){
            Debug.LogWarning("Não temos nenhuma quest cadastrada na lista de todas as quests");
        }
        else{
            foreach(ObjectiveSO objectiveSO in allQuests){
                if(objectiveSO==null){
                    Debug.Log("Tem uma quest nula na lista de todas as quests, dando continue");
                    continue;
                }
                else{
                    allQuestsDictionary.Add(objectiveSO.Id,objectiveSO);
                }
            }
        }
    }

    public void LoadData(GameData gameData)
    {
        objectivesData = gameData.objectivesData;
        
        if(objectivesData.Count>0)
        foreach(string s in objectivesData.Keys){
            if(s==null){Debug.Log("Por algum motivo tentamos dar laod numa key do objectives data que é nula, dando um continue");continue;}
            if(objectivesData.TryGetValue(s,out auxObjectiveData)!=false){
                if(auxObjectiveData.hasStarted&&!auxObjectiveData.hasFinished){
                    LoadQuest(allQuestsDictionary[s],auxObjectiveData.stringData);
                }
            }
            else{
                Debug.Log($"Por algum motivo deu errado ler um objective data da string {s}, dando um continue");
                continue;
            }
        }
    }

    public void SaveData(GameData gameData)
    {
        gameData.objectivesData = objectivesData;
    }
    public void StartQuest(ObjectiveSO objectiveSO){
        GameObject newObjectiveInstantiableGO = Instantiate(objectiveGO);
        ObjectiveInstantiable newObjectiveInstantiable=newObjectiveInstantiableGO.GetComponent<ObjectiveInstantiable>();
        newObjectiveInstantiable.Settup(objectiveSO);
        newObjectiveInstantiable.StartObjective();
    }
    void LoadQuest(ObjectiveSO objectiveSO,string stringData){
        GameObject newObjectiveInstantiableGO = Instantiate(objectiveGO);
        ObjectiveInstantiable newObjectiveInstantiable=newObjectiveInstantiableGO.GetComponent<ObjectiveInstantiable>();
        newObjectiveInstantiable.Settup(objectiveSO);
        newObjectiveInstantiable.StartObjective();
        newObjectiveInstantiable.LoadObjective(stringData);
    }
    public void UpdateQuestData(string id,ObjectiveData objectiveData){
        if(objectivesData.ContainsKey(id)){

        }
        else{
            objectivesData.Add(id,objectiveData);
        }
    }
}

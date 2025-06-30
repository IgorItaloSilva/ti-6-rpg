using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour,IDataPersistence
{
    public static ObjectiveManager instance;
    [Header("COLOCAR TODOS OS OBJETIVOS AQUI")]
    [SerializeField]List<ObjectiveSO> allQuests;
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
        /* foreach(KeyValuePair<string,ObjectiveData> keyValuePair in objectivesData){
            if(keyValuePair.Key==null){Debug.Log("Por algum motivo tentamos dar laod numa key do objectives data que é nula, dando um continue");continue;}
            if(keyValuePair.Value.hasStarted&&!keyValuePair.Value.hasFinished){
                    LoadQuest(allQuestsDictionary[keyValuePair.Key],keyValuePair.Value.stringData);
                }
        } */
        //versão antiga e errada 
        foreach(string s in objectivesData.Keys.ToArray()){
            if(s==null){Debug.Log("Por algum motivo tentamos dar laod numa key do objectives data que é nula, dando um continue");continue;}
            if(objectivesData.TryGetValue(s,out auxObjectiveData)!=false){
                ObjectiveUiManager.instance?.CreateButton(allQuestsDictionary[s],auxObjectiveData);
                if(auxObjectiveData.hasStarted&&!auxObjectiveData.hasFinished){
                    LoadQuest(allQuestsDictionary[s],auxObjectiveData.stringData);
                    GameEventsManager.instance.objectiveEvents.StartObjective(s);
                }
                if(auxObjectiveData.hasFinished){
                    GameEventsManager.instance.objectiveEvents.CompleteObjective(s);
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
        GameObject newObjectiveInstantiableGO = Instantiate(objectiveSO.ObjectivePrefab);
        ObjectiveInstantiable newObjectiveInstantiable=newObjectiveInstantiableGO.GetComponent<ObjectiveInstantiable>();
        ObjectiveUiManager.instance?.CreateButton(objectiveSO,new ObjectiveData(true));
        newObjectiveInstantiable.Settup(objectiveSO);
        newObjectiveInstantiable.StartObjective();
    }
    void LoadQuest(ObjectiveSO objectiveSO,string stringData){
        GameObject newObjectiveInstantiableGO = Instantiate(objectiveSO.ObjectivePrefab);
        ObjectiveInstantiable newObjectiveInstantiable=newObjectiveInstantiableGO.GetComponent<ObjectiveInstantiable>();
        newObjectiveInstantiable.Settup(objectiveSO);
        newObjectiveInstantiable.StartObjective();
        newObjectiveInstantiable.LoadObjective(stringData);
    }
    public void UpdateQuestData(string id, ObjectiveData objectiveData)
    {
        ObjectiveUiManager.instance?.UpdateData(id, objectiveData);
        if (objectivesData.ContainsKey(id))
        {
            objectivesData[id] = objectiveData;
        }
        else
        {
            objectivesData.Add(id, objectiveData);
        }
        //DataPersistenceManager.instance?.SaveGame();
    }
}

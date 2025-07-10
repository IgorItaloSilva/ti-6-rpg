using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour, IDataPersistence
{
    public static ObjectiveManager instance;
    [Header("COLOCAR TODOS OS OBJETIVOS AQUI")]
    [SerializeField] List<ObjectiveSO> allQuests;
    [SerializeField] SerializableDictionary<string, ObjectiveData> objectivesData;
    ObjectiveData auxObjectiveData;

    [SerializeField] SerializableDictionary<string, ObjectiveSO> allQuestsDictionary;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        objectivesData = new SerializableDictionary<string, ObjectiveData>();
        allQuestsDictionary = new SerializableDictionary<string, ObjectiveSO>();
        if (allQuests.Count == 0)
        {
            Debug.LogWarning("Não temos nenhuma quest cadastrada na lista de todas as quests");
        }
        else
        {
            foreach (ObjectiveSO objectiveSO in allQuests)
            {
                if (objectiveSO == null)
                {
                    Debug.Log("Tem uma quest nula na lista de todas as quests, dando continue");
                    continue;
                }
                else
                {
                    allQuestsDictionary.Add(objectiveSO.Id, objectiveSO);
                }
            }
        }
    }
    void Start()
    {
        LoadObjectiveData();
        DataPersistenceManager.instance?.SaveGame();
    }

    public void LoadData(GameData gameData)
    {
        objectivesData = gameData.objectivesData;
    }
    void LoadObjectiveData()
    {
        if (objectivesData == null) return;
        if (objectivesData.Count > 0)
            foreach (string s in objectivesData.Keys.ToArray())
            {
                Debug.Log(s);
                if (s == null) { Debug.Log("Por algum motivo tentamos dar laod numa key do objectives data que é nula, dando um continue"); continue; }
                if (objectivesData.TryGetValue(s, out auxObjectiveData) != false)
                {
                    if (!auxObjectiveData.questWasRefused)
                    {
                        ObjectiveUiManager.instance?.CreateButton(allQuestsDictionary[s], auxObjectiveData);
                        if (auxObjectiveData.hasStarted && !auxObjectiveData.hasFinished)
                        {
                            LoadQuest(allQuestsDictionary[s], auxObjectiveData.stringData);
                            GameEventsManager.instance.objectiveEvents.StartObjective(s);
                        }
                        if (auxObjectiveData.hasFinished && allQuestsDictionary[s].RequiresRecompletion)
                        {
                            GameEventsManager.instance.objectiveEvents.CompleteObjective(s);
                        }
                    }
                    else
                    {
                        GameEventsManager.instance?.objectiveEvents.RefuseQuest(s);
                    }
                }
                else
                {
                    Debug.Log($"Por algum motivo deu errado ler um objective data da string {s}, dando um continue");
                    continue;
                }
            }
    }

    public void SaveData(GameData gameData)
    {
        gameData.objectivesData = objectivesData;
    }
    public void StartQuest(ObjectiveSO objectiveSO)
    {
        GameObject newObjectiveInstantiableGO = Instantiate(objectiveSO.ObjectivePrefab);
        ObjectiveInstantiable newObjectiveInstantiable = newObjectiveInstantiableGO.GetComponent<ObjectiveInstantiable>();
        ObjectiveUiManager.instance?.CreateButton(objectiveSO, new ObjectiveData(true));
        newObjectiveInstantiable.Settup(objectiveSO);
        newObjectiveInstantiable.StartObjective(true);
    }
    void LoadQuest(ObjectiveSO objectiveSO, string stringData)
    {
        GameObject newObjectiveInstantiableGO = Instantiate(objectiveSO.ObjectivePrefab);
        ObjectiveInstantiable newObjectiveInstantiable = newObjectiveInstantiableGO.GetComponent<ObjectiveInstantiable>();
        newObjectiveInstantiable.Settup(objectiveSO);
        newObjectiveInstantiable.StartObjective(false);
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
        DataPersistenceManager.instance?.SaveGame();
    }
    public void RefuseQuest(string id)
    {
        if (objectivesData.ContainsKey(id))
        {
            Debug.LogWarning($"Deu merda! Você acabou de recusar uma quest ativa!! de Id: {id}");
        }
        else
        {
            objectivesData.Add(id, new ObjectiveData(false,true));
        }
        GameEventsManager.instance?.objectiveEvents.RefuseQuest(id);
        DataPersistenceManager.instance?.SaveGame();
    }
}

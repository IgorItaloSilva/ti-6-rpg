using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarInteractabe : Interactable
{
    [SerializeField] int spawnId;
    [SerializeField] GameObject effect;
    [SerializeField] ObjectiveSO objectiveToBeProgressedSO;
    [SerializeField] private AudioSource source;
    void OnEnable()
    {
        GameEventsManager.instance.levelEvents.onKitsuneDeath += KitsuneDied;
        GameEventsManager.instance.objectiveEvents.onReciveAlreadyMadeProgress += SendProgressAlreadyMade;
    }
    void OnDisable()
    {
        GameEventsManager.instance.levelEvents.onKitsuneDeath -= KitsuneDied;
        GameEventsManager.instance.objectiveEvents.onReciveAlreadyMadeProgress -= SendProgressAlreadyMade;
    }
    override protected void Start()
    {
        base.Start();
        if (effect != null && !AlreadyInterated) effect.SetActive(false);
    }
    void KitsuneDied(int kitsuneId)
    {
        if (kitsuneId == spawnId && !AlreadyInterated)
        {
            ActivatePillar();
        }
    }

    void ActivatePillar()
    {
        GameEventsManager.instance.levelEvents.PillarActivated();
        AlreadyInterated = true;
        if (effect != null) effect.SetActive(true);
        if (source) AudioPlayer.instance.PlaySFX("Light Beam", source);
        if (objectiveToBeProgressedSO != null) GameEventsManager.instance.objectiveEvents.ProgressMade(objectiveToBeProgressedSO.Id);
        Save();
    }

    public override void Load(InteractableData interactableData)
    {
        base.Load(interactableData);
        if (AlreadyInterated)
        {
            if (effect != null) effect.SetActive(true);
            GameEventsManager.instance.levelEvents.PillarActivated();
        }
    }
    void SendProgressAlreadyMade(string objectiveId)
    {
        if (objectiveToBeProgressedSO != null)
        {
            if (objectiveToBeProgressedSO.Id == objectiveId)
            {
                if (AlreadyInterated)
                {
                    GameEventsManager.instance.objectiveEvents.ProgressMade(objectiveToBeProgressedSO.Id);
                }
            }
        }
    }
}

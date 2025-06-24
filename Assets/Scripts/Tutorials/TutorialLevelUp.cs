using UnityEngine;

public class TutorialLevelUp : Interactable
{
    [SerializeField] TutorialSO tutorialSO;
    void OnEnable()
    {
        GameEventsManager.instance.tutorialEvents.onLevelUpTutorial += ShowTutorial;
    }
    void OnDisable()
    {
        GameEventsManager.instance.tutorialEvents.onLevelUpTutorial -= ShowTutorial;
    }

    void ShowTutorial()
    {
        if (GameManager.instance.shouldShowTutorials)
        {
            Invoke("ActualShowTutorial", 1f);
        }
    }
    public override void Load(InteractableData interactableData)
    {
        base.Load(interactableData);
        if (AlreadyInterated) gameObject.SetActive(false);
    }
    void ActualShowTutorial()
    {
        UIManager.instance.DisplayPopupTutorial(tutorialSO);
        AlreadyInterated = true;
        Save();
        gameObject.SetActive(false);
    }
}

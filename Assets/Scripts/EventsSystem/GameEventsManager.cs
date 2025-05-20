using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameEventsManager : MonoBehaviour
{
    /****************************************
                COMO USAR  ESSE GAME EVENTS MANAGER
        Declarar classes novas que definem os eventos possiveis, e inicializar elas 
        no awake, elas devem ser publicas
        
        !!!!Tutorial de como declarar classes e usar os eventos no script PlayerEvents!!!!
    *****************************************/
    public static GameEventsManager instance {get; private set;}
    public PlayerEvents playerEvents;
    public UIEvents uiEvents;
    public SkillTreeEvents skillTreeEvents;
    public RuneEvents runeEvents;
    public LevelEvents levelEvents;
    public ObjectiveEvents objectiveEvents;
    public TutorialEvents tutorialEvents;
    //EXEMPLO DE NOVA CLASSE
    //public NovaClasseEvents novaClasseEvents;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        playerEvents = new PlayerEvents();
        uiEvents = new UIEvents();
        skillTreeEvents = new SkillTreeEvents();
        runeEvents = new RuneEvents();
        levelEvents = new LevelEvents();
        objectiveEvents = new ObjectiveEvents();
        tutorialEvents = new TutorialEvents();
        //novaClasseEvents = new NovaClasseEvents();
    }

}

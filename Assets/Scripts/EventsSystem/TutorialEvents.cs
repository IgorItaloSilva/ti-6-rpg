using System;
using UnityEngine;

public class TutorialEvents
{
    public event Action onLevelUpTutorial;
    public void LevelUpTutorial()
    {
        if (onLevelUpTutorial != null)
        {
            onLevelUpTutorial();
        }
    }
}

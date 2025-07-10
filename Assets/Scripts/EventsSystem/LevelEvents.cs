using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEvents
{
    public event Action<int> onKitsuneDeath;
    public void KitsuneDeath(int Id)
    {
        if (onKitsuneDeath != null)
        {
            onKitsuneDeath(Id);
        }
    }
    public event Action onPillarActivated;
    public void PillarActivated()
    {
        if (onPillarActivated != null)
        {
            onPillarActivated();
        }
    }
    /* public event Action<int> OnEnemyDied;
    public void EnemyDied(int enemyTypeCast)
    {
        if (OnEnemyDied != null)
        {
            OnEnemyDied(enemyTypeCast);
        }
    } */
}

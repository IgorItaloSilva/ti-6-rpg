using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Checkpoint : MonoBehaviour
{
    private Canvas _prompt;
    private bool _isPlayerInRange;

    private void Awake()
    {
        _prompt = GetComponentInChildren<Canvas>();
    }
    
    private void Start()
    {
        _prompt.enabled = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _prompt.enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(PlayerStateMachine.Instance.IsInteractPressed)
        {
            DataPersistenceManager.instance.SaveGame();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _prompt.enabled = false;
        }
    }
}

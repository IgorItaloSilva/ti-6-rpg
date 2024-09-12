using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Header("First selected button")]
    [SerializeField] private Button firstSelectedButton;

    protected virtual void OnEnable(){
        SetFirstSelected(firstSelectedButton);
    }
    public void SetFirstSelected(Button firstSelectedButton){
        firstSelectedButton.Select();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditosCrontroller : MonoBehaviour
{
    [SerializeField]GameObject painelCreditos;
    void Start()
    {
        if(painelCreditos!=null){
            CloseCredits();
        }
    }
    public void OpenCredits(){
        painelCreditos.SetActive(true);
    }
    public void CloseCredits(){
        painelCreditos.SetActive(false);
    }
}

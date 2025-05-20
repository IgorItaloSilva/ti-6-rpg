using System.Collections;
using UnityEngine;

public class TimeToDisable : MonoBehaviour
{
    [SerializeField] float seconds;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);

    }
    
}
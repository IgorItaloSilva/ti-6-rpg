using System.Threading.Tasks;
using UnityEngine;

public class TimeToDisable : MonoBehaviour
{
    [SerializeField] int miliseconds;



    void OnEnable()
    {
        Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Disable()
    {
        await Task.Delay(miliseconds);
        gameObject.SetActive(false);

    }
    
}
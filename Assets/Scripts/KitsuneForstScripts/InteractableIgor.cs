using UnityEngine;


public class InteractableIgor : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject[] doors;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        canvas.SetActive(true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.E))
        {
            Debug.Log("AAAAAAA");
            Destroy(canvas);
            Destroy(gameObject);
            doors[0].gameObject?.SetActive(false);
            doors[1].gameObject.SetActive(true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        canvas.SetActive(false);
    }
}

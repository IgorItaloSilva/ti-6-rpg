using UnityEngine;


public class InteractableIgor : MonoBehaviour
{
    [SerializeField] GameObject lockedDoorObject;
    [SerializeField] GameObject[] doors;
    [SerializeField] private AudioSource source;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        lockedDoorObject.SetActive(true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.E))
        {
            AudioPlayer.instance.PlaySFX("Gate Open", source);
            lockedDoorObject.SetActive(false);
            gameObject.SetActive(false);
            doors[0].gameObject?.SetActive(false);
            doors[1].gameObject.SetActive(true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        lockedDoorObject.SetActive(false);
    }
}

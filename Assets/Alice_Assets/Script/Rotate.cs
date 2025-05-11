using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 100, 0); // degrees per second

    void FixedUpdate()
    {
        transform.Rotate(rotationSpeed * Time.fixedDeltaTime);
        //Vector3 newTransform = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(Time.time) / 10, transform.position.z );
        //transform.position = newTransform;
    }
}      
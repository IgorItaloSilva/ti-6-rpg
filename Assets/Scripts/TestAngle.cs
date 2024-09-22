using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAngle : MonoBehaviour
{
    [SerializeField] Transform target;

    // Update is called once per frame
    void Update()
    {
        Vector3 thisForward = transform.forward;
        Vector3 dir = (target.position - transform.position).normalized;
        thisForward.y = 0;
        dir.y = 0;
        Debug.Log(Vector3.Angle(transform.forward, dir));
    }
}

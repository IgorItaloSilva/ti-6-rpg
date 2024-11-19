using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSteeringAgentMigue : MonoBehaviour,ISteeringAgent
{
    CharacterController cc;
    void Start(){
        cc= GetComponent<CharacterController>();
    }
    public float GetCharHeight()
    {
        throw new System.NotImplementedException();
    }

    public float GetMaxForce()
    {
        throw new System.NotImplementedException();
    }

    public float GetMaxVelocity()
    {
        return 8;
    }

    public LayerMask GetObstaclesLayerMask()
    {
        throw new System.NotImplementedException();
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public float GetSphereRadius()
    {
        throw new System.NotImplementedException();
    }

    public Vector3 GetVelocity()
    {
        return cc.velocity;
    }

}

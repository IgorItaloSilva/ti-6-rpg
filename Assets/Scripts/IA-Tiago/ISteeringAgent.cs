using UnityEngine;

public interface ISteeringAgent {

    public Vector3 GetVelocity();
    public float GetMaxVelocity();
    public Vector3 GetPosition();
    public float GetMaxForce();
    public float GetSphereRadius();
    public float GetCharHeight();
    public LayerMask GetObstaclesLayerMask();
}

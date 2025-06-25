using UnityEngine;

public class TriggerVFX : Interactable
{
    [Header("O GAME OBJECT DO SEU VFX VEM AQUI")]
    [SerializeField] GameObject[] vfxs;
    [Header("N TOCA AQUI FDP")]
    [SerializeField] LayerMask layerMask;
    Collider[] colliders;

    protected override void Start()
    {
        float rad = sphereCollider.radius;
        SetAllActive(false);
        if (Physics.OverlapSphereNonAlloc(transform.position, rad, colliders, layerMask) != 0)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    SetAllActive(true);
                }
            }
        }

    }
    override protected void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            SetAllActive(true);
        }
    }
    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            SetAllActive(false);
        }
    }
    void SetAllActive(bool active)
    {
        if (vfxs.Length == 0) return;
        foreach (GameObject go in vfxs)
        {
            go.SetActive(active);
        }
    }
}

using System;
using UnityEngine;

public class DisableSakuraVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _sakuraVFX = new ParticleSystem[2];
    private void Start()
    {
        if (!_sakuraVFX[0])
            _sakuraVFX[0] = Camera.main?.transform.GetChild(0).GetChild(2).GetComponent<ParticleSystem>();
        if (!_sakuraVFX[1])
            _sakuraVFX[0] = Camera.main?.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            foreach (var vfx in _sakuraVFX)
                vfx.Stop();
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
            foreach (var vfx in _sakuraVFX)
                vfx.Play();
    }
}

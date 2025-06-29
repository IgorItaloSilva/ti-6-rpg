using System;
using UnityEngine;

public class KitsuneMagicBullet : MonoBehaviour
{
    Transform target; // alvo dos projeteis
    private WeaponManager _weaponManager;
    Rigidbody rb;

    float speed = 25; // velocidade dos projeteis

    bool isClose = false; // Checar se chegou proximo o suficiente e para de olhar pro player
    float moveTimer; // Tempo ate os projeteis seguirem o player

    private void Awake()
    {
        _weaponManager = GetComponent<WeaponManager>();
    }

    void Start()
    {
        target = PlayerStateMachine.Instance.transform;
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        isClose = false;
        moveTimer = 0.6f;
        Invoke(nameof(EnableWeaponCollider), 0.1f);
    }

    void EnableWeaponCollider()
    {
        _weaponManager.EnableCollider();
    }

    void Update() { 
        if(!isClose) {
            Vector3 pos = target.position;
            pos.y += 0.5f;
            transform.LookAt(pos);
            if(Vector3.Distance(target.position, transform.position) < 2f)
                isClose = true;
        }
        if(moveTimer <= 0)
            transform.position += transform.forward * speed * Time.deltaTime;
        else
            moveTimer -= Time.deltaTime;
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Ground"))
        {
            gameObject.SetActive(false);
        }
        
    }

}
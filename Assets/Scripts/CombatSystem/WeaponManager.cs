using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class WeaponManager : MonoBehaviour
{
    protected float damage;
    [SerializeField] protected bool showDebug = false;
    [SerializeField] protected Enums.DamageType damageType;
    [SerializeField] protected float baseDamage = 10f;
    protected Collider damageCollider;
    protected List<IDamagable> damagedTargets = new List<IDamagable>();
    protected EnemyBehaviour enemyBehaviour;



    protected virtual void Start()
    {
        damage = baseDamage;
    }

    void OnEnable()
    {
        damageCollider = GetComponent<Collider>();
        enemyBehaviour = GetComponentInParent<EnemyBehaviour>();
        if (enemyBehaviour == null) Debug.LogWarning("A arma não achou o enemyControllerDela");
        if (damageCollider == null)
        {
            Debug.LogWarning($"O weapon manager do {name} não achou o collider dela");
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        //Debug.Log("A arma colidiu com algo");
        //if(!other.gameObject.CompareTag("EnemyDetection")&&!other.CompareTag("Enemy")) 
        //não tem pra que tirar todas as excessoes quando a gente pode só colocar o caso do jogador aqui
        if (other.CompareTag("Player"))
        {
            IDamagable alvoAtacado = other.gameObject.GetComponentInParent<IDamagable>();
            //Debug.Log($"A interface Idamageble que eu peguei foi {alvoAtacado}");
            if (alvoAtacado != null)
            {
                if (PlayerStateMachine.Instance.IsBlocking)
                {
                    if (PlayerStateMachine.Instance.ShouldParry)
                    {
                        enemyBehaviour.WasParried();
                        AudioPlayer.instance.PlaySFX("Parry");
                        PlayerStateMachine.Instance.Animator.ResetTrigger(PlayerStateMachine.Instance.HasParried);
                        PlayerStateMachine.Instance.Animator.SetTrigger(PlayerStateMachine.Instance.HasParried);
                    }
                    else
                    {
                        AudioPlayer.instance.PlaySFX("Parry");
                        DealDamage(alvoAtacado, damage / 3);
                    }
                }
                else
                {
                    PlayerStateMachine.Instance.ResetAttacks();
                    DealDamage(alvoAtacado, damage);
                }
                DisableCollider();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            damagedTargets.Clear();
    }

    protected virtual void DealDamage(IDamagable alvo, float dano)
    {
        if (damagedTargets.Contains(alvo))
        {
            return;
        }
        damagedTargets.Add(alvo);
        if (!PlayerStateMachine.Instance.IsDodging)
        {
            alvo.TakeDamage(dano, damageType, false);
            //Criar um texto de dano na tela
            if (showDebug) Debug.Log($"Enviei {dano} de dano para ser tomado para {alvo}");
        }
        else
        {
            if (showDebug) Debug.Log($"Não enviei {dano} de dano para ser tomado para {alvo}. Ele está desviando!");
        }
    }
    public void EnableCollider()
    {
        damageCollider.enabled = true;
    }

    public void DisableCollider()
    {
        damageCollider.enabled = false;
        damagedTargets.Clear();
    }

    void OnDisable()
    {
        damagedTargets.Clear();
    }
    
}
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpecialAttackUIManager : MonoBehaviour
{
    public static SpecialAttackUIManager instance;
    [SerializeField] SpecialAttack[] specialAttacksReference;
    [SerializeField] bool showDebug;
    int NSpecialAttacks;
    SerializableDictionary<int, SpecialAttack> specialAttacks;
    void OnEnable()
    {
        GameEventsManager.instance.skillTreeEvents.onActivatePowerUp += UnlockPowerUp;
    }
    void OnDisable()
    {
        GameEventsManager.instance.skillTreeEvents.onActivatePowerUp -= UnlockPowerUp;
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        if (specialAttacksReference.Length == 0 && showDebug)
        {
            Debug.LogWarning("Não temos nenhum special Attack cadastrado");
        }
        else
        {
            specialAttacks = new SerializableDictionary<int, SpecialAttack>();
            foreach (SpecialAttack sa in specialAttacksReference)
            {
                if (specialAttacks.ContainsKey(sa.Id))
                {
                    if (showDebug) Debug.LogError("Tentamos adicionar um SpecialAttack com Id repetido");
                }
                else
                {
                    specialAttacks.Add(sa.Id, sa);
                }
            }
        }
    }
    private void UnlockPowerUp(int id)
    {
        SpecialAttack sp;
        int specialId=0;
        switch (id) //sim essa merda é hardcoded  ¯\_(ツ)_/¯
        {
            case 5: //auto dano
                specialId = 1;
                break;
            case 6: //sangramento
                specialId = 2;
                break;
            case 2: //salto
                specialId = 3;
                break;
            case 1: //poise
                specialId = 4;
                break;
            default:break; //Power up de outra coisa
        }
        if (specialAttacks.TryGetValue(specialId,out sp))
        {
            sp.LockedImageGO.SetActive(false);
        }
    }
    void Update()
    {
        if (Keyboard.current.numpad7Key.wasPressedThisFrame && GameManager.instance.cheatsEnabled)
        {
            for (int i = 1; i < 5; i++)
            {
                UnlockPowerUp(i);
            }
        }
        if (Keyboard.current.numpad1Key.wasPressedThisFrame)
        {
            StartCooldown(1);
        }
        if (Keyboard.current.numpad2Key.wasPressedThisFrame)
        {
            StartCooldown(2);
        }
        if (Keyboard.current.numpad3Key.wasPressedThisFrame)
        {
            StartCooldown(3);
        }
        if (Keyboard.current.numpad4Key.wasPressedThisFrame)
        {
            StartCooldown(4);
        }
    }
    public void StartCooldown(int specialId)//chamado pelo statemachine
    {
        StartCoroutine(HandleCooldown(specialId));
    }
    IEnumerator HandleCooldown(int specialId)
    {
        SpecialAttack sp;
        if (specialAttacks.TryGetValue(specialId, out sp))
        {
            float timeRemaning = sp.CooldownTimeSec;
            sp.CooldownImage.gameObject.SetActive(true);
            sp.cooldownText.gameObject.SetActive(true);
            sp.cooldownText.text = ((int)timeRemaning).ToString();
            while (timeRemaning > 0)
            {
                timeRemaning -= Time.deltaTime;
                sp.CooldownImage.fillAmount = timeRemaning / sp.CooldownTimeSec;
                sp.cooldownText.text = ((int)timeRemaning+1).ToString();
                yield return null;
            }
            sp.CooldownImage.gameObject.SetActive(false);
            sp.cooldownText.gameObject.SetActive(false);
            PlayerStateMachine.Instance?.EndSpecialCooldown(specialId);
        }
    }
}

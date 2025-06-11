using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SpecialAttack : MonoBehaviour
{
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public int CooldownTimeSec { get; private set; }
    [field: SerializeField] public TextMeshProUGUI cooldownText { get; private set; }
    [field: SerializeField] public Image BackgorundImage { get; private set; }
    [field: SerializeField] public Image CooldownImage { get; private set; }
    [field: SerializeField] public GameObject LockedImageGO { get; private set; }
}

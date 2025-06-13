using UnityEngine;
using UnityEngine.EventSystems;

public class MoedaTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] int hardcodeID;
    public void OnPointerEnter(PointerEventData eventData)
    {
        SkillTreeUIManager.instance.ActivatePowerUpDescriptionBox(hardcodeID);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SkillTreeUIManager.instance.DeactivatePowerUpDescriptionBox();
    }
}
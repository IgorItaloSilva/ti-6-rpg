using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class RuneButton : MonoBehaviour ,IPointerDownHandler
{
    public int id;
    [SerializeField]TextMeshProUGUI nameText;
    [SerializeField]TextMeshProUGUI descriptionText;
    [SerializeField]TextMeshProUGUI typeText;
    [SerializeField]TextMeshProUGUI qualityText;
    public RuneSO rune;
    public void SetRuneAndTexts(RuneSO rune){
        this.rune=rune;
        nameText.text=rune.Nome;
        descriptionText.text=rune.DescriptionText;
        typeText.text=GetTypeText(rune.Part);
        SetQualityText(rune.Quality);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        RunesUiManager.instance.EquipRune(id);
    }
    string GetTypeText(Enums.KatanaPart runeType){
        switch(runeType){
            case Enums.KatanaPart.Blade: return "Blade";
            case Enums.KatanaPart.Guard: return "Guard";
            case Enums.KatanaPart.Handle: return "Handle";
        }
        Debug.LogWarning("Uma runa sem tipo foi setada");
        return "Error Type Not Found";
    }
    void SetQualityText(Enums.ItemQuality quality){
        switch(quality){
            case Enums.ItemQuality.Common: 
                qualityText.text = "Common";
                qualityText.color = Color.gray;
            break;
            case Enums.ItemQuality.Rare: 
                qualityText.text = "Rare";
                qualityText.color = Color.blue;
            break;
            case Enums.ItemQuality.Epic: 
                qualityText.text = "Epic";
                qualityText.color = new Color(160f/255,32f/255,240f/255);
            break;
            case Enums.ItemQuality.Legendary: 
                qualityText.text = "Legendary";
                qualityText.color = new Color(255f/255, 165f/255, 0f);
            break;
        }
    }
}

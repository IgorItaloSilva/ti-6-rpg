using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class RuneButton : MonoBehaviour ,IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField]TextMeshProUGUI nameText;
    [SerializeField]TextMeshProUGUI descriptionText;
    [SerializeField]TextMeshProUGUI typeText;
    [SerializeField]TextMeshProUGUI qualityText;
    public RuneSO rune;
    public void SetRuneAndTexts(RuneSO rune){
        this.rune=rune;
        nameText.text=rune.name;
        descriptionText.text=rune.DescriptionText;
        typeText.text=GetTypeText(rune.Type);
        SetQualityText(rune.Quality);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
    string GetTypeText(Enums.RuneType runeType){
        switch(runeType){
            case Enums.RuneType.Blade: return "Blade";
            case Enums.RuneType.Guard: return "Guard";
            case Enums.RuneType.Handle: return "Handle";
        }
        Debug.LogWarning("Uma runa sem tipo foi setada");
        return "Error Type Not Found";
    }
    void SetQualityText(int quality){
        switch(quality){
            case 0: 
                qualityText.text = "Common";
                qualityText.color = Color.gray;
            break;
            case 2: 
                qualityText.text = "Rare";
                qualityText.color = Color.blue;
            break;
            case 3: 
                qualityText.text = "Epic";
                qualityText.color = new Color(160f/255,32f/255,240f/255);
            break;
            case 4: 
                qualityText.text = "Legendary";
                qualityText.color = new Color(255f/255, 165f/255, 0f);
            break;
        }
    }
}

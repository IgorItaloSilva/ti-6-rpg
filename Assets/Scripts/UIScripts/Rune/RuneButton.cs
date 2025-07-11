using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class RuneButton : MonoBehaviour
{
    public int id;
    [SerializeField]TextMeshProUGUI nameText;
    [SerializeField]Image icon;
    [SerializeField]TextMeshProUGUI descriptionText;
    [SerializeField]TextMeshProUGUI typeText;
    [SerializeField]TextMeshProUGUI qualityText;
    [field:SerializeField]public GameObject textos { get; private set; }
    public RuneSO rune;
    public bool isEquiped;
    public void SetRuneAndTexts(RuneSO rune)
    {
        if (RuneManager.instance.showRuneDebug)
        {
            if (rune == null) Debug.LogError("Recebemos uma runa vazia wtf");
            else Debug.Log($"Recebemos a runa {rune}");
        }
        this.rune = rune;
        nameText.text = rune.Nome;
        icon.sprite = rune.Sprite;
        descriptionText.text = rune.DescriptionText;
        typeText.text = GetTypeText(rune.Part);
        SetQualityText(rune.Quality);
    }
     public void LoadRuneAndTexts(GameObject textos)//chamado pelo event do botão
    {
        textos.SetActive(true);
        nameText.text = rune.Nome;
        icon.sprite = rune.Sprite;
        descriptionText.text = rune.DescriptionText;
        typeText.text = GetTypeText(rune.Part);
        SetQualityText(rune.Quality);
    } 
    public void Equip()
    {
        RunesUiManager.instance.EquipRune(id);
    }
    public void OnPointerDown()
    {
        if(isEquiped){
            Debug.Log("Cliquei em desequipar uma runa");
            RunesUiManager.instance.Unequip(id);
        }
        else{
            Debug.Log("Cliquei em equipar uma runa");
            RunesUiManager.instance.EquipRune(id);
        }
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

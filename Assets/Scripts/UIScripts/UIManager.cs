using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField]private Slider lifeSlider;
    [SerializeField]private Image saveIcon;
    [SerializeField]private GameObject painelPause;
    [SerializeField]private GameObject painelSkillTree;
    [SerializeField]SkillNodeUI[] powerUpNodes; //O NODE PRECISA SER FEITO EM ORDER PELO ID
    [Header("Caixinha Descricao e Nome")]
    [SerializeField]GameObject powerUpUIBox;
    [SerializeField]TextMeshProUGUI powerUpDescriptionText;
    [SerializeField]TextMeshProUGUI powerUpNameText;
    [SerializeField]float offset = 5;
    bool powerUpBoxIsOpen = false;
    bool painelPauseIsOpen;
    bool painelSkillTreeIsOpen;
    void OnEnable(){
        GameEventsManager.instance.uiEvents.onUpdateSliders+=UpdateSliders;
        GameEventsManager.instance.uiEvents.onLifeChange+=UpdateHealth;
        GameEventsManager.instance.uiEvents.onSavedGame+=FeedBackSave;
    }
    void OnDisable(){
        GameEventsManager.instance.uiEvents.onUpdateSliders-=UpdateSliders;
        GameEventsManager.instance.uiEvents.onLifeChange-=UpdateHealth;
        GameEventsManager.instance.uiEvents.onSavedGame-=FeedBackSave;
    }
    void Start()
    {
        if(instance==null){
            instance=this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
        if(painelPause==null){
            Debug.LogWarning("O nosso uiManager não tem referencia ao menu de pause");
        }
        else{
            painelPause.SetActive(false);
            painelPauseIsOpen=false;
        }
        if(!powerUpUIBox){
            Debug.LogWarning("O nosso uiManager não tem referencia da caixa do powerUp");
        }
        else{
            DeactivatePowerUpDescriptionBox();
        }
        if(!painelSkillTree){
            Debug.LogWarning("O nosso uiManager não tem referencia ao painel de skill");
        }
        else{
            painelSkillTree.SetActive(false);
            painelSkillTreeIsOpen=false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame){
            if(painelPauseIsOpen){
                UnpauseGame();
            }
            else{
                PauseGame();
            }
        }
        if(Mouse.current.leftButton.wasPressedThisFrame){
            Debug.Log($"pos do mouse = {Mouse.current.position.ReadValue()}");
        }
    }
    private void UpdateHealth(float vidaAtual){
        if(lifeSlider!=null){
            lifeSlider.value=vidaAtual;
        }
    }
    private void UpdateSliders(int id,float minValue,float maxValue){
        switch(id){
            case 0:
                if(lifeSlider!=null){
                    lifeSlider.minValue=minValue;
                    lifeSlider.maxValue=maxValue;
                }
            break;
            default: return;
        }
    }
    private void FeedBackSave(){
        if(saveIcon!=null){
            saveIcon.gameObject.SetActive(true);
            StartCoroutine(SpinSaveIcon());
        }
    }
    public void QuitGame(){
        Application.Quit();
    }
    public void VoltarMainMenu(){
        Time.timeScale=1f;
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
    IEnumerator SpinSaveIcon(){
        float timerTotal=5f;
        while(timerTotal>0){
            timerTotal-=Time.unscaledDeltaTime;
            saveIcon.rectTransform.Rotate(Vector3.forward,-5);
            yield return null;
        }
        saveIcon.gameObject.SetActive(false);
    }
    private void UnpauseGame(){
        Time.timeScale=1f;
        Cursor.lockState=CursorLockMode.Locked;
        Cursor.visible=false;
        painelPause.SetActive(false);
        painelPauseIsOpen=false;
    }
    private void PauseGame(){
        Time.timeScale=0f;
        Cursor.lockState=CursorLockMode.Confined;
        Cursor.visible=true;
        painelPause.SetActive(true);
        painelPauseIsOpen=true;
    }
    public void ActivatePowerUpDescriptionBox(int id){
        if(!powerUpBoxIsOpen){
            SkillNodeUI node = powerUpNodes[id];
            powerUpUIBox.SetActive(true);
            Vector2 pos = Mouse.current.position.ReadValue() + new Vector2(offset,offset);
            powerUpUIBox.GetComponent<RectTransform>().SetPositionAndRotation(pos,Quaternion.identity);
            powerUpNameText.text = node.powerUp.Name;
            powerUpDescriptionText.text = node.powerUp.UiDescription;
            powerUpBoxIsOpen=true;
        }
    }
    public void DeactivatePowerUpDescriptionBox(){
        powerUpUIBox.SetActive(false);
        powerUpBoxIsOpen=false;
    }
    public void AlternarPainelSkillTree(){
        if(painelSkillTreeIsOpen){
            painelSkillTree.SetActive(false);
            painelPauseIsOpen=false;
        }
        else{
            painelSkillTree.SetActive(true);
            painelPauseIsOpen=true;
        }

    }
}

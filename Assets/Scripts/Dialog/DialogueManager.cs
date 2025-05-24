using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public Image profilePic, arrow;
    public TMP_Text nameSpace, text;
    public Queue<Speech> dialogue;
    [SerializeField]GameObject[] optionButtons;
    TextMeshProUGUI[] optionButtonsText;
    private string current;
    private Speech currentSpeech;
    private Coroutine op;
    public bool isChatting {get; private set;}= false;
    void Awake(){
        if(instance==null){
            instance=this;
        }
        else{
            Destroy(this);
        }
        optionButtonsText =  new TextMeshProUGUI[optionButtons.Length];
        for(int i=0;i<optionButtons.Length;i++){
            optionButtonsText[i]=optionButtons[i].GetComponentInChildren<TextMeshProUGUI>();
        }
    }
    /* private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isChatting)
        {
            Skip();
        }
    } */
    public void Paste(Speech content)
    {
        DeactivateArrow();
        DeactivateAnswerButtons();
        text.text = null;
        nameSpace.text = content.speakerName;
        profilePic.sprite = content.speakerIcon;
        op = StartCoroutine(StepPasting(content.text));
    }

    public void PasteAll(string content)
    {
        StopCoroutine(op);
        op = null;
        text.text = content;
        if(currentSpeech.needsAnswer){
            ActivateAnswerButtons();
        }
        else{
            ActivateArrow();
        }
    }

    public void AdvanceDialog()
    {
        if (op != null)
        {
            //Debug.Log("Corrotina ainda não terminou");
            PasteAll(current);
        }
        else
        {
            if(!currentSpeech.needsAnswer){
                if (dialogue.Count > 0){
                    currentSpeech = dialogue.Dequeue();
                    Paste(currentSpeech);
                }
                else EndDialogue();
            }
            //se precisar de resposta vamos travar aqui
        }
    }
    void AdvanceDialogAfterAnswer(){
        if(dialogue.Count > 0){
            currentSpeech = dialogue.Dequeue();
            Paste(currentSpeech);
        }
        else EndDialogue();
    }
    public void Option1(){
        currentSpeech.dialogAnswer?.Option1();
        DeactivateAnswerButtons();
        AdvanceDialogAfterAnswer();
    }
    public void Option2(){
        currentSpeech.dialogAnswer?.Option2();
        DeactivateAnswerButtons();
        AdvanceDialogAfterAnswer();
    }
    public void Option3(){
        currentSpeech.dialogAnswer?.Option3();
        DeactivateAnswerButtons();
        AdvanceDialogAfterAnswer();
    }

    // Este metodo deve ser usado unica e exclusivamente para a primeira execu��o de exibir o dialogo, ja que n�o � possivel invocar o metodo acima sem um Callback Context adequado
    public void Commence()
    {
        StopAllCoroutines();
        if (dialogue.Count > 0){
            currentSpeech = dialogue.Dequeue();
            Paste(currentSpeech);
        }
        else EndDialogue();
    }

    public void EndDialogue()
    {
        isChatting = false;
        UIManager.instance.SwitchToScreen((int)UIManager.UIScreens.Closed);
        /* dialogueScreen.SetActive(false); moviods para o UIManagaer
        GameManager.instance.UnpauseGameAndLockCursor(); */
    }

    public void StartDialogue(Dialogue source)
    {
        //GameManager.instance.PauseGameAndUnlockCursor();movidos para o UIManagaer
        UIManager.instance.SwitchToScreen((int)UIManager.UIScreens.Dialog);
        dialogue = new Queue<Speech>(source.dialogue.Count);
        foreach (var data in source.dialogue)
        {
            dialogue.Enqueue(data);
        }
        //dialogueScreen.SetActive(true);moviods para o UIManagaer
        isChatting = true;
        Commence();
    }

    private void DeactivateArrow()
    {
        arrow.gameObject.SetActive(false);
    }

    private void ActivateArrow()
    {
        if(currentSpeech.needsAnswer)return;
        arrow.gameObject.SetActive(true);
    }

    IEnumerator StepPasting(string content)
    {
        current = content;
        foreach (char value in content)
        {
            text.text += value;
            yield return new WaitForSecondsRealtime(0.02f);
        }
        if(currentSpeech.needsAnswer){
            ActivateAnswerButtons();
        }
        else{
            ActivateArrow();
        }
        op = null;
    }
    void ActivateAnswerButtons(){
        if(currentSpeech.needsAnswer){
            for(int i=0;i<currentSpeech.amountAnswers;i++){
                optionButtons[i].SetActive(true);
                optionButtonsText[i].text=currentSpeech.optionsTexts[i];
            }
        }
    }
    void DeactivateAnswerButtons(){
        for(int i=0;i<optionButtons.Length;i++){
            optionButtons[i].SetActive(false);
        }
    }
}

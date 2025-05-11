using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public GameObject dialogueScreen;
    public Image profilePic, arrow;
    public TMP_Text nameSpace, text;
    private int maxCharCount;
    public Queue<Speech> dialogue;
    private string current;
    private Coroutine op;
    public bool isChatting {get; private set;}= false;
    void Awake(){
        if(instance==null){
            instance=this;
        }
        else{
            Destroy(this);
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
        ActivateArrow();
    }

    public void Skip()
    {
        if (op != null)
        {
            Debug.Log("op diferente de null");
            PasteAll(current);
        }
        else
        {
            if (dialogue.Count > 0) Paste(dialogue.Dequeue());
            else EndDialogue();
        }
    }

    // Este metodo deve ser usado unica e exclusivamente para a primeira execu��o de exibir o dialogo, ja que n�o � possivel invocar o metodo acima sem um Callback Context adequado
    public void Commence()
    {
        StopAllCoroutines();
        if (dialogue.Count > 0) Paste(dialogue.Dequeue());
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
        ActivateArrow();
        op = null;
    }
}

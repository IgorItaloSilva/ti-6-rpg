using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    public GameObject dialogueScreen;
    public Image profilePic, arrow;
    public TMP_Text nameSpace, text;
    private int maxCharCount;
    public Queue<Speech> dialogue;
    private string current;
    private Coroutine op;
    private bool chatting = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && chatting)
        {
            Skip();
        }
    }
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

    // Este metodo deve ser usado unica e exclusivamente para a primeira execução de exibir o dialogo, ja que não é possivel invocar o metodo acima sem um Callback Context adequado
    public void Commence()
    {
        StopAllCoroutines();
        if (dialogue.Count > 0) Paste(dialogue.Dequeue());
        else EndDialogue();
    }

    public void EndDialogue()
    {
        chatting = false;
        dialogueScreen.SetActive(false);
        GameManager.instance.UnpauseGameAndLockCursor();
    }

    public void StartDialogue(Dialogue source)
    {
        GameManager.instance.PauseGameAndUnlockCursor();
        dialogue = new Queue<Speech>(source.dialogue.Count);
        foreach (var data in source.dialogue)
        {
            dialogue.Enqueue(data);
        }
        dialogueScreen.SetActive(true);
        chatting = true;
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

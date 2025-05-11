using UnityEngine;

public class Speek : MonoBehaviour
{
    public Dialogue[] dialogues;
    protected int counter = 0;
    public DialogueManager dialogueManager;

    public void StartDialogue()
    {
        dialogueManager = DialogueManager.instance;
        Dialogue();
    }
    public void Dialogue()
    {
        dialogueManager.StartDialogue(dialogues[counter]);
        if (counter < dialogues.Length - 1) counter++;
    }
}

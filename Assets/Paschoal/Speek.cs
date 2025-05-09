using UnityEngine;

public class Speek : MonoBehaviour
{
    public Dialogue[] dialogues;
    protected int counter = 0;
    public DialogueBox dialogueBox;

    public void StartDialogue()
    {
        dialogueBox = FindAnyObjectByType<DialogueBox>();
        Dialogue();
    }
    public void Dialogue()
    {
        dialogueBox.StartDialogue(dialogues[counter]);
        if (counter < dialogues.Length - 1) counter++;
    }
}

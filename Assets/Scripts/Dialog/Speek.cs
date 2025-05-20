using UnityEngine;

public class Speek : MonoBehaviour
{
    public Dialogue[] dialogues;
    protected int counter = 0;
    public void Dialogue()
    {
        DialogueManager.instance.StartDialogue(dialogues[counter]);
        if (counter < dialogues.Length - 1) counter++;
    }
}

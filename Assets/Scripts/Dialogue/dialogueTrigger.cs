using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueSequence dialogue;
    public DialogueManager dialogueManager;
    public DialogueTriggerType triggerType;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered)
            return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            dialogueManager.StartDialogue(dialogue);
        }
    }
}
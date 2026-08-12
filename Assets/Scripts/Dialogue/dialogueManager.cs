using UnityEngine;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    public UIDocument uiDocument;

    private VisualElement dialogueBox;
    private Label speakerLabel;
    private Label dialogueLabel;


    private DialogueSequence currentDialogue;

    private int currentLineIndex;
    void Awake()
    {
        var root = uiDocument.rootVisualElement;

        dialogueBox = root.Q<VisualElement>("DialogueBox");

        speakerLabel = root.Q<Label>("SpeakerLabel");

        dialogueLabel = root.Q<Label>("DialogueLabel");

        HideDialogue();
    }

    public void StartDialogue(DialogueSequence dialogue)
    {
        currentDialogue = dialogue;

        currentLineIndex = 0;

        ShowDialogue();

        DisplayCurrentLine();
    }

    void DisplayCurrentLine()
    {
        if (currentLineIndex >= currentDialogue.lines.Length)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = currentDialogue.lines[currentLineIndex];

        speakerLabel.text = line.speaker;

        dialogueLabel.text = line.text;
    }

    public void NextLine()
    {
        currentLineIndex++;

        DisplayCurrentLine();
    }

    public void TriggerDialogue(DialogueTriggerType trigger)
    {
        foreach (DialogueLine line in currentDialogue.lines)
        {
            if (line.triggerType == trigger)
            {
                speakerLabel.text = line.speaker;
                dialogueLabel.text = line.text;

                ShowDialogue();

                return;
            }
        }
    }

    void ShowDialogue()
    {
        dialogueBox.style.display =
            DisplayStyle.Flex;
    }

    void HideDialogue()
    {
        dialogueBox.style.display =
            DisplayStyle.None;
    }

    void EndDialogue()
    {
        HideDialogue();

        currentDialogue = null;
    }
}
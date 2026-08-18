using UnityEngine;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    public UIDocument uiDocument;

    public DialogueSequence openingDialogue;
    public float openingDialogueDelay = 5f;

    private VisualElement dialogueBox;
    private Label speakerLabel;
    private Label dialogueLabel;

    private DialogueSequence currentDialogue;

    private int currentLineIndex;

    private DialogueTrigger currentTrigger;


    void Start()
    {
        if (openingDialogue != null)
        {
            Invoke(nameof(StartOpeningDialogue), openingDialogueDelay);
        }
    }


    void Awake()
    {
        var root = uiDocument.rootVisualElement;

        dialogueBox = root.Q<VisualElement>("dialogueBox");
        speakerLabel = root.Q<Label>("speakerLabel");
        dialogueLabel = root.Q<Label>("dialogueLabel");

        HideDialogue();
    }


    public void StartDialogue(DialogueSequence dialogue)
    {
        currentDialogue = dialogue;

        currentLineIndex = 0;

        currentTrigger = null;

        ShowDialogue();

        DisplayCurrentLine();
    }
    public void StartDialogue(DialogueSequence dialogue, DialogueTrigger trigger)
    {
        currentDialogue = dialogue;

        currentLineIndex = 0;

        
        currentTrigger = trigger;

        ShowDialogue();

        DisplayCurrentLine();
    }


    void DisplayCurrentLine()
    {
        if (currentDialogue == null)
            return;

       
        if (currentLineIndex >= currentDialogue.lines.Length)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = currentDialogue.lines[currentLineIndex];

        speakerLabel.text = line.speaker.ToString();
        dialogueLabel.text = line.text;

        UpdateSpeakerStyle(line.speaker);

        CancelInvoke(nameof(NextLine));

        Invoke(nameof(NextLine), line.displayTime);
    }

    public void NextLine()
    {
        if (currentDialogue == null)
            return;

        currentLineIndex++;

        DisplayCurrentLine();
    }

    void EndDialogue()
    {
        CancelInvoke(nameof(NextLine));

        HideDialogue();

        DialogueTrigger finishedTrigger = currentTrigger;

        currentDialogue = null;
        currentLineIndex = 0;
        currentTrigger = null;
        if (finishedTrigger != null)
        {
            finishedTrigger.DialogueFinished();
        }
    }

    void ShowDialogue()
    {
        dialogueBox.style.display = DisplayStyle.Flex;
    }


    void HideDialogue()
    {
        dialogueBox.style.display = DisplayStyle.None;
    }


    void UpdateSpeakerStyle(DialogueSpeaker speaker)
    {
        speakerLabel.RemoveFromClassList("wheatley");
        speakerLabel.RemoveFromClassList("glados");

        switch (speaker)
        {
            case DialogueSpeaker.Wheatley:
                speakerLabel.AddToClassList("wheatley");
                break;

            case DialogueSpeaker.GLaDOS:
                speakerLabel.AddToClassList("glados");
                break;
        }
    }


    void StartOpeningDialogue()
    {
        StartDialogue(openingDialogue);
    }

    void OnDestroy()
    {
        CancelInvoke();
    }
}
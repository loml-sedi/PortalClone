using System.Collections.Generic;
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

    [SerializeField] private string openingDialogueID = "opening";

    public bool dialogueLocked = false;

    [System.Serializable]
    public class QueuedDialogue
    {
        public DialogueSequence dialogue;
        public DialogueTrigger trigger;

        public QueuedDialogue(DialogueSequence dialogue, DialogueTrigger trigger)
        {
            this.dialogue = dialogue;
            this.trigger = trigger;
        }
    }

    private Queue<QueuedDialogue> dialogueQueue = new Queue<QueuedDialogue>();
    private bool dialoguePlaying = false;

    void Start()
    {
        if (openingDialogue != null)
        {
            if (DialogueState.Instance != null &&
                DialogueState.Instance.HasTriggered(openingDialogueID))
            {
                return;
            }

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
        if (dialogueLocked)
            return;

        if (dialoguePlaying)
            return;

        dialoguePlaying = true;

        currentDialogue = dialogue;
        currentLineIndex = 0;
        currentTrigger = null;

        ShowDialogue();
        DisplayCurrentLine();
    }


    public void StartDialogue(DialogueSequence dialogue, DialogueTrigger trigger)
    {
        if (dialogueLocked)
            return;

        if (dialoguePlaying)
        {
            dialogueQueue.Enqueue(
                new QueuedDialogue(dialogue, trigger)
            );

            return;
        }

        dialoguePlaying = true;

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

        dialoguePlaying = false;

        if (finishedTrigger != null)
        {
            finishedTrigger.DialogueFinished();
        }

        if (dialogueLocked)
            return;

        if (dialogueQueue.Count > 0)
        {
            QueuedDialogue nextDialogue = dialogueQueue.Dequeue();

            StartDialogue(nextDialogue.dialogue, nextDialogue.trigger);
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
        if (DialogueState.Instance != null &&
            DialogueState.Instance.HasTriggered(openingDialogueID))
        {
            return;
        }

        if (DialogueState.Instance != null)
        {
            DialogueState.Instance.MarkTriggered(openingDialogueID);
        }

        StartDialogue(openingDialogue);
    }

    void OnDestroy()
    {
        CancelInvoke();
    }
}
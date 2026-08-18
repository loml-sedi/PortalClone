using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue")]
    public DialogueSequence dialogue;
    public DialogueManager dialogueManager;

    [Header("Trigger Settings")]
    public DialogueTriggerType triggerType = DialogueTriggerType.None;

    [Tooltip("If enabled, this dialogue can only trigger once.")]
    public bool triggerOnce = true;

    [Tooltip("Delay before the dialogue starts.")]
    public float dialogueDelay = 0f;

    [Header("Player Input")]
    [Tooltip("The player's Move action from the New Input System.")]
    public InputActionReference moveAction;

    [Header("Wait Too Long")]
    [Tooltip("How long the player must remain inactive before dialogue plays.")]
    public float waitTime = 10f;

    [Header("Dialogue After Completion")]
    [Tooltip("Optional dialogue trigger that activates after this dialogue finishes.")]
    public DialogueTrigger nextDialogueTrigger;

    private bool triggered = false;
    private bool playerInside = false;

    private Coroutine waitCoroutine;

    [Header("Dialogue ID")]
    [Tooltip("Unique ID used to remember whether this dialogue has already triggered.")]
    public string dialogueID;


    private void Start()
    {
        if (triggerType == DialogueTriggerType.WaitTooLong)
        {
            StartWaitTimer();
        }

        if (triggerType == DialogueTriggerType.StartLevel)
        {
            TriggerDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = true;

        switch (triggerType)
        {
            case DialogueTriggerType.EnterRoom:
                TriggerDialogue();
                break;

            case DialogueTriggerType.ExploreArea:
                StartExploreTimer();
                break;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = false;

        if (triggerType == DialogueTriggerType.ExploreArea)
        {
            StopWaitTimer();
        }
    }



    private void StartWaitTimer()
    {
        if (triggerOnce && triggered)
            return;

        if (waitCoroutine != null)
            return;

        waitCoroutine = StartCoroutine(WaitTooLongCoroutine());
    }


    private IEnumerator WaitTooLongCoroutine()
    {
        float inactiveTime = 0f;

        while (true)
        {
         
            if (triggerOnce && triggered)
            {
                waitCoroutine = null;
                yield break;
            }

            bool playerIsMoving = false;

       
            if (moveAction != null && moveAction.action != null)
            {
                Vector2 movement = moveAction.action.ReadValue<Vector2>();

                playerIsMoving = movement.sqrMagnitude > 0.01f;
            }

            if (playerIsMoving)
            {
                inactiveTime = 0f;
            }
            else
            {
                inactiveTime += Time.deltaTime;
            }

            if (inactiveTime >= waitTime)
            {
                waitCoroutine = null;

                TriggerDialogue();

                yield break;
            }

            yield return null;
        }
    }

    private void StartExploreTimer()
    {
        if (triggerOnce && triggered)
            return;

        if (waitCoroutine != null)
            return;

        waitCoroutine = StartCoroutine(ExploreAreaCoroutine());
    }


    private IEnumerator ExploreAreaCoroutine()
    {
        float timeInside = 0f;

        while (playerInside)
        {
            timeInside += Time.deltaTime;

            if (timeInside >= waitTime)
            {
                waitCoroutine = null;

                TriggerDialogue();

                yield break;
            }

            yield return null;
        }

        waitCoroutine = null;
    }


    private void StopWaitTimer()
    {
        if (waitCoroutine != null)
        {
            StopCoroutine(waitCoroutine);
            waitCoroutine = null;
        }
    }

    public void TriggerDialogue()
    {
        if (triggerOnce && triggered)
            return;

        if (triggerOnce &&
            !string.IsNullOrEmpty(dialogueID) &&
            DialogueState.Instance != null &&
            DialogueState.Instance.HasTriggered(dialogueID))
        {
            return;
        }

        if (dialogue == null)
        {
            Debug.LogWarning(
                $"DialogueTrigger on {gameObject.name} has no DialogueSequence assigned."
            );

            return;
        }

        if (dialogueManager == null)
        {
            Debug.LogWarning(
                $"DialogueTrigger on {gameObject.name} has no DialogueManager assigned."
            );

            return;
        }

        triggered = true;

        if (DialogueState.Instance != null &&
            !string.IsNullOrEmpty(dialogueID))
        {
            DialogueState.Instance.MarkTriggered(dialogueID);
        }

        if (dialogueDelay > 0f)
        {
            StartCoroutine(DelayedDialogue());
        }
        else
        {
            StartDialogue();
        }
    }


    private IEnumerator DelayedDialogue()
    {
        yield return new WaitForSeconds(dialogueDelay);

        StartDialogue();
    }


    private void StartDialogue()
    {
        dialogueManager.StartDialogue(dialogue, this);
    }


    public void DialogueFinished()
    {
        if (triggerType != DialogueTriggerType.WaitTooLong)
            return;

        if (nextDialogueTrigger != null)
        {
            nextDialogueTrigger.ForceTrigger();
        }
    }


    public void PressButton()
    {
        if (triggerType == DialogueTriggerType.PressButton)
        {
            TriggerDialogue();
        }
    }


    public void ActivatePortal()
    {
        if (triggerType == DialogueTriggerType.ActivatePortal)
        {
            TriggerDialogue();
        }
    }


    public void CompletePuzzle()
    {
        if (triggerType == DialogueTriggerType.CompletePuzzle)
        {
            TriggerDialogue();
        }
    }


    public void FailPuzzle()
    {
        if (triggerType == DialogueTriggerType.FailPuzzle)
        {
            TriggerDialogue();
        }
    }


    public void RetryPuzzle()
    {
        if (triggerType == DialogueTriggerType.RetryPuzzle)
        {
            TriggerDialogue();
        }
    }


    public void EndLevel()
    {
        if (triggerType == DialogueTriggerType.EndLevel)
        {
            TriggerDialogue();
        }
    }

    public void FirstGrab()
    {
        if (triggerType == DialogueTriggerType.FirstGrab)
        {
            TriggerDialogue();
        }
    }

    public void Teleport()
    {
        if (triggerType == DialogueTriggerType.Teleport)
        {
            TriggerDialogue();
        }
    }


    public void ResetTrigger()
    {
        triggered = false;

        StopWaitTimer();

        if (triggerType == DialogueTriggerType.WaitTooLong)
        {
            StartWaitTimer();
        }
    }

    public void ForceTrigger()
    {
        TriggerDialogue();
    }
}
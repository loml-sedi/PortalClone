using UnityEngine;

public class PuzzleTimeoutManager : MonoBehaviour
{
    [System.Serializable]
    public class TimeoutDialogue
    {
        public float time;
        public DialogueTrigger dialogueTrigger;
        public bool triggered;
    }

    [SerializeField] private TimeoutDialogue[] timeoutDialogues;

    private float timer;
    private bool puzzleCompleted;

    private void Update()
    {
        if (puzzleCompleted)
            return;

        timer += Time.deltaTime;

        foreach (TimeoutDialogue timeout in timeoutDialogues)
        {
            if (!timeout.triggered && timer >= timeout.time)
            {
                timeout.triggered = true;

                if (timeout.dialogueTrigger != null)
                {
                    timeout.dialogueTrigger.TriggerDialogue();
                }
            }
        }
    }

    public void CompletePuzzle()
    {
        puzzleCompleted = true;
    }

    public void ResetTimer()
    {
        timer = 0f;
        puzzleCompleted = false;

        foreach (TimeoutDialogue timeout in timeoutDialogues)
        {
            timeout.triggered = false;
        }
    }
}
using UnityEngine;

public class ExploreAreaTrigger : MonoBehaviour
{
    [SerializeField] private float inactivityTime = 30f;
    [SerializeField] private DialogueTrigger dialogueTrigger;

    private float timer;
    private bool triggered;

    private void Update()
    {
        if (triggered)
            return;

        timer += Time.deltaTime;

        if (timer >= inactivityTime)
        {
            TriggerDialogue();
        }
    }

    private void TriggerDialogue()
    {
        triggered = true;

        if (dialogueTrigger != null)
        {
            dialogueTrigger.TriggerDialogue();
        }
    }

    public void MeaningfulInteraction()
    {
        timer = 0f;
    }

    public void ResetExploreTimer()
    {
        timer = 0f;
        triggered = false;
    }
}
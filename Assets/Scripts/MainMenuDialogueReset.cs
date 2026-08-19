using UnityEngine;

public class MainMenuDialogueReset : MonoBehaviour
{
    private void Start()
    {
        if (DialogueState.Instance != null)
        {
            DialogueState.Instance.ResetAllDialogues();
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

public class DialogueState : MonoBehaviour
{
    public static DialogueState Instance { get; private set; }

    private HashSet<string> triggeredDialogues = new HashSet<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool HasTriggered(string id)
    {
        return triggeredDialogues.Contains(id);
    }

    public void MarkTriggered(string id)
    {
        if (!string.IsNullOrEmpty(id))
        {
            triggeredDialogues.Add(id);
        }
    }

    public void ResetDialogue(string id)
    {
        if (!string.IsNullOrEmpty(id))
        {
            triggeredDialogues.Remove(id);
        }
    }

    public void ResetAllDialogues()
    {
        triggeredDialogues.Clear();
    }
}
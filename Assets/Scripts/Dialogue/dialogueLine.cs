using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public DialogueSpeaker speaker;

    [TextArea(2, 5)]
    public string text;

    public float displayTime = 3f;
}
using UnityEngine;

public enum DialogueTriggerType
{
    None,

    EnterRoom,

    PressButton,
    ActivatePortal,

    FirstGrab,
    Teleport,

    CompletePuzzle,
    FailPuzzle,

    WaitTooLong,
    ExploreArea,
    RetryPuzzle,

    StartLevel,
    EndLevel
}
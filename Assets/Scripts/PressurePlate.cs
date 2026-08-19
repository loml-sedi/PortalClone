using UnityEngine;

public class PressurePlate : MonoBehaviour
{

    public DialogueTrigger endLevelDialogue;
    private void OnTriggerEnter2D(Collider2D collide)
    {
        if (collide.CompareTag("Box"))
        {
            Debug.Log("Level Complete");
            //Transition to next scene here!
            CompleteLevel();
        }
    }

    public void CompleteLevel()
    {
        if (endLevelDialogue != null)
        {
            endLevelDialogue.EndLevel();
        }
    }
}

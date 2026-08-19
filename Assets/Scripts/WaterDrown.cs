using UnityEngine;

public class WaterDrown : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";

    [Header("Drown Dialogue")]
    [SerializeField] private DialogueTrigger drownDialogue;

    private bool drowned = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (drowned)
            return;

        if (other.CompareTag(playerTag))
        {
            Drown();
        }
    }

    private void Drown()
    {
        drowned = true;

        Debug.Log("Player drowned!");

        if (drownDialogue != null)
        {
            drownDialogue.TriggerDrown(this);
        }
    }
}
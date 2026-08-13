using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform otherPortal;
    private bool canTeleport = true;

    public DialogueManager dialogueManager;
    public DialogueSequence teleportDialogue;
    private bool teleportDialogueTriggered = false;

    private void OnTriggerEnter2D(Collider2D collide)
    {
        if (!canTeleport) return;
        if (!collide.CompareTag("Player") && !collide.CompareTag("Box")) return;


        Rigidbody2D rb = collide.GetComponent<Rigidbody2D>();
        collide.transform.position = otherPortal.position;
        Vector2 velocity = rb.linearVelocity;
        
        rb.linearVelocity = velocity;

        canTeleport = false; //disables both portals

        Teleport destinationPortal = otherPortal.GetComponent<Teleport>();
        destinationPortal.canTeleport = false;

        if (!teleportDialogueTriggered && dialogueManager != null && teleportDialogue != null)
        {
            teleportDialogueTriggered = true;
            dialogueManager.StartDialogue(teleportDialogue);
        }

        Invoke(nameof(EnablePortal), 0.2f);
       destinationPortal.Invoke(nameof(EnablePortal), 0.2f);


    }

    private void EnablePortal()
    {
        canTeleport = true;
    }
}

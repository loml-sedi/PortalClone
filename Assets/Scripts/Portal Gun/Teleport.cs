using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform otherPortal;
    private bool canTeleport = true;

    [SerializeField] private PortalGun portalGun;

    public DialogueTrigger dialogueTrigger;
    public ExploreAreaTrigger exploreAreaTrigger;
    private void OnTriggerEnter2D(Collider2D collide)
    {
        if (!canTeleport) return;
        if (!collide.CompareTag("Player") && !collide.CompareTag("Box")) return;

        if (portalGun.bluePlaced && portalGun.orangePlaced)
        {
            Rigidbody2D rb = collide.GetComponent<Rigidbody2D>();
            collide.transform.position = otherPortal.position;
            Vector2 velocity = rb.linearVelocity;

            rb.linearVelocity = velocity;

            canTeleport = false; //disables both portals

            Teleport destinationPortal = otherPortal.GetComponent<Teleport>();
            destinationPortal.canTeleport = false;

            if (collide.CompareTag("Player") && dialogueTrigger != null)
            {
                dialogueTrigger.TriggerDialogue();
            }

            if (collide.CompareTag("Player"))
            {
                if (exploreAreaTrigger != null)
                {
                    exploreAreaTrigger.MeaningfulInteraction();
                }
            }

            Invoke(nameof(EnablePortal), 0.2f);
            destinationPortal.Invoke(nameof(EnablePortal), 0.2f);

        }


    }

    private void EnablePortal()
    {
        canTeleport = true;
    }
}

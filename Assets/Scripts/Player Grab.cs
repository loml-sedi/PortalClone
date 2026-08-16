using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrab : MonoBehaviour
{
    public Transform grabPoint;

    private GameObject nearbyObject;
    private GameObject grabbedObject;

    public DialogueManager dialogueManager;
    public DialogueSequence firstGrabDialogue;

    private bool firstGrabDialoguePlayed = false;

    private void OnTriggerEnter2D(Collider2D collide)
    {
        if (collide.CompareTag("Box"))
        {
            Debug.Log("Can pick up!");
            nearbyObject = collide.gameObject;
        }
    }

      private void OnTriggerExit2D(Collider2D collide)
    {
        if (collide.gameObject == nearbyObject)
        {
            nearbyObject = null;
        }
    }

    private void Update()
    {
        if (grabbedObject != null)
        {
            grabbedObject.transform.position = grabPoint.position;
        }
    }

    public void Grab(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Release();

        // Grab nearby object
        if (nearbyObject != null)
        {
            grabbedObject = nearbyObject;

            Rigidbody2D rb = grabbedObject.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.simulated = false;
            }

            grabbedObject.transform.position = grabPoint.position;

            if (!firstGrabDialoguePlayed && dialogueManager != null && firstGrabDialogue != null)
            {
                firstGrabDialoguePlayed = true;
                dialogueManager.StartDialogue(firstGrabDialogue);
            }
        }

    }

    public bool Release()
    {
        if (grabbedObject == null) return false;

        Rigidbody2D rb = grabbedObject.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.simulated = true;
        }

        grabbedObject = null;
        return true;
    }
}

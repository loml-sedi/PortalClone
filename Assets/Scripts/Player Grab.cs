using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrab : MonoBehaviour
{
    public Transform grabPoint;

    private GameObject nearbyObject;
    private GameObject grabbedObject;

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

        if (grabbedObject != null) //Release already grabbed object
        {
            grabbedObject.GetComponent<Rigidbody2D>().simulated = true;
            grabbedObject = null;
            return;   
        }

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
        }

    }
}

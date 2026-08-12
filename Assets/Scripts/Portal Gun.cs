using UnityEngine;
using UnityEngine.InputSystem;

public class PortalGun : MonoBehaviour
{
   public Camera camera;
   public GameObject bluePortal;
   public GameObject orangePortal;
   public LayerMask portalLayer;
   public Transform firePoint;

    public void shootBlue(InputAction.CallbackContext context)
    {
        Debug.Log("ShootBlue function called");
        if (!context.performed) return;
        ShootPortal(bluePortal);
    }

    public void shootOrange(InputAction.CallbackContext context)
    {
        Debug.Log("ShootOrange function called");
        if (!context.performed) return;
        ShootPortal(orangePortal);
    }


    private void ShootPortal(GameObject portal)
    {
        Debug.Log("ShootPortal started");
        Debug.Log("ShootPortal called with: " + portal.name);

        Vector2 mousePosition = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()); //Get mouse position
        Vector2 direction = mousePosition - (Vector2)camera.transform.position; //Get direction of mouse

        direction.Normalize();

        RaycastHit2D hit = Physics2D.Raycast(mousePosition, direction, 100f, portalLayer); //Detect the portal layer
 
        if (hit.collider != null)
        {
            Debug.Log("Hit: " + hit.collider.name);
            Debug.Log("Moving portal: " + portal.name);

            // portal.transform.position = hit.point + hit.normal * 0.05f;

            // float angle = Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg; //Calculate the angle

            // portal.transform.rotation = Quaternion.Euler(0,0,angle-90);
        Vector2 newPosition = hit.point + hit.normal * 0.1f;
 
        Debug.Log("New portal position: " + newPosition);

        portal.transform.position = newPosition;

        Debug.Log(
            "Actual portal position: " +
            portal.transform.position
        );

  
        }
        else
{
    Debug.Log("RAYCAST DID NOT HIT ANYTHING");
}
    }
}

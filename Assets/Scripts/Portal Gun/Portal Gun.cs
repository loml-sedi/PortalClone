using UnityEngine;
using UnityEngine.InputSystem;

public class PortalGun : MonoBehaviour
{
    public Camera camera;
    public GameObject bluePortal;
    public GameObject orangePortal;
    public LayerMask portalLayer;
    public LayerMask obstacleLayer;

    public Transform firePoint;
    public float portalDistance = 10f;

    public PlayerGrab playerGrab;
    public DialogueTrigger portalTrigger;
    public DialogueTrigger firstPortalTrigger;

    public void shootBlue(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (playerGrab.Release())
            return;

        ShootPortal(bluePortal);
    }

    public void shootOrange(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (playerGrab.Release())
            return;

        ShootPortal(orangePortal);
    }

    void Update()
    {
        Vector3 mousePos = camera.ScreenToWorldPoint(
            Mouse.current.position.ReadValue()
        );

        Vector3 direction = mousePos - transform.position;
        direction.z = 0;

        float angle = Mathf.Atan2(
            direction.y,
            direction.x
        ) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(
            new Vector3(0, 0, angle)
        );
    }

    private void ShootPortal(GameObject portal)
    {
        Vector2 mousePosition = camera.ScreenToWorldPoint(
            Mouse.current.position.ReadValue()
        );

        Vector2 direction = mousePosition - (Vector2)camera.transform.position;

        direction.Normalize();

        LayerMask raycastLayers = portalLayer | obstacleLayer;

        RaycastHit2D hit = Physics2D.Raycast(
            firePoint.position,
            direction,
            portalDistance,
            raycastLayers
        );

        Debug.Log("Portal distance: " + hit.distance);

        RaycastHit2D obstacleHit = Physics2D.Raycast(
            firePoint.position,
            direction,
            portalDistance,
            obstacleLayer
        );

        if (obstacleHit.collider != null)
        {
            Debug.Log(
                "OBSTACLE HIT: " +
                obstacleHit.collider.gameObject.name
            );

            return;
        }
        else
        {
            Debug.Log("NO OBSTACLE HIT");
        }

        if (hit.collider == null)
        {
            Debug.Log("No portalable surface found.");
            return;
        }

        if (((1 << hit.collider.gameObject.layer) & obstacleLayer) != 0)
        {
            Debug.Log("Obstacle hit - portal blocked!");
            return;
        }

        if (((1 << hit.collider.gameObject.layer) & portalLayer) != 0)
        {
            Debug.Log("Portal surface hit!");

            portal.transform.position = hit.point;

            if (portalTrigger != null)
            {
                portalTrigger.ActivatePortal();
            }
        }
    }
}
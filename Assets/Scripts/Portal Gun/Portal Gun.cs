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

    public DialogueManager dialogueManager;
    public DialogueSequence portalDialogue;
    public PlayerGrab playerGrab;
    private bool portalDialogueTriggered = false;

    public void shootBlue(InputAction.CallbackContext context)
    {
        if (!context.performed) return;


        if (playerGrab.Release()) return;
        

        ShootPortal(bluePortal);
    }

    public void shootOrange(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (playerGrab.Release()) return;
        ShootPortal(orangePortal);
    }

    void Update()
    {
        Vector3 mousePos = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()); //Gun to follow mouse
        Vector3 direction = mousePos - transform.position;
        direction.z = 0;
        

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }


    private void ShootPortal(GameObject portal)
    {
        Vector2 mousePosition = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()); //Get mouse position
        Vector2 direction = mousePosition - (Vector2)camera.transform.position; //Get direction of mouse

        direction.Normalize();

        LayerMask raycastLayers = portalLayer | obstacleLayer;

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, direction, portalDistance, raycastLayers); //Detect the portal layer
        Debug.Log("Portal distance: "+hit.distance);
        RaycastHit2D obstacleHit = Physics2D.Raycast(firePoint.position, direction, portalDistance, obstacleLayer);//Detects obstacle
        //Debug.Log("Obstacle distance: "+obstacleHit.distance);

if (obstacleHit.collider != null)
{
    Debug.Log("OBSTACLE HIT: " + obstacleHit.collider.gameObject.name);
    return; //Once it hits the obstacle, nothing will happen
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

        if (hit.collider != null)
        {
         if (((1 << hit.collider.gameObject.layer) & obstacleLayer) != 0)
    {
        // Hit an obstacle - don't place portal
        Debug.Log("Obstacle hit - portal blocked!");
        return;
    }

    if (((1 << hit.collider.gameObject.layer) & portalLayer) != 0)
    {
        // Hit a valid portal surface
         Debug.Log("Portal surface hit!");
        portal.transform.position = hit.point;

            if (!portalDialogueTriggered &&dialogueManager != null &&portalDialogue != null)
            {
                    portalDialogueTriggered = true;
                    dialogueManager.StartDialogue(portalDialogue);
            }
        }

  
        }
        else
    {
    Debug.Log("RAYCAST DID NOT HIT ANYTHING");
    }
    }
}

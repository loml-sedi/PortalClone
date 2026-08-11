using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 5f;

    private float horizontalMovement;

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(
            horizontalMovement * moveSpeed,
            rb.linearVelocity.y
        );
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<float>();
         Debug.Log("Movement: " + horizontalMovement);
    }
}

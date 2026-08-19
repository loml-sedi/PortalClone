using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private BoxCollider2D cameraBounds;

    [Header("Camera Settings")]
    [SerializeField] private float smoothTime = 0.15f;

    private Vector3 velocity;
    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 targetPosition = target.position;

        // Keep camera behind the 2D scene
        targetPosition.z = transform.position.z;

        if (cameraBounds != null)
        {
            Bounds bounds = cameraBounds.bounds;

            float halfHeight = cam.orthographicSize;
            float halfWidth = halfHeight * cam.aspect;

            float minX = bounds.min.x + halfWidth;
            float maxX = bounds.max.x - halfWidth;

            float minY = bounds.min.y + halfHeight;
            float maxY = bounds.max.y - halfHeight;

            targetPosition.x = Mathf.Clamp(
                targetPosition.x,
                minX,
                maxX
            );

            targetPosition.y = Mathf.Clamp(
                targetPosition.y,
                minY,
                maxY
            );
        }

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime
        );
    }
}
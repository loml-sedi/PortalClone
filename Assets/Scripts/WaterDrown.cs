using UnityEngine;

public class WaterDrown : MonoBehaviour
{
    [SerializeField] string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            Drown();
        }
    }

    private void Drown()
    {
        Debug.Log("Player drowned! Game paused.");
        Time.timeScale = 0f;
    }
}
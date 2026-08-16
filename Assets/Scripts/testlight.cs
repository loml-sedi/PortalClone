using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(AudioSource))]
public class BulbLightFlicker : MonoBehaviour
{
    [SerializeField] float minIntensity;
    [SerializeField] float maxIntensity;
    [SerializeField] float secondsBetweenFlickers;
    [SerializeField] AudioClip flickerSound;

    Light2D myLight;
    AudioSource audioSource;

    private void Start()
    {
        myLight = GetComponent<Light2D>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(LightFlicker());
    }

    IEnumerator LightFlicker()
    {
        yield return new WaitForSeconds(secondsBetweenFlickers);
        myLight.intensity = Random.Range(minIntensity, maxIntensity);

        if (flickerSound != null)
        {
            audioSource.PlayOneShot(flickerSound);
        }

        StartCoroutine(LightFlicker());
    }
}
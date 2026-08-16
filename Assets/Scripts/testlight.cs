using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BulbLightFlicker : MonoBehaviour
{
    [SerializeField] float minIntensity;
    [SerializeField] float maxIntensity;
    [SerializeField] float secondsBetweenFlickers;

    [Header("Audio")]
    [SerializeField] AudioClip flickerSound;
    [SerializeField] AudioClip humLoopSound;
    [SerializeField] AudioSource flickerAudioSource;
    [SerializeField] AudioSource humAudioSource;

    Light2D myLight;

    private void Start()
    {
        myLight = GetComponent<Light2D>();

        if (humLoopSound != null && humAudioSource != null)
        {
            humAudioSource.clip = humLoopSound;
            humAudioSource.loop = true;
            humAudioSource.playOnAwake = false;
            humAudioSource.Play();
        }

        StartCoroutine(LightFlicker());
    }

    IEnumerator LightFlicker()
    {
        yield return new WaitForSeconds(secondsBetweenFlickers);
        myLight.intensity = Random.Range(minIntensity, maxIntensity);

        if (flickerSound != null && flickerAudioSource != null)
        {
            flickerAudioSource.PlayOneShot(flickerSound);
        }

        StartCoroutine(LightFlicker());
    }
}
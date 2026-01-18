using UnityEngine;

public class HallucinationAudio : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip[] fakeSounds; 

    [Header("Settings")]
    public float minInterval = 5f; 
    public float maxInterval = 15f;
    private AudioSource audioSource;
    private float nextSoundTime = 0f;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 0f; 
        audioSource.playOnAwake = false;

        PlanNextSound();
    }

    void Update()
    {
        if (PanicSystem.instance.currentHeartRate > 100f)
        {
            if (Time.time >= nextSoundTime)
            {
                PlayRandomSound();
                PlanNextSound();
            }
        }
    }

    void PlayRandomSound()
    {
        if (fakeSounds.Length == 0) return;

        AudioClip clip = fakeSounds[Random.Range(0, fakeSounds.Length)];

        float panicFactor = Mathf.InverseLerp(100f, 180f, PanicSystem.instance.currentHeartRate);
        float volume = Mathf.Lerp(0.3f, 1.0f, panicFactor);

        audioSource.PlayOneShot(clip, volume);
    }

    void PlanNextSound()
    {

        float panicFactor = Mathf.InverseLerp(100f, 180f, PanicSystem.instance.currentHeartRate);
        float currentMin = Mathf.Lerp(maxInterval, minInterval, panicFactor);

        nextSoundTime = Time.time + Random.Range(minInterval, currentMin);
    }
}
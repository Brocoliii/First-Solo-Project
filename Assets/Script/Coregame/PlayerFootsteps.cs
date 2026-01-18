using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerFootsteps : MonoBehaviour
{
    [Header("Settings")]
    public float stepInterval = 0.5f; 
    public AudioClip[] stepSounds;   
    private AudioSource audioSource;
    private float nextStepTime;
    private Vector3 lastPosition;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        lastPosition = transform.position;
    }

    void Update()
    {
        float speed = Vector3.Distance(transform.position, lastPosition) / Time.deltaTime;
        lastPosition = transform.position;

        if (speed > 0.1f && Time.time >= nextStepTime)
        {
            PlayStep();
            nextStepTime = Time.time + stepInterval;
        }
    }

    void PlayStep()
    {
        if (stepSounds.Length > 0)
        {
            AudioClip clip = stepSounds[Random.Range(0, stepSounds.Length)];

            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(clip);
        }
    }
}
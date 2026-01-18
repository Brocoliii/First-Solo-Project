using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PlayerFocus : MonoBehaviour
{
    [Header("UI Settings")]
    public Image focusReticle;
    public float focusSpeed = 1f;

    [Header("Raycast Settings")]
    public float focusDistance = 5f;
    public LayerMask focusLayer;

    [Header("Sound")]
    public AudioClip focusLoopSound; 
    public AudioClip completeSound;  

    private float currentFocusTime = 0f;
    private FocusTarget currentTarget;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (focusReticle != null) focusReticle.fillAmount = 0f;
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        bool hitSomething = Physics.Raycast(ray, out hit, focusDistance, focusLayer);

        if (hitSomething && hit.collider.GetComponent<FocusTarget>())
        {
            currentTarget = hit.collider.GetComponent<FocusTarget>();

            if (Input.GetButton("Fire2") && !currentTarget.isCompleted)
            {
                if (!audioSource.isPlaying && focusLoopSound != null)
                {
                    audioSource.clip = focusLoopSound;
                    audioSource.loop = true;
                    audioSource.Play();
                }

                currentFocusTime += Time.deltaTime * focusSpeed;

                if (focusReticle != null)
                    focusReticle.fillAmount = currentFocusTime / currentTarget.timeToFocus;

                if (currentFocusTime >= currentTarget.timeToFocus)
                {
                    if (completeSound != null) AudioSource.PlayClipAtPoint(completeSound, transform.position);

                    currentTarget.OnFocusComplete();
                    ResetFocus();
                }
            }
            else
            {
                ResetFocus();
            }
        }
        else
        {
            ResetFocus();
            currentTarget = null;
        }
    }

    void ResetFocus()
    {
        currentFocusTime = 0f;

        if (audioSource.isPlaying && audioSource.clip == focusLoopSound) audioSource.Stop();

        if (focusReticle != null)
            focusReticle.fillAmount = Mathf.Lerp(focusReticle.fillAmount, 0f, Time.deltaTime * 10f);
    }
}
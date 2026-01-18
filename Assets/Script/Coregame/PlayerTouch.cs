using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))] 
public class PlayerTouch : MonoBehaviour
{
    [Header("Setup")]
    public Image rubProgressBar;
    public PlayerMovement cameraScript;
    public float reachDistance = 5f;
    public LayerMask touchLayer;

    [Header("Sound")]
    public AudioClip rubSoundLoop; 

    private RubbableObject currentTarget;
    private bool isRubbing = false;
    private AudioSource audioSource;

    void Start()
    {
        if (rubProgressBar != null) rubProgressBar.fillAmount = 0;
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true; 
        audioSource.clip = rubSoundLoop;
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, reachDistance, touchLayer))
        {
            RubbableObject obj = hit.collider.GetComponent<RubbableObject>();
            if (obj != null && !obj.isComplete)
            {
                currentTarget = obj;
                HandleRubbing();
                return;
            }
        }
        StopRubbing();
    }

    void HandleRubbing()
    {
        if (Input.GetButton("Fire1"))
        {
            isRubbing = true;
            if (cameraScript != null) cameraScript.enabled = false;

            float mouseX = Mathf.Abs(Input.GetAxis("Mouse X"));
            float mouseY = Mathf.Abs(Input.GetAxis("Mouse Y"));
            float rubAmount = mouseX + mouseY;

            if (rubAmount > 0.1f)
            {
                currentTarget.AddRub(rubAmount);

                if (!audioSource.isPlaying) audioSource.Play();
            }
            else
            {
                if (audioSource.isPlaying) audioSource.Pause();
            }

            if (rubProgressBar != null)
                rubProgressBar.fillAmount = currentTarget.currentRubProgress / currentTarget.amountToRub;
        }
        else
        {
            StopRubbing();
        }
    }

    void StopRubbing()
    {
        if (isRubbing)
        {
            isRubbing = false;
            if (cameraScript != null) cameraScript.enabled = true;
            audioSource.Stop();
        }

        if (rubProgressBar != null)
            rubProgressBar.fillAmount = Mathf.Lerp(rubProgressBar.fillAmount, 0f, Time.deltaTime * 5f);
    }
}
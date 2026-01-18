using UnityEngine;
using UnityEngine.UI; // ต้องใช้เพื่อคุมภาพ UI

public class PanicVisuals : MonoBehaviour
{
    [Header("Camera Shake (อาการมือสั่น)")]
    public Transform cameraTransform;
    public float shakeIntensity = 0.5f;
    public float shakeSpeed = 10f; 

    [Header("Blackout Effect (อาการหน้ามืด)")]
    public Image blackoutImage; 
    [Range(0, 1)]
    public float maxDarkness = 0.8f; 

    private Vector3 originalLocalPos;

    void Start()
    {
        if (cameraTransform != null)
            originalLocalPos = cameraTransform.localPosition;
    }

    void Update()
    {
        float heartRate = PanicSystem.instance.currentHeartRate;

        float severity = Mathf.InverseLerp(60f, 180f, heartRate);

        if (cameraTransform != null)
        {
            float x = (Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f) * 2f;
            float y = (Mathf.PerlinNoise(0f, Time.time * shakeSpeed) - 0.5f) * 2f;

            Vector3 shakeOffset = new Vector3(x, y, 0) * shakeIntensity * severity;
            cameraTransform.localPosition = originalLocalPos + shakeOffset;
        }

        if (blackoutImage != null)
        {
            Color color = blackoutImage.color;
            color.a = severity * maxDarkness;
            blackoutImage.color = color;
        }
    }
}
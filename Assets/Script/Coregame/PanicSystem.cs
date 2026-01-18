using UnityEngine;

public class PanicSystem : MonoBehaviour
{
    public static PanicSystem instance;

    [Header("Heart Rate Settings")]
    [Tooltip("อัตราการเต้นหัวใจปัจจุบัน")]
    public float currentHeartRate = 80f; 

    [Tooltip("ค่าสูงสุดที่รับได้ (ถ้าเกิน = ตาย/Game Over)")]
    public float maxHeartRate = 180f;

    [Tooltip("ค่าต่ำสุด (หัวใจคนเราไม่ควรต่ำกว่านี้)")]
    public float minHeartRate = 60f;

    [Header("Game State")]
    public bool isPanicAttack = false; 

    void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) 
        {
            ChangeHeartRate(10f);
        }
        if (Input.GetKeyDown(KeyCode.L)) 
        {
            ChangeHeartRate(-10f);
        }

        CheckPanicState();
    }

    public void ChangeHeartRate(float amount)
    {
        currentHeartRate += amount;

        currentHeartRate = Mathf.Clamp(currentHeartRate, minHeartRate, maxHeartRate);

        Debug.Log("Heart Rate: " + currentHeartRate); 
    }

    void CheckPanicState()
    {
        if (currentHeartRate >= 120f)
        {
            isPanicAttack = true;
        }
        else
        {
            isPanicAttack = false;
        }

        if (currentHeartRate >= maxHeartRate)
        {
            Debug.Log("GAME OVER: Heart Attack!");
        }
    }
}
using UnityEngine;

public class FocusTarget : MonoBehaviour
{
    [Header("Settings")]
    public float timeToFocus = 3.0f; 
    public float panicReduction = 20f;

    [HideInInspector]
    public bool isCompleted = false; 

    public void OnFocusComplete()
    {
        if (isCompleted) return;

        GameManeger.instance.CompleteObjective("Sight (มอง)");

        isCompleted = true;
        Debug.Log("Focus Complete: " + gameObject.name);

        // 1. ลดค่า Panic
        PanicSystem.instance.ChangeHeartRate(-panicReduction);

        // 2. (Optional) เอฟเฟกต์ตอนเสร็จ เช่น วัตถุหายไป หรือมีเสียงติ๊ง
        // Destroy(gameObject); // ถ้าอยากให้หายไปเลยให้เปิดบรรทัดนี้

        // ปิดการทำงาน (จะได้ไม่มาจ้องซ้ำ)
        this.enabled = false;
    }
}
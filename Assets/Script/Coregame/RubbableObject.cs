using UnityEngine;

public class RubbableObject : MonoBehaviour
{
    [Header("Settings")]
    public float sensitivity = 0.5f; 
    public float amountToRub = 100f; 
    public float panicReduction = 30f; 

    [HideInInspector]
    public float currentRubProgress = 0f;
    [HideInInspector]
    public bool isComplete = false;

    public void AddRub(float amount)
    {
        if (isComplete) return;

        currentRubProgress += amount * sensitivity;

        if (currentRubProgress >= amountToRub)
        {
            GameManeger.instance.CompleteObjective("Touch (สัมผัส)");
            CompleteRub();
        }
    }

    void CompleteRub()
    {
        isComplete = true;
        Debug.Log("Rubbing Complete!");

        // ลดค่า Panic
        PanicSystem.instance.ChangeHeartRate(-panicReduction);

        // (Optional) อาจจะใส่เสียง Effect ตรงนี้ เช่นเสียงผ้า หรือเสียงถอนหายใจเฮือกใหญ่
    }
}
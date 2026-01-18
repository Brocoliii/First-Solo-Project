using UnityEngine;
using System.Collections.Generic; 

public class ItemRandomizer : MonoBehaviour
{
    [Header("ของสำคัญ 5 อย่าง (Key Items)")]
    public Transform[] keyItems;

    [Header("จุดเกิดที่เป็นไปได้ (Spawn Points)")]
    public Transform[] spawnPoints; 

    void Start()
    {
        ShuffleAndSpawn();
    }

    void ShuffleAndSpawn()
    {
        if (spawnPoints.Length < keyItems.Length)
        {
            Debug.LogError("Error: จุดเกิด (Spawn Points) น้อยกว่าจำนวนของ! สร้างเพิ่มด่วน");
            return;
        }

        List<Transform> availablePoints = new List<Transform>(spawnPoints);

        foreach (Transform item in keyItems)
        {
            if (item == null) continue;

            int randomIndex = Random.Range(0, availablePoints.Count);
            Transform randomPoint = availablePoints[randomIndex];

            item.position = randomPoint.position;
            item.rotation = randomPoint.rotation; 
            Debug.Log(item.name + " ย้ายไปเกิดที่ " + randomPoint.name);

            availablePoints.RemoveAt(randomIndex);
        }
    }
}
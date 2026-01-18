using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicBox : MonoBehaviour
{
    [Header("Settings")]
    public float healRadius = 4f;
    public float healRate = 20f;
    public float targetHeartRate = 80f; 

    [Header("Limited Use")]
    public bool isOneTimeUse = true;
    public float batteryLife = 10f;

    private Transform player;
    private AudioSource audioSource;
    private bool isBroken = false;
    private bool isCompleted = false; 

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
        else player = Camera.main.transform;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (player == null || isBroken) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= healRadius)
        {
            PanicSystem.instance.ChangeHeartRate(-healRate * Time.deltaTime);

            if (!isCompleted && PanicSystem.instance.currentHeartRate <= targetHeartRate)
            {
                isCompleted = true;
                GameManeger.instance.CompleteObjective("Sound (การได้ยิน)");
                Debug.Log("Sound Objective Complete!");
            }

            if (isOneTimeUse)
            {
                batteryLife -= Time.deltaTime;
                if (batteryLife <= 0) BreakRadio();
            }
        }
    }

    void BreakRadio()
    {
        isBroken = true;
        audioSource.Stop();
        // audioSource.PlayOneShot(brokenSound); // ถ้ามีเสียงพัง
        Debug.Log("Radio Battery Dead!");
    }

    void OnDrawGizmos()
    {
        Gizmos.color = isBroken ? Color.red : Color.cyan;
        Gizmos.DrawWireSphere(transform.position, healRadius);
    }
}
using UnityEngine;

public class BreathingMinigame : MonoBehaviour
{
    [Header("UI Objects")]
    public Transform guideCircle;  
    public Transform playerCircle;

    [Header("Settings")]
    public float breathSpeed = 2.0f; 
    public float sizeMin = 0.5f;     
    public float sizeMax = 1.5f;     
    public float panicReductionRate = 10f; 

    private float currentGuideTime = 0f;
    private float currentPlayerScale = 0.5f;

    void OnEnable()
    {
        currentPlayerScale = sizeMin;
    }

    void Update()
    {
        float wave = Mathf.PingPong(Time.time * breathSpeed, 1f);
        float targetScale = Mathf.Lerp(sizeMin, sizeMax, wave);
        guideCircle.localScale = Vector3.one * targetScale;

        if (Input.GetKey(KeyCode.Space))
        {
            currentPlayerScale += Time.deltaTime * breathSpeed; 
        }
        else
        {
            currentPlayerScale -= Time.deltaTime * breathSpeed; 
        }

        currentPlayerScale = Mathf.Clamp(currentPlayerScale, sizeMin, sizeMax);
        playerCircle.localScale = Vector3.one * currentPlayerScale;

        if (Mathf.Abs(currentPlayerScale - targetScale) < 0.2f)
        {
            playerCircle.GetComponent<UnityEngine.UI.Image>().color = Color.green;

            PanicSystem.instance.ChangeHeartRate(-panicReductionRate * Time.deltaTime);
        }
        else
        {
            playerCircle.GetComponent<UnityEngine.UI.Image>().color = Color.red;
        }

        if (Input.GetKeyDown(KeyCode.Escape) || PanicSystem.instance.currentHeartRate <= 70)
        {
            CloseMinigame();
        }
    }

    public void CloseMinigame()
    {
        gameObject.SetActive(false); 
    }
}
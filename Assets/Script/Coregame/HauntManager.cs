using UnityEngine;
using System.Collections;

public class HauntManager : MonoBehaviour
{
    [Header("Settings")]
    public float minTime = 15f;
    public float maxTime = 40f;

    [Header("1. กลุ่มเสียง ")]
    public AudioSource soundEffectSource; 
    public AudioClip[] knockDoorClips;   
    public AudioClip[] knockWindowClips;  
    public AudioClip[] footstepClips;     
    public AudioClip[] objectFallSoundClips; 
    public AudioClip[] whisperClips;     

    [Header("2. กลุ่มของตก ")]
    public Rigidbody[] thingsToFall; 

    [Header("3. กลุ่มไฟและฟ้าผ่า")]
    public Light[] roomLights; 
    public AudioClip thunderSound;
    public GameObject lightningFlash; 

    [Header("4. กลุ่มประตูผีสิง")]
    public Transform hauntedDoor; 
    public AudioClip doorSlamSound; 

    [Header("5. กลุ่มเงาปริศนา")]
    public GameObject shadowFigure; 
    public Transform[] shadowSpawnPoints; 

    void Start()
    {
        StartCoroutine(HauntLoop());

        if (shadowFigure != null) shadowFigure.SetActive(false);
        if (lightningFlash != null) lightningFlash.SetActive(false);
    }

    IEnumerator HauntLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(waitTime);

            int eventIndex = Random.Range(1, 11); 
            Debug.Log("Event Number: " + eventIndex);

            if (PanicSystem.instance.currentHeartRate > 150) continue;

            switch (eventIndex)
            {
                case 1: PlaySound(knockDoorClips, 1f); Debug.Log("เคาะประตู"); break;
                case 2: PlaySound(knockWindowClips, 0.8f); Debug.Log("เคาะหน้าต่าง"); break;
                case 3: PlaySound(footstepClips, 0.5f); Debug.Log("เสียงเดิน"); break;
                case 4: PlaySound(objectFallSoundClips, 1f); Debug.Log("เสียงของตก (หลอก)"); break;
                case 5: TriggerFallingObject(); Debug.Log("ของตกจริง!"); break;
                case 6: StartCoroutine(FlickerLights()); Debug.Log("ไฟกระพริบ"); break;
                case 7: PlaySound(whisperClips, 1.0f); Debug.Log("เสียงกระซิบ"); break;
                case 8: StartCoroutine(OpenCloseDoor()); Debug.Log("ประตูผีสิง"); break;
                case 9: StartCoroutine(ShowShadow()); Debug.Log("เงาแวบๆ"); break;
                case 10: StartCoroutine(LightningStrike()); Debug.Log("ฟ้าผ่า"); break;
            }
        }
    }


    void PlaySound(AudioClip[] clips, float volume)
    {
        if (clips.Length > 0 && soundEffectSource != null)
        {
            AudioClip clip = clips[Random.Range(0, clips.Length)];
            soundEffectSource.PlayOneShot(clip, volume);
        }
    }

    void TriggerFallingObject()
    {
        if (thingsToFall.Length > 0)
        {
            Rigidbody rb = thingsToFall[Random.Range(0, thingsToFall.Length)];
            if (rb != null)
            {
                rb.useGravity = true; 
                rb.AddForce(Random.insideUnitSphere * 200f); 
            }
        }
    }

    IEnumerator FlickerLights()
    {
        foreach (var light in roomLights) light.enabled = false;
        yield return new WaitForSeconds(0.1f);
        foreach (var light in roomLights) light.enabled = true;
        yield return new WaitForSeconds(0.1f);
        foreach (var light in roomLights) light.enabled = false;
        yield return new WaitForSeconds(2.0f); 
        foreach (var light in roomLights) light.enabled = true;
    }

    IEnumerator OpenCloseDoor()
    {
        if (hauntedDoor != null)
        {
            Quaternion originalRot = hauntedDoor.rotation;
            Quaternion targetRot = originalRot * Quaternion.Euler(0, 45, 0); 

            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * 2;
                hauntedDoor.rotation = Quaternion.Slerp(originalRot, targetRot, t);
                yield return null;
            }

            yield return new WaitForSeconds(1f); 

            hauntedDoor.rotation = originalRot;
            if (soundEffectSource != null && doorSlamSound != null)
                soundEffectSource.PlayOneShot(doorSlamSound);
        }
    }

    IEnumerator ShowShadow()
    {
        if (shadowFigure != null && shadowSpawnPoints.Length > 0)
        {
           
            Transform spawn = shadowSpawnPoints[Random.Range(0, shadowSpawnPoints.Length)];
            shadowFigure.transform.position = spawn.position;
            shadowFigure.transform.rotation = spawn.rotation;

            shadowFigure.SetActive(true); 
            yield return new WaitForSeconds(1.5f); 
            shadowFigure.SetActive(false); 
        }
    }

    IEnumerator LightningStrike()
    {
        if (soundEffectSource != null && thunderSound != null)
            soundEffectSource.PlayOneShot(thunderSound);

        if (lightningFlash != null)
        {
            lightningFlash.SetActive(true); 
            yield return new WaitForSeconds(0.1f);
            lightningFlash.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            lightningFlash.SetActive(true); 
            yield return new WaitForSeconds(0.2f);
            lightningFlash.SetActive(false);
        }
    }
}